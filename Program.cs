using Microsoft.EntityFrameworkCore;
using SistemaAPIRest.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("EventsCs");
builder.Services.AddDbContext<EventDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Usando armazenamento em memória:
//builder.Services.AddDbContext<EventDbContext>(e => e.UseInMemoryDatabase("EventsDb"));

builder.Services.AddControllers();
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
