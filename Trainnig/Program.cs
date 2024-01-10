using Microsoft.EntityFrameworkCore;
using TrainnigApI.Data;
using TrainnigApI.Model;
using TrainnigApI.service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDBContext>(option =>
option.UseSqlServer(builder.Configuration["ConnectionStrings:DbCoreConnectionString"]));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBaseService<Center>, BaseService<Center>>();
builder.Services.AddScoped<IBaseService<Account>, BaseService<Account>>();
builder.Services.AddScoped<IBaseService<Room>, BaseService<Room>>();
builder.Services.AddScoped<IBaseService<Reservation>, BaseService<Reservation>>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
