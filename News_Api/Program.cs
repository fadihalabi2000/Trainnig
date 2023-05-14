using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NewsApiData;
using NewsApiDomin.Models;
using NewsApiRepositories.UnitOfWorkRepository;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using NewsApiServies.Auth;
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

builder.Services.AddControllers(
    options =>
    {
        options.ReturnHttpNotAcceptable = true;
    }
    ).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("NewsApiAuthentication", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",
    });
    options.AddSecurityRequirement(new
    Microsoft.OpenApi.Models.OpenApiSecurityRequirement
{
      {
         new OpenApiSecurityScheme
         {
           Reference = new OpenApiReference
           {

             Type = ReferenceType.SecurityScheme,

              Id = "NewsApiAuthentication"
           }
         },
              new List<string>()
      }
  });
});



builder.Services.AddScoped<IUnitOfWorkRepo,UnitOfWorkRepo>();
builder.Services.AddScoped<IUnitOfWorkService,UnitOfWorkService>();
builder.Services.AddTransient<IUserAuthService,UserAuthService>();
builder.Services.AddTransient<IAuthorAuthService,AuthorAuthService>();
builder.Services.AddTransient<IAdminAuthService, AdminAuthService>();
builder.Services.AddTransient<IMyLogger, MyLogger>();

builder.Services.AddDbContext<NewsApiDbContext>(option =>
option.UseSqlServer(builder.Configuration["ConnectionStrings:NewsApiConnectionString"]));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigins",
    builder =>
    {
        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();

    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseCors("AllowAngularOrigins");
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
