//----------------------------------------------------------------------- 
// Copyright (c) 2019 All rights reserved.
// </copyright>
// <author>Jorge Pertuz Egea/Jpertuz</author>
// <date>Diciembre 2023</date>
//-----------------------------------------------------------------------


using Infraestructure;
using Infraestructure.Interfaces;
using Infraestructure.Models;
using Infraestructure.Repositories;
using Interfaces.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ContextDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("BDConnetion")));

JWTAuthentication JWTAuthenticationSection = builder.Configuration.GetSection("JWTAuthentication").Get<JWTAuthentication>();





// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test01", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
   

    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTAuthenticationSection.Secret)),

        ValidateIssuer = false,
        ValidIssuer = "",

        ValidateAudience = false,
        ValidAudience = "",

        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.FromMilliseconds(2),

        

    };

   
});

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IStatusServices, StatusServices>();
builder.Services.AddScoped<IProductsServices, ProductsServices>();



builder.Services.AddControllers().AddNewtonsoftJson(options => { options.UseMemberCasing(); });// Convierte Json Salida en CamellCAse -- Microsoft.AspNetCore.Mvc.NewtonsoftJson


builder.Services.AddCors(setupAction =>
{
    setupAction.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins(JWTAuthenticationSection.HostOriginPermited)
       .AllowAnyMethod()
       .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
