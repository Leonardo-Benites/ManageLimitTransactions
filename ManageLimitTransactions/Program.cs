using Application.Services;
using Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Amazon.DynamoDBv2;
using Infrastructure.Settings;
using Infrastructure;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Register services
builder.Services.Configure<DatabaseSettings>(config.GetSection(DatabaseSettings.KeyName));
builder.Services.AddSingleton<IAmazonDynamoDB>(_ =>
    new AmazonDynamoDBClient(new AmazonDynamoDBConfig
    {
        ServiceURL = "http://localhost:8000"
    })
);

builder.Services.AddScoped<IClientAccountService, ClientAccountService>();
builder.Services.AddScoped<IClientAccountRepository, ClientAccountRepository>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(MappingProfileService));

builder.Services.AddControllers();
builder.Services.AddAuthorization();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        options.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseStaticFiles();

app.UseMiddleware<UnauthorizedResponseMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();

