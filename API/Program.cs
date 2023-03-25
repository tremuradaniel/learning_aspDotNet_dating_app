using API.Data;
using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DataContext>(opt => 
{
  opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();

// AddTransient -- too short lived for a http request - created and disposed
// of as and when it's used

// AddSingleton -- create a service when the application first starts and is 
// never disposed until the application has closed down

// AddScoped -- when we create a controller, as in a request hit the endpoint
// so the framework instantiates a new instance of that controller,
// the controller looks at its depencencies or the framework does and determines
// that it must create these services and create new instances of these services
// when the controllers are created 
// WHEN the controller is disposed of at the end of the HTTP request, then
// any depencennt services are also disposed 

// AddScoped would have worked with TokenService directly, but using the 
// interface has an advantage when testing - easier to mock stuff
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.MapControllers();

app.Run();
