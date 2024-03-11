using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var constr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<RpstestContext>(options => options.UseNpgsql(constr));

builder.Services.AddScoped<IRepository<User>, GenericRepository<User>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
