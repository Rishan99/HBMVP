using HouseBrokerMVP.API;
using HouseBrokerMVP.API.ExcepitonHandler;
using HouseBrokerMVP.Business;
using HouseBrokerMVP.Infrastructure;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = false;
});

builder.Services.AddControllers();
builder.Services.ConfigServices(configuration);

builder.Services.AddServicesFromInfrastructure(configuration);
builder.Services.AddServicesFromBusiness(configuration);


var app = builder.Build();

app.UseCors("AllAllowPolicy");
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
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandler>();
app.MapControllers();

app.Run();
