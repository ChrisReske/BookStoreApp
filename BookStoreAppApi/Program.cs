using System.Text;
using BookStoreAppApi.Configurations;
using BookStoreAppApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region SeriLog Configuration

builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

#endregion

#region CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        b => b
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin());
});

#endregion

#region Database connection

var connString = builder.Configuration.GetConnectionString("BookStoreAppDbConnection");
builder.Services
    .AddDbContext<BookStoreDbContext>(options => options
        .UseSqlServer(connString));

#endregion

#region AutoMapper

builder.Services.AddAutoMapper(typeof(MapperConfig));

#endregion

#region NETCore Identity

builder.Services
    .AddIdentityCore<ApiUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BookStoreDbContext>();

#endregion

#region Authentication (JWT)

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
