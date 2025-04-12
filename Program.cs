﻿using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using OntuPhdApi.Data;
using OntuPhdApi.Repositories.Defense;
using OntuPhdApi.Repositories.Employee;
using OntuPhdApi.Repositories.Program;
using OntuPhdApi.Services;
using OntuPhdApi.Services.ApplyDocuments;
using OntuPhdApi.Services.Authorization;
using OntuPhdApi.Services.Defense;
using OntuPhdApi.Services.Documents;
using OntuPhdApi.Services.Employees;
using OntuPhdApi.Services.Files;
using OntuPhdApi.Services.News;
using OntuPhdApi.Services.Programs;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ЭТУ СТРОЧКУ УДАЛИ!!!!! ЕСЛИ НЕ РАБОТАЕТ!!!!!!!!!!!!!!!!!!!!!!!! И БУДЕТ ОБРАТНО ЛОКАЛХОСТ
        // ИЛИ ДОБАВЬ ЧТО_ТО ЕЩЁ
        builder.WebHost.UseUrls("http://0.0.0.0:5124", "https://0.0.0.0:5125");

        // Adding Jwt services
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
            };
        });

        builder.Services.AddAuthorization();


        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddScoped<DatabaseService>();

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            });

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions => NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson()
            );
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Enter JWT Accesss Token",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {jwtSecurityScheme, Array.Empty<string>() }
            });



        });

        builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
        builder.Services.AddScoped<IProgramService, ProgramService>();
        builder.Services.AddScoped<IProgramFileService, ProgramFileService>();

        builder.Services.AddScoped<IDocumentService, DocumentService>();

        builder.Services.AddScoped<IApplyDocumentsService, ApplyDocumentsService>();

        builder.Services.AddScoped<INewsService, NewsService>();

        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IEmployeesService, EmployeesService>();

        builder.Services.AddScoped<IDefenseService, DefenseService>();
        builder.Services.AddScoped<IDefenseRepository, DefenseRepository>();

        builder.Services.AddScoped<ISpecialityNFieldsService, SpecialityNFieldsService>();

        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "OntuPhd API",
                Version = "v1",
                Description = """<div style="text-align: center;"><br>  <h2>Hello, FRONTENDERRR! 🌸</h2><br>   """ +
                "<p>Сидит мать, дочь и сын на кухне ждут отца </p><br>" +
                "<p> - С кем он щас поздоровается того он и выебет. </p><br>" +
                "<p>Входит отец на кухню : </p><br>" +
                "<p> - ВСЕМ ПРИВЕТ! ✨</p><br>" +
                "<p><small>have fun</small></p><br></div>"
            });
        });

        var app = builder.Build();
        app.UseCors("AllowAll");

        // Middleware
        app.UseRouting();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "OntuPhd API V1");
            c.RoutePrefix = string.Empty;
        });

        // app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}