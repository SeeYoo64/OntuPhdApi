﻿using Npgsql;
using OntuPhdApi.Models.News;
using OntuPhdApi.Models.Programs;
using System.Data;
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

        public List<FieldOfStudyDto> GetSpecialitiesNFields(string degree = null) 
        {
            var fieldOfStudyDtoList = new List<FieldOfStudyDto>();
            var fieldsDict = new Dictionary<string, FieldOfStudyDto>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"
                SELECT  
                f.code, f.name, f.degree  
                FROM field_of_study f ";
                if (!string.IsNullOrEmpty(degree))
                {
                    sql += " WHERE Degree = @degree ";
                }
                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    if (!string.IsNullOrEmpty(degree))
                    {
                        cmd.Parameters.AddWithValue("degree", degree);
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string fieldCode = reader.GetString(reader.GetOrdinal("code"));

                            // Если ещё не добавлен, добавляем FieldOfStudyDto
                            if (!fieldsDict.TryGetValue(fieldCode, out var fieldDto))
                            {
                                fieldDto = new FieldOfStudyDto
                                {
                                    Code = fieldCode,
                                    Name = reader.GetString(reader.GetOrdinal("name")),
                                    Degree = reader.GetString(2)
                                };
                                fieldsDict.Add(fieldCode, fieldDto);
                            }

                        }
                    }
                }
            }

            return fieldsDict.Values.ToList();
        }

        public List<ShortSpeciality> GetSpecialitiesByCode(string code)
        {
            var ShortSpecialityList = new List<ShortSpeciality>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"
                SELECT 
                s.code AS speciality_code,
                s.name AS speciality_name 
                FROM speciality s
                WHERE s.field_code = @code";

                using (var cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@code", code);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ShortSpecialityList.Add(new ShortSpeciality
                            {
                                Code = reader.GetString(0),
                                Name = reader.GetString(1)
                            });

                        }
                    }
                }
            }

            return ShortSpecialityList;
        }


    }
}
