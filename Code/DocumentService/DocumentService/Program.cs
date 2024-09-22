using Amazon;
using Amazon.Runtime;
using DocumentService.BackgroundFileProcessing;
using DocumentService.Domain.Persistence;
using DocumentService.Persistence;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMetadataRepository, MetadataRepository>();

var awsSection = builder.Configuration.GetSection("AWS");
var awsCredentials = new BasicAWSCredentials(
    awsSection.GetValue<string>("AccessKey"),
    awsSection.GetValue<string>("SecretKey"));

var region = RegionEndpoint.GetBySystemName(awsSection.GetValue<string>("Region"));

builder.Services.RegisterServices(awsCredentials, region);
builder.Services.AddHostedService<HostProgram>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
