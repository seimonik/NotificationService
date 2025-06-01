using NotificationService.Helper;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var conffiguration = builder.Configuration;
var assembly = typeof(Program).Assembly;

// Add services to the container.

builder.Services.AddControllers();
services.AddHostedService<KafkaConsumerService>();

services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
