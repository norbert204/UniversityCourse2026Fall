using StudentApi;
using StudentApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Modified from `main`
// Hello from Test branch

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
}
else
{
    builder.Services.AddSingleton<IStudentRepository, OracleStudentRepository>();
}
builder.Services.AddTransient<RandomService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
