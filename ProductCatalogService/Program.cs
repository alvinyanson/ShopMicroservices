using Microsoft.EntityFrameworkCore;
using ProductCatalogService.Data;
using ProductCatalogService.Data.Repository.Contracts;
using ProductCatalogService.Data.Repository;
using ProductCatalogService.SyncDataServices.Http;
using ProductCatalogService.EventProcessing;
using ProductCatalogService.AsyncDataServices;
using ProductCatalogService.Services.Contracts;
using ProductCatalogService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// database context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"));
});


// services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IHttpComms, HttpComms>();
builder.Services.AddTransient<IHttpContextHelper, HttpContextHelper>();
//builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
//builder.Services.AddHostedService<MesageBusSubscriber>();

var app = builder.Build();

PrepDb.PrepPopulation(app, builder.Environment.IsProduction());


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
