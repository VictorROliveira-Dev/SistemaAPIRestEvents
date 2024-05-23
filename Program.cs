using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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
// Configurando o swagger para implementar a documentação:
builder.Services.AddSwaggerGen(doc =>
{
    //Alterando propriedades na documentação:
    doc.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Events.API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Alef Ramos",
            Email = "aleframos62@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/alef-ramos/")
        }
    });

    var xmlFile = "SistemaAPIRest.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    doc.IncludeXmlComments(xmlPath);
});

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
