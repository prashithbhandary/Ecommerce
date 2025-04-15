using clothing_store.application.interfaces;
using clothing_store.application.mapper;
using clothing_store.application.services;
using clothing_store.infrastructure.context;
using clothing_store.infrastructure.Indentity;
using clothing_store.infrastructure.mapper;
using clothing_store.infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<dbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<PasswordHasher>(); 
 builder.Services.AddScoped<JwtTokenGenerator>();
builder.Services.AddScoped<IUserAccountMapper, UserAccountMapper>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<CategoryRepository>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load JWT settings
var jwtSettings = builder.Configuration.GetSection("Jwt");

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
      };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAngularClient", policy =>
  {
    policy.WithOrigins("https://localhost:4200", "http://localhost:4200") // Allow Angular on both HTTP & HTTPS
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials();
  });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors("AllowAngularClient");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
