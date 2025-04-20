using Npgsql;
using OntuPhdApi.Models.News;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Programs.Components;
using OntuPhdApi.Repositories.SpecAndField;
using System.Data;
using System.Text.Json;

namespace OntuPhdApi.Services.SpecAndFields
{
    public class SpecialityNFieldsService : ISpecialityNFieldsService
    {
        private readonly ISpecAndFieldRepository _repository;

        public SpecialityNFieldsService(ISpecAndFieldRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<FieldOfStudyResponseDto>> GetFieldsWithSpecialitiesAsync(string? degree)
        {
            var fields = await _repository.GetFieldsWithSpecialitiesAsync(degree);
            return fields.Select(f => new FieldOfStudyResponseDto
            {
                Code = f.Code,
                Name = f.Name,
                Degree = f.Degree
            }).ToList();
        }

        public async Task<List<SpecialityDto>> GetSpecialitiesByCodeAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException("Code cannot be null or empty.", nameof(code));

            var specialities = await _repository.GetSpecialitiesByCodeAsync(code);
            return specialities.Select(s => new SpecialityDto
            {
                Code = s.Code,
                Name = s.Name
            }).ToList();
        }

    }
}
