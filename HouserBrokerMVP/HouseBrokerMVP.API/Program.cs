using HouseBrokerMVP.API;
using HouseBrokerMVP.Business;
using HouseBrokerMVP.Business.Services;
using HouseBrokerMVP.Core.Context;
using HouseBrokerMVP.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = false;
});



//Swagger
builder.Services.AddSwaggerGen(s =>
{
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "LOGIN",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                    });
});
builder.Services.AddControllers();
builder.Services.ConfigServices(configuration);
builder.Services.AddServicesFromInfrastructure(configuration);
builder.Services.AddServicesFromBusiness(configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseStaticFiles(
               new StaticFileOptions
               {
                   FileProvider = new PhysicalFileProvider(
                       Path.Combine(app.Environment.ContentRootPath, "PropertyImage")),
                   RequestPath = "/propertyImage"
               }
           
           );
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllAllowPolicy");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
//Defaults initializations in migrations

using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();
        dbContext.Database.Migrate();
        await migrationService.Run();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        //something maybe wrong, add logger here
    }
}

app.Run();
