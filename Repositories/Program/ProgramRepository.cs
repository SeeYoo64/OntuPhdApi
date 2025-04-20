using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using OntuPhdApi.Controllers;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Programs.Components;
using OntuPhdApi.Repositories.Institutes;
using OntuPhdApi.Utilities;

namespace OntuPhdApi.Repositories.Program
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly AppDbContext _context;
        private readonly IInstituteRepository _instituteRepository;
        private readonly ILogger<ProgramRepository> _logger;

        public ProgramRepository(
            AppDbContext context,
            IInstituteRepository instituteRepository,
            ILogger<ProgramRepository> logger)
        {
            _context = context;
            _instituteRepository = instituteRepository;
            _logger = logger;
        }

        public async Task<Institute> GetOrCreateInstituteAsync(string instituteName)
        {
            _logger.LogInformation("Fetching or creating institute with name {InstituteName}.", instituteName);
            var institute = await _context.Institutes
                .FirstOrDefaultAsync(i => i.Name == instituteName);

            if (institute == null)
            {
                institute = new Institute { Name = instituteName };
                institute.Id = await _instituteRepository.InsertInstituteAsync(institute);
                _logger.LogInformation("Created new institute with name {InstituteName} and ID {InstituteId}.", instituteName, institute.Id);
            }

            return institute;
        }

        public async Task<IEnumerable<ProgramModel>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all programs from database.");
            return await _context.Programs
                .Include(p => p.FieldOfStudy)
                .Include(p => p.Speciality)
                .Include(p => p.Institute)
                .Include(p => p.LinkFaculties)
                .Include(p => p.ProgramDocument)
                .Include(p => p.ProgramCharacteristics)
                    .ThenInclude(pc => pc.Area)
                .Include(p => p.ProgramCompetence)
                    .ThenInclude(pc => pc.OverallCompetences)
                .Include(p => p.ProgramCompetence)
                    .ThenInclude(pc => pc.SpecialCompetences)
                .Include(p => p.ProgramComponents)
                    .ThenInclude(pc => pc.ControlForms)
                .Include(p => p.Jobs)
                .OrderBy(p => p.Degree == "phd" ? 0 : 1)
                .ThenBy(p => p.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProgramModel>> GetByDegreeAsync(string degree)
        {
            _logger.LogInformation("Fetching programs for degree {Degree}.", degree?.ToString() ?? "all");
            return await _context.Programs
                .Where(p => p.Degree == degree)
                .ToListAsync();
        }

        public async Task<ProgramModel> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching program with ID {ProgramId}.", id);
            return await _context.Programs
                .Include(p => p.FieldOfStudy)
                .Include(p => p.Speciality)
                .Include(p => p.Institute)
                .Include(p => p.LinkFaculties)
                .Include(p => p.ProgramDocument)
                .Include(p => p.ProgramCharacteristics)
                    .ThenInclude(pc => pc.Area)
                .Include(p => p.ProgramCompetence)
                    .ThenInclude(pc => pc.OverallCompetences)
                .Include(p => p.ProgramCompetence)
                    .ThenInclude(pc => pc.SpecialCompetences)
                .Include(p => p.ProgramComponents)
                    .ThenInclude(pc => pc.ControlForms)
                .Include(p => p.Jobs)
                .FirstOrDefaultAsync(p => p.Id == id);

        }


        public async Task AddAsync(ProgramModel program)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Temporarily detach related entities to save ProgramModel first
                var programCharacteristics = program.ProgramCharacteristics;
                var programCompetence = program.ProgramCompetence;
                var programComponents = program.ProgramComponents;
                var jobs = program.Jobs;
                var linkFaculties = program.LinkFaculties;
                var institute = program.Institute;
                var fieldOfStudy = program.FieldOfStudy;
                var speciality = program.Speciality;

                program.FieldOfStudy = null;
                program.Speciality = null;
                program.Institute = null;
                program.ProgramCharacteristics = null;
                program.ProgramCompetence = null;
                program.ProgramComponents = null;
                program.Jobs = null;
                program.LinkFaculties = null;
                
                // Save ProgramModel first to generate its Id
                await _context.Programs.AddAsync(program);
                await _context.SaveChangesAsync();

                // Handle FieldOfStudy
                if (fieldOfStudy != null)
                {
                    var existingFieldOfStudy = await _context.FieldOfStudies
                        .FirstOrDefaultAsync(f => f.Code.ToLower() == fieldOfStudy.Code.ToLower());

                    if (existingFieldOfStudy != null)
                    {
                        program.FieldOfStudyId = existingFieldOfStudy.Id;
                    }
                    else
                    {
                        await _context.FieldOfStudies.AddAsync(fieldOfStudy);
                        await _context.SaveChangesAsync();
                        program.FieldOfStudyId = fieldOfStudy.Id;
                    }
                }

                // Handle Speciality
                if (speciality != null)
                {
                    var existingSpeciality = await _context.Specialities
                        .FirstOrDefaultAsync(s => s.Code.ToLower() == speciality.Code.ToLower());

                    if (existingSpeciality != null)
                    {
                        program.SpecialityId = existingSpeciality.Id;
                    }
                    else
                    {
                        speciality.FieldOfStudyId = (int)program.FieldOfStudyId;
                        await _context.Specialities.AddAsync(speciality);
                        await _context.SaveChangesAsync();
                        program.SpecialityId = speciality.Id;
                    }
                }


                // Reattach and update ProgramId for ProgramCharacteristics
                if (programCharacteristics != null)
                {
                    programCharacteristics.ProgramId = program.Id;
                    if (programCharacteristics.Area != null)
                    {
                        // Temporarily detach Area
                        var area = programCharacteristics.Area;
                        programCharacteristics.Area = null;
                        await _context.ProgramCharacteristics.AddAsync(programCharacteristics);
                        await _context.SaveChangesAsync();

                        // Reattach Area with correct ProgramCharacteristicsId
                        area.ProgramCharacteristicsId = programCharacteristics.Id;
                        await _context.Areas.AddAsync(area);
                    }
                    else
                    {
                        await _context.ProgramCharacteristics.AddAsync(programCharacteristics);
                    }
                    await _context.SaveChangesAsync();
                }

                // Reattach and update ProgramId for ProgramCompetence
                if (programCompetence != null)
                {
                    programCompetence.ProgramId = program.Id;
                    var overallCompetences = programCompetence.OverallCompetences ?? new List<OverallCompetence>();
                    var specialCompetences = programCompetence.SpecialCompetences ?? new List<SpecialCompetence>();

                    // Temporarily detach OverallCompetences and SpecialCompetences
                    programCompetence.OverallCompetences = null;
                    programCompetence.SpecialCompetences = null;
                    await _context.ProgramCompetences.AddAsync(programCompetence);
                    await _context.SaveChangesAsync();

                    // Reattach OverallCompetences and SpecialCompetences
                    foreach (var oc in overallCompetences)
                    {
                        oc.ProgramCompetenceId = programCompetence.Id;
                        await _context.OverallCompetences.AddAsync(oc);
                    }
                    foreach (var sc in specialCompetences)
                    {
                        sc.ProgramCompetenceId = programCompetence.Id;
                        await _context.SpecialCompetences.AddAsync(sc);
                    }
                    await _context.SaveChangesAsync();
                }

                // Reattach and update ProgramId for ProgramComponents
                if (programComponents != null && programComponents.Any())
                {
                    foreach (var component in programComponents)
                    {
                        component.ProgramId = program.Id;
                        var controlForms = component.ControlForms ?? new List<ControlForm>();
                        component.ControlForms = null;
                        await _context.ProgramComponents.AddAsync(component);
                        await _context.SaveChangesAsync();

                        foreach (var cf in controlForms)
                        {
                            cf.ProgramComponentId = component.Id;
                            await _context.ControlForms.AddAsync(cf);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                // Reattach and update ProgramId for Jobs
                if (jobs != null && jobs.Any())
                {
                    foreach (var job in jobs)
                    {
                        job.ProgramId = program.Id;
                        await _context.Jobs.AddAsync(job);
                    }
                    await _context.SaveChangesAsync();
                }

                // Reattach and update ProgramId for LinkFaculties
                if (linkFaculties != null && linkFaculties.Any())
                {
                    foreach (var faculty in linkFaculties)
                    {
                        faculty.ProgramId = program.Id;
                        await _context.LinkFaculties.AddAsync(faculty);
                    }
                    await _context.SaveChangesAsync();
                }


                if (institute != null && !string.IsNullOrEmpty(institute.Name))
                {
                    var normalizedName = institute.Name.Trim().ToLowerInvariant();

                    var existingInstitute = await _context.Institutes
                        .FirstOrDefaultAsync(i => i.Name.ToLower().Trim() == normalizedName);

                    if (existingInstitute != null)
                    {
                        program.InstituteId = existingInstitute.Id;
                    }
                    else
                    {
                        var newInstitute = new Institute
                        {
                            Name = institute.Name.Trim() 
                        };
                        await _context.Institutes.AddAsync(newInstitute);
                        await _context.SaveChangesAsync();
                        program.InstituteId = newInstitute.Id;
                    }
                }


                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(ProgramModel program)
        {
            _context.Programs.Update(program);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var program = await _context.Programs.FindAsync(id);
            if (program != null)
            {
                _context.Programs.Remove(program);
                await _context.SaveChangesAsync();
            }
        }


    }
}