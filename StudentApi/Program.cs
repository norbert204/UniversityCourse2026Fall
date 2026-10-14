using Serilog;
using StudentApi;
using StudentApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog(options => options
    .MinimumLevel.Information()
    .WriteTo.File("logs/log.txt")
    .WriteTo.Console());

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IStudentRepository, StudentRepository>();

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

app.UseExceptionHandler("/error");

app.MapControllers();

app.Run();
