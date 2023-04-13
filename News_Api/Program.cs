using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsApiData;
using NewsApiDomin.Models;
using NewsApiRepositories.UnitOfWorkRepository;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using NewsApiServies.Auth.Interfaces;
using NewsApiServies.Pagination;
using NewsApiServies.Pagination.Interface;
using Services.Auth;
using Services.MyLogger;
using Services.Transactions;
using Services.Transactions.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWorkRepo,UnitOfWorkRepo>();
builder.Services.AddScoped<IUnitOfWorkService,UnitOfWorkService>();
builder.Services.AddTransient<IUserAuthService,UserAuthService>();
builder.Services.AddTransient<IAuthorAuthService,AuthorAuthService>();
builder.Services.AddTransient<IMyLogger, MyLogger>();

builder.Services.AddDbContext<NewsApiDbContext>(option =>
option.UseSqlServer(builder.Configuration["ConnectionStrings:NewsApiConnectionString"]));

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
