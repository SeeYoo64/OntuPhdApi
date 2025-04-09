using System;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Services;
using OntuPhdApi.Services.Programs;

namespace OntuPhdApi.Controllers
{
    public enum DegreeType
    {
        phd,
        doc
    }
    [ApiController]
    [Route("api/[controller]")]

    public class ProgramsController : ControllerBase
    {
        private readonly IProgramService _programService;

        public ProgramsController(IProgramService programService)
        {
            _programService = programService;
        }
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Files/Uploads/Programs");

        [HttpGet]
        public IActionResult GetPrograms()
        {
            try
            {
                    var programs = _programService.GetPrograms();
                    // Сортировка по Degrees - phd -> everything else
                    programs = programs
                        .OrderBy(r => r.Degree switch {
                            "phd" => 1,
                            _ => 2
                        })
                        .ThenBy(r => r.Id)
                        .ToList();

                    return Ok(programs);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("degrees")]
        public IActionResult GetProgramsDegrees([FromQuery] DegreeType? degree)
        {
            try
            {
                var programsDegrees = _programService.GetProgramsDegrees(degree);
                return Ok(programsDegrees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgram(int id)
        {
            var program = await _programService.GetProgram(id);
            if (program == null)
                return NotFound();
            return Ok(program);
        }

        [HttpPost]
        public async Task<IActionResult> AddProgram([FromForm] ProgramRequestDto request)
        {
            // Проверка обязательных полей: Name и Degree
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Degree))
            {
                return BadRequest("Invalid program data. Name and Degree are required.");
            }

            try
            {
                // Создание модели программы
                var program = new ProgramModel
                {
                    Degree = request.Degree,
                    Name = request.Name,
                    NameCode = request.NameCode,
                    FieldOfStudy = request.FieldOfStudy != null
                        ? new FieldOfStudy { Code = request.FieldOfStudy.Code, Name = request.FieldOfStudy.Name }
                        : null,
                    Speciality = request.Speciality != null
                        ? new Speciality { Code = request.Speciality.Code, Name = request.Speciality.Name, FieldCode = request.Speciality.FieldCode }
                        : null,
                    Form = request.Form,
                    Objects = request.Objects,
                    Directions = request.Directions,
                    Descriptions = request.Descriptions,
                    Purpose = request.Purpose,
                    Years = request.Years,
                    Credits = request.Credits,
                    ProgramCharacteristics = request.ProgramCharacteristics != null
                        ? new ProgramCharacteristics
                        {
                            Area = request.ProgramCharacteristics.Area != null
                                ? new Area
                                {
                                    Object = request.ProgramCharacteristics.Area.Object,
                                    Aim = request.ProgramCharacteristics.Area.Aim,
                                    Theory = request.ProgramCharacteristics.Area.Theory,
                                    Methods = request.ProgramCharacteristics.Area.Methods,
                                    Instruments = request.ProgramCharacteristics.Area.Instruments
                                }
                                : null,
                            Focus = request.ProgramCharacteristics.Focus,
                            Features = request.ProgramCharacteristics.Features
                        }
                        : null,
                    ProgramCompetence = request.ProgramCompetence != null
                        ? new ProgramCompetence
                        {
                            OverallCompetence = request.ProgramCompetence.OverallCompetence,
                            SpecialCompetence = request.ProgramCompetence.SpecialCompetence,
                            IntegralCompetence = request.ProgramCompetence.IntegralCompetence
                        }
                        : null,
                    Results = request.Results,
                    LinkFaculty = request.LinkFaculty,
                    Components = request.Components,
                    Jobs = request.Jobs,
                    Accredited = request.Accredited,
                    ProgramDocumentId = 0 // Временно, будет заполнено сервисом
                };

                // Обработка файла, если он есть
                if (request.File != null && request.File.Length > 0)
                {
                    if (!Directory.Exists(_uploadFolder))
                        Directory.CreateDirectory(_uploadFolder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);
                    var filePath = Path.Combine(_uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.File.CopyToAsync(stream);
                    }

                    // Добавление программы с файлом
                    await _programService.AddProgram(program, filePath, request.File.ContentType, request.File.Length);
                }
                else
                {
                    // Добавление программы без файла
                    await _programService.AddProgram(program, null, null, 0);
                }

                return CreatedAtAction(nameof(GetProgram), new { id = program.Id }, program);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // PUT: Обновление программы с опциональной заменой файла
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(int id, [FromForm] ProgramRequestDto request)
        {
            //if (string.IsNullOrEmpty(request.Name))
            //{
            //    return BadRequest("Invalid program data. Name is required.");
            //}

            try
            {
                var existingProgram = await _programService.GetProgram(id);
                if (existingProgram == null)
                    return NotFound("Program not found.");

                existingProgram.Degree = request.Degree; 
                existingProgram.Name = request.Name;    
                existingProgram.NameCode = request.NameCode;
                existingProgram.FieldOfStudy = request.FieldOfStudy;
                existingProgram.Speciality = request.Speciality;
                existingProgram.Form = request.Form;
                existingProgram.Objects = request.Objects;
                existingProgram.Directions = request.Directions;
                existingProgram.Purpose = request.Purpose;
                existingProgram.Descriptions = request.Descriptions;
                existingProgram.Years = request.Years;
                existingProgram.Credits = request.Credits;
                existingProgram.Results = request.Results;
                existingProgram.LinkFaculty = request.LinkFaculty;
                existingProgram.Components = request.Components;
                existingProgram.Jobs = request.Jobs;
                existingProgram.Accredited = request.Accredited;

                if (request.ProgramCharacteristics != null)
                {
                    existingProgram.ProgramCharacteristics = new ProgramCharacteristics
                    {
                        Area = request.ProgramCharacteristics.Area != null
                            ? new Area
                            {
                                Object = request.ProgramCharacteristics.Area.Object,
                                Aim = request.ProgramCharacteristics.Area.Aim,
                                Theory = request.ProgramCharacteristics.Area.Theory,
                                Methods = request.ProgramCharacteristics.Area.Methods,
                                Instruments = request.ProgramCharacteristics.Area.Instruments
                            }
                            : null, 
                        Focus = request.ProgramCharacteristics.Focus,
                        Features = request.ProgramCharacteristics.Features
                    };
                }
                else
                {
                    existingProgram.ProgramCharacteristics = null; 
                }

                
                if (request.ProgramCompetence != null)
                {
                    existingProgram.ProgramCompetence = new ProgramCompetence
                    {
                        OverallCompetence = request.ProgramCompetence.OverallCompetence,
                        SpecialCompetence = request.ProgramCompetence.SpecialCompetence,
                        IntegralCompetence = request.ProgramCompetence.IntegralCompetence
                    };
                }
                else
                {
                    existingProgram.ProgramCompetence = null; 
                }



                if (request.File != null && request.File.Length > 0)
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Files/Uploads/Programs");
                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    var originalFileName = Path.GetFileNameWithoutExtension(request.File.FileName);
                    var extension = Path.GetExtension(request.File.FileName);
                    var uniqueSuffix = DateTime.Now.ToString("yyyyMMddHHmmss");
                    var fileName = $"{originalFileName}_{uniqueSuffix}{extension}";
                    var filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.File.CopyToAsync(stream);
                    }

                    await _programService.UpdateProgramWithDocument(existingProgram, filePath, request.File.FileName, request.File.ContentType, request.File.Length);
                }
                else
                {
                    existingProgram.ProgramDocumentId = null;
                    await _programService.UpdateProgram(existingProgram);
                }


                return Ok(existingProgram);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }









        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            try
            {
                await _programService.DeleteProgram(id);
                return NoContent(); // 204 - Успешное удаление
            }
            catch (Exception ex)
            {
                if (ex.Message == "Program not found.")
                    return NotFound("Program not found.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}