using Microsoft.EntityFrameworkCore;
using WAD13400.Controllers;
using System.Text.Json.Serialization;
using System.Text.Json;
using WAD13400.DAL.Data;
using WAD13400.DAL.Repositories;
using WAD13400.DAL.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TaskTrackerDbContext>(options =>
    options.UseSqlServer(
            builder.Configuration.GetConnectionString("TaskTrackerConnectionString")
        )
    );

builder.Services.AddScoped<IRepository<TaskItem>, TaskItemRepository>();
builder.Services.AddScoped<IRepository<ProjectItem>, ProjectItemRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
