using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using SkiService.Models;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5231");

// JWT-Einstellungen aus der Konfiguration laden
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

// Add services to the container.
// Database Context Configuration
builder.Services.AddDbContext<SkiServiceDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 23))));

// Authentication Setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SkiService API",
        Version = "v1",
        Description = "API für den Ski-Service",
        Contact = new OpenApiContact
        {
            Name = "Leon Egli",
            Email = "leonegli6@gmail.com",
        }
    });
});

// CORS erlauben
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        corsBuilder =>
        {
            corsBuilder.AllowAnyOrigin() // Für lokale Tests Alle erlauben, später anpassen
                      .AllowAnyMethod()
                      .AllowAnyHeader();
        });
});

var app = builder.Build();  // Haupt-Instanz des WebApplication

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkiService API v1");
        c.RoutePrefix = "swagger"; // Swagger verfügbar unter /swagger
    });
}

app.UseCors("AllowWebApp");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Registriert die Controller
});

// Authentication und Authorization Middleware
app.Run();
