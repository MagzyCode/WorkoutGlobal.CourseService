using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WorkoutGlobal.CourseService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers()
    .AddNewtonsoftJson();
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositories();
builder.Services.ConfigureAttributes();
builder.Services.ConfigureMassTransit(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Bus.Factory.CreateUsingRabbitMq(config =>
{
    config.Host(builder.Configuration["MassTransitSettings:Host"]);
}).Start();

app.Run();