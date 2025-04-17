using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using OntuPhdApi.Controllers;
using OntuPhdApi.Data;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Repositories.Program;
using System.Text.Json;

namespace OntuPhdApi.Services.Programs
{
    public class ProgramService : IProgramService
    {
        private readonly IProgramRepository _programRepository;
        private readonly ILogger<ProgramService> _logger;
        private readonly string _programsUploadsPath;

        public ProgramService(IProgramRepository programRepository, ILogger<ProgramService> logger, IWebHostEnvironment environment)
        {
            _programRepository = programRepository;
            _logger = logger;
            _programsUploadsPath = Path.Combine(environment.WebRootPath, "Files", "Uploads", "Programs");
        }

        public async Task<IEnumerable<ProgramModel>> GetAllProgramsAsync()
        {
            _logger.LogInformation("Fetching all programs");
            return await _programRepository.GetAllProgramsAsync();
        }

        public async Task<ProgramModel> GetProgramByIdAsync(int id)
        {
            _logger.LogInformation("Fetching program with ID: {ProgramId}", id);
            var program = await _programRepository.GetProgramByIdAsync(id);
            if (program == null)
            {
                _logger.LogWarning("Program with ID {ProgramId} not found", id);
                throw new KeyNotFoundException("Program not found");
            }
            return program;
        }

        public async Task<IEnumerable<ProgramsDegreeDto>> GetProgramsByDegreeAsync(string degreeType)
        {
            _logger.LogInformation("Fetching programs with DegreeType: {DegreeType}", degreeType);
            var programs = await _programRepository.GetProgramsByDegreeAsync(degreeType);
            return programs.Select(p => new ProgramsDegreeDto
            {
                Id = p.Id,
                Degree = p.Degree,
                Name = p.Name,
                FieldOfStudy = p.FieldOfStudy,
                Speciality = p.Speciality != null ? new ShortSpeciality
                {
                    Code = p.Speciality.Code,
                    Name = p.Speciality.Name
                } : null
            });
        }


        public async Task AddProgramAsync(ProgramCreateUpdateDto programDto)
        {
            if (string.IsNullOrEmpty(programDto.Degree) || string.IsNullOrEmpty(programDto.Name))
            {
                _logger.LogWarning("Invalid program data: Degree and Name are required");
                throw new ArgumentException("Degree and Name are required");
            }

            _logger.LogInformation("Adding new program with name: {ProgramName}", programDto.Name);
            try
            {
                var program = new ProgramModel
                {
                    Degree = programDto.Degree,
                    Name = programDto.Name,
                    NameCode = programDto.NameCode,
                    FieldOfStudy = programDto.FieldOfStudy,
                    Speciality = programDto.Speciality,
                    Form = programDto.Form,
                    Objects = programDto.Objects,
                    Directions = programDto.Directions,
                    Descriptions = programDto.Descriptions,
                    Purpose = programDto.Purpose,
                    Years = programDto.Years,
                    Credits = programDto.Credits,
                    ProgramCharacteristics = programDto.ProgramCharacteristics,
                    ProgramCompetence = programDto.ProgramCompetence,
                    Results = programDto.Results,
                    LinkFaculty = programDto.LinkFaculty,
                    Components = programDto.Components,
                    Jobs = programDto.Jobs,
                    Accredited = programDto.Accredited
                };

                await _programRepository.AddProgramAsync(program);

                // Создаём папку
                var programDir = Path.Combine(_programsUploadsPath, program.Id.ToString());
                Directory.CreateDirectory(programDir);

                // Сохраняем файлы
                if (programDto.Files != null && programDto.Files.Any())
                {
                    program.LinksFile = new List<ProgramFiles>();
                    foreach (var file in programDto.Files)
                    {
                        if (file.Length == 0)
                            continue;

                        // Проверка размера
                        if (file.Length > 20 * 1024 * 1024)
                        {
                            _logger.LogWarning("File too large for program ID: {ProgramId}, Size: {Size}", program.Id, file.Length);
                            throw new ArgumentException("File size exceeds 20 MB");
                        }

                        // Проверка типа
                        var allowedTypes = new[] { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
                        if (!allowedTypes.Contains(file.ContentType))
                        {
                            _logger.LogWarning("Invalid file type for program ID: {ProgramId}, Type: {Type}", program.Id, file.ContentType);
                            throw new ArgumentException("Only PDF, DOC, DOCX files are allowed");
                        }

                        var extension = Path.GetExtension(file.FileName).ToLower();
                        var fileName = $"file_{DateTime.UtcNow:yyyyMMddHHmm}_{Guid.NewGuid().ToString("N")[..8]}{extension}";
                        var filePath = Path.Combine(programDir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        program.LinksFile.Add(new ProgramFiles
                        {
                            Name = file.FileName,
                            Link = fileName // Только имя файла
                        });
                        _logger.LogInformation("Uploaded file for program ID: {ProgramId}, File: {File}", program.Id, fileName);
                    }
                }

                await _programRepository.UpdateProgramAsync(program);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add program with name: {ProgramName}", programDto.Name);
                throw;
            }
        }

        public async Task UpdateProgramAsync(int id, ProgramCreateUpdateDto programDto)
        {
            if (string.IsNullOrEmpty(programDto.Degree) || string.IsNullOrEmpty(programDto.Name))
            {
                _logger.LogWarning("Invalid program data: Degree and Name are required");
                throw new ArgumentException("Degree and Name are required");
            }

            _logger.LogInformation("Updating program with ID: {ProgramId}", id);
            try
            {
                var program = await _programRepository.GetProgramByIdAsync(id);
                if (program == null)
                {
                    _logger.LogWarning("Program with ID {ProgramId} not found", id);
                    throw new KeyNotFoundException("Program not found");
                }

                // Обновляем поля
                program.Degree = programDto.Degree;
                program.Name = programDto.Name;
                program.NameCode = programDto.NameCode;
                program.FieldOfStudy = programDto.FieldOfStudy;
                program.Speciality = programDto.Speciality;
                program.Form = programDto.Form;
                program.Objects = programDto.Objects;
                program.Directions = programDto.Directions;
                program.Descriptions = programDto.Descriptions;
                program.Purpose = programDto.Purpose;
                program.Years = programDto.Years;
                program.Credits = programDto.Credits;
                program.ProgramCharacteristics = programDto.ProgramCharacteristics;
                program.ProgramCompetence = programDto.ProgramCompetence;
                program.Results = programDto.Results;
                program.LinkFaculty = programDto.LinkFaculty;
                program.Components = programDto.Components;
                program.Jobs = programDto.Jobs;
                program.Accredited = programDto.Accredited;

                // Обновляем файлы
                var programDir = Path.Combine(_programsUploadsPath, id.ToString());
                Directory.CreateDirectory(programDir);

                if (programDto.Files != null && programDto.Files.Any())
                {
                    // Удаляем старые файлы
                    if (program.LinksFile != null && program.LinksFile.Any())
                    {
                        foreach (var oldFile in program.LinksFile)
                        {
                            var fullPath = Path.Combine(programDir, oldFile.Link);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                                _logger.LogInformation("Deleted old file for program ID: {ProgramId}, File: {File}", id, oldFile.Link);
                            }
                        }
                    }

                    // Сохраняем новые файлы
                    program.LinksFile = new List<ProgramFiles>();
                    foreach (var file in programDto.Files)
                    {
                        if (file.Length == 0)
                            continue;

                        // Проверка размера
                        if (file.Length > 5 * 1024 * 1024)
                        {
                            _logger.LogWarning("File too large for program ID: {ProgramId}, Size: {Size}", id, file.Length);
                            throw new ArgumentException("File size exceeds 5 MB");
                        }

                        // Проверка типа
                        var allowedTypes = new[] { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
                        if (!allowedTypes.Contains(file.ContentType))
                        {
                            _logger.LogWarning("Invalid file type for program ID: {ProgramId}, Type: {Type}", id, file.ContentType);
                            throw new ArgumentException("Only PDF, DOC, DOCX files are allowed");
                        }

                        var extension = Path.GetExtension(file.FileName).ToLower();
                        var fileName = $"file_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..8]}{extension}";
                        var filePath = Path.Combine(programDir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        program.LinksFile.Add(new ProgramFiles
                        {
                            Name = file.FileName,
                            Link = fileName // Только имя файла
                        });
                        _logger.LogInformation("Uploaded new file for program ID: {ProgramId}, File: {File}", id, fileName);
                    }
                }

                await _programRepository.UpdateProgramAsync(program);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update program with ID: {ProgramId}", id);
                throw;
            }
        }

        public async Task DeleteProgramAsync(int id)
        {
            _logger.LogInformation("Deleting program with ID: {ProgramId}", id);
            try
            {
                var program = await _programRepository.GetProgramByIdAsync(id);
                if (program == null)
                {
                    _logger.LogWarning("Program with ID {ProgramId} not found", id);
                    throw new KeyNotFoundException("Program not found");
                }

                // Удаляем файлы
                var programDir = Path.Combine(_programsUploadsPath, id.ToString());
                if (Directory.Exists(programDir))
                {
                    foreach (var file in Directory.GetFiles(programDir))
                    {
                        System.IO.File.Delete(file);
                        _logger.LogInformation("Deleted file for program ID: {ProgramId}, File: {File}", id, file);
                    }
                    Directory.Delete(programDir);
                    _logger.LogInformation("Deleted directory for program ID: {ProgramId}, Directory: {Dir}", id, programDir);
                }

                await _programRepository.DeleteProgramAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete program with ID: {ProgramId}", id);
                throw;
            }
        }



    }
}
