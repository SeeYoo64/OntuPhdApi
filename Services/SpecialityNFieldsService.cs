using Npgsql;
using OntuPhdApi.Models.News;
using OntuPhdApi.Models.Programs;
using System.Text.Json;

namespace OntuPhdApi.Services
{
    public class SpecialityNFieldsService : ISpecialityNFieldsService
    {
        private readonly string _connectionString;

        public SpecialityNFieldsService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<FieldOfStudyDto> GetSpecialitiesNFields() 
        {
            var fieldOfStudyDtoList = new List<FieldOfStudyDto>();
            var fieldsDict = new Dictionary<string, FieldOfStudyDto>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"
            SELECT 
                f.code AS field_code,
                f.name AS field_name,
                s.code AS speciality_code,
                s.name AS speciality_name
            FROM 
                field_of_study f
            LEFT JOIN 
                speciality s ON f.code = s.field_code
            ORDER BY 
                f.code, s.code;
        ";

                using (var cmd = new NpgsqlCommand(sql, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string fieldCode = reader.GetString(reader.GetOrdinal("field_code"));

                        // Если ещё не добавлен, добавляем FieldOfStudyDto
                        if (!fieldsDict.TryGetValue(fieldCode, out var fieldDto))
                        {
                            fieldDto = new FieldOfStudyDto
                            {
                                Code = fieldCode,
                                Name = reader.GetString(reader.GetOrdinal("field_name")),
                                Specialities = new List<Speciality>()
                            };
                            fieldsDict.Add(fieldCode, fieldDto);
                        }

                        // Если у этого поля есть специализация — добавляем
                        if (!reader.IsDBNull(reader.GetOrdinal("speciality_code")))
                        {
                            var specialityDto = new Speciality
                            {
                                Code = reader.GetString(reader.GetOrdinal("speciality_code")),
                                Name = reader.GetString(reader.GetOrdinal("speciality_name"))
                            };
                            fieldDto.Specialities.Add(specialityDto);
                        }
                    }
                }
            }

            return fieldsDict.Values.ToList();
        }




    }
}
