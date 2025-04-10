using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OntuPhdApi.Data;
using OntuPhdApi.Repositories;
using OntuPhdApi.Services;
using OntuPhdApi.Services.ApplyDocuments;
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

        // ›“” —“–Œ◊ ” ”ƒ¿À»!!!!! ≈—À» Õ≈ –¿¡Œ“¿≈“!!!!!!!!!!!!!!!!!!!!!!!! » ¡”ƒ≈“ Œ¡–¿“ÕŒ ÀŒ ¿À’Œ—“
        // »À» ƒŒ¡¿¬‹ ◊“Œ_“Œ ≈Ÿ®
        builder.WebHost.UseUrls("http://0.0.0.0:5124", "https://0.0.0.0:5125");


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
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
        builder.Services.AddScoped<IProgramService, ProgramService>();
        builder.Services.AddScoped<IProgramFileService, ProgramFileService>();

        builder.Services.AddScoped<IDocumentService, DocumentService>();

        builder.Services.AddScoped<IApplyDocumentsService, ApplyDocumentsService>();

        builder.Services.AddScoped<INewsService, NewsService>();

        builder.Services.AddScoped<IEmployeesService, EmployeesService>();

        builder.Services.AddScoped<IDefenseService, DefenseService>();

        builder.Services.AddScoped<ISpecialityNFieldsService, SpecialityNFieldsService>();




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
                Description = "HELLO HI ^_^ HIIIIII HELLOO "
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

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        // app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}