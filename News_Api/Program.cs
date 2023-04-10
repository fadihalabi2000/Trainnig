using Microsoft.EntityFrameworkCore;
using NewsApiData;

using NewsApiServies.CRUD;
using NewsApiServies.CRUD.Interfaces;
using Services.CRUD;
using Services.CRUD.Interfaces;
using Services.Transactions;
using Services.Transactions.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson
                                 (x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWorkRepo,UnitOfWorkRepo>();
builder.Services.AddScoped<IUnitOfWorkService,UnitOfWorkService>();
builder.Services.AddDbContext<NewsApiDbContext>(option =>
option.UseSqlServer(builder.Configuration["ConnectionStrings:NewsApiConnectionString"]));



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
