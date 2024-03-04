using Microsoft.EntityFrameworkCore;
using TrainnigApI.Data;
using TrainnigApI.Model;
using TrainnigApI.service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDBContext>(option =>
option.UseSqlServer(builder.Configuration["ConnectionStrings:DbCoreConnectionString"]));
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy( name:"AllowAngularOrigins",
//    builder =>
//    {
//        builder.AllowAnyOrigin()
//                            .AllowAnyHeader()
//                            .AllowAnyMethod();

//    });
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAngularOrigins",
    policy => { 
    {
        policy.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
        }
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBaseService<Center>, BaseService<Center>>();
builder.Services.AddScoped<IBaseService<Account>, BaseService<Account>>();
builder.Services.AddScoped<IBaseService<Room>, BaseService<Room>>();
builder.Services.AddScoped<IBaseService<Reservation>, BaseService<Reservation>>();
builder.Services.AddScoped<IBaseService<ReservationRoom>, BaseService<ReservationRoom>>();
builder.Services.AddScoped<IBaseService<ReservationService>, BaseService<ReservationService>>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularOrigins");
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
