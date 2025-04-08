using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using OntuPhdApi.Services;
using OntuPhdApi.Services.ApplyDocuments;
using OntuPhdApi.Services.Defense;
using OntuPhdApi.Services.Documents;
using OntuPhdApi.Services.Employees;
using OntuPhdApi.Services.News;
using OntuPhdApi.Services.Programs;

var builder = WebApplication.CreateBuilder(args);

// щрс ярпнвйс сдюкх!!!!! еякх ме пюанрюер!!!!!!!!!!!!!!!!!!!!!!!! х асдер напюрмн кнйюкуняр
// хкх днаюбэ врн_рн еы╗
builder.WebHost.UseUrls("http://0.0.0.0:5124", "https://0.0.0.0:5125");


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<DatabaseService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IProgramService, ProgramService>();
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

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
