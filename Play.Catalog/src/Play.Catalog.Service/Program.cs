using Play.Catalog.Service.Entities;
using Play.Common.MassTransit;
using Play.Common.MongoDB;
using Play.Common.Settings;

var builder = WebApplication.CreateBuilder(args);
const string AllowedOriginSetting = "AllowedOrigin";
ServiceSettings serviceSettings;

// Add services to the container.

serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

builder.Services.AddMongo()
	.AddMongoRepository<Item>("items")
	.AddMassTransitWithRabbitMq();

builder.Services.AddControllers(options =>
{
	options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseCors(_builder =>
	{
		_builder.WithOrigins(builder.Configuration[AllowedOriginSetting])
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
