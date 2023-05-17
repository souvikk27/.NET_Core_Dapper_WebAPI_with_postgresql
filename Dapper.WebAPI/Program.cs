using Dapper.WebAPI.InfraStructure;
using Dapper.WebAPI.Interfaces;
using Dapper.WebAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDapperStores();
builder.Services.AddInfraStructure();
builder.Services.AddHelpers();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "",
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
});
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("./v1/swagger.json", "My API V1"); //originally "./swagger/v1/swagger.json"
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
