using System.Text.Json.Serialization;
using OntuPhdApi.Services;
using OntuPhdApi.Services.ApplyDocuments;
using OntuPhdApi.Services.Defense;
using OntuPhdApi.Services.Documents;
using OntuPhdApi.Services.Employees;
using OntuPhdApi.Services.News;
using OntuPhdApi.Services.Programs;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();
