using BookStoreAppApi.Configurations;
using BookStoreAppApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

app.MapControllers();

app.Run();
