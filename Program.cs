using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using OntuPhdApi.Data;
using OntuPhdApi.Repositories.ApplyDocument;
using OntuPhdApi.Repositories.Defense;
using OntuPhdApi.Repositories.Document;
using OntuPhdApi.Repositories.Employee;
using OntuPhdApi.Repositories.Institutes;
using OntuPhdApi.Repositories.News;
using OntuPhdApi.Repositories.Program;
using OntuPhdApi.Services;
using OntuPhdApi.Services.ApplyDocuments;
using OntuPhdApi.Services.Authorization;
using OntuPhdApi.Services.Defense;
using OntuPhdApi.Services.Documents;
using OntuPhdApi.Services.Employees;
using OntuPhdApi.Services.Files;
using OntuPhdApi.Services.Institutes;
using OntuPhdApi.Services.News;
using OntuPhdApi.Services.Programs;
using OntuPhdApi.Utilities;
using OntuPhdApi.Utilities.Mappers;
using static OntuPhdApi.Services.Programs.ProgramService;

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

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies["accessToken"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("Auth failed: " + context.Exception.Message);
                    return Task.CompletedTask;
                }
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


        var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(dataSource));

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

        builder.Services.AddScoped<OntuPhdApi.Utilities.Mappers.IProgramMapper, OntuPhdApi.Utilities.Mappers.ProgramMapper>(); // Or OntuPhdApi.Utilities.Mappers

        builder.Services.AddScoped<IInstituteRepository, InstituteRepository>();
        builder.Services.AddScoped<IInstituteService, InstituteService>();

        builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
        builder.Services.AddScoped<IDocumentService, DocumentService>();

        builder.Services.AddScoped<IApplyDocumentRepository, ApplyDocumentRepository>();
        builder.Services.AddScoped<IApplyDocumentsService, ApplyDocumentsService>();

        builder.Services.AddScoped<INewsRepository, NewsRepository>();
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
                builder.WithOrigins("http://192.168.0.160:3000")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });



        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "OntuPhd API",
                Version = "v1",
                Description = """<div style="text-align: center;"><br>  <h2>Hello, FRONTENDERRR! 🌸</h2><br>   """ 
            });
        });

        var app = builder.Build();


        // Middleware
        app.UseRouting();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "OntuPhd API V1");
            c.RoutePrefix = string.Empty;
        });

        // app.UseHttpsRedirection();
        app.UseCors("AllowAll");

        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}