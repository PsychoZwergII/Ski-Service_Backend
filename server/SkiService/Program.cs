using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using SkiService.Models; // Ensure this namespace matches where SkiServiceDbContext is defined

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
        builder =>
        {
            builder.WithOrigins("https://psychozwergii.github.io") // Erlaube deine Web-App
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
        c.RoutePrefix = string.Empty; // Startet Swagger unter der Root-URL
    });
}

var appBuilder = app;  // Hier jetzt ein anderer Name, um Konflikte zu vermeiden

appBuilder.MapPost("/api/Order", async (Order order, SkiServiceDbContext db) =>
{
    Console.WriteLine("POST request received");
    if (order == null)
    {
        return Results.BadRequest("Invalid Order data");
    }
    await db.Orders.AddAsync(order);
    await db.SaveChangesAsync();
    return Results.Ok(order);
});

app.MapPost("/api/Order", async (HttpContext context, SkiServiceDbContext db) =>
{
    try
    {
        var requestBody = await context.Request.ReadFromJsonAsync<Order>();
        
        if (requestBody != null)
        {
            // Extrahiere die serviceId aus dem POST-Request
            var serviceName = context.Request.Form["serviceId"].ToString();
            
            // Suche den Service in der Datenbank
            var service = await db.Service.FirstOrDefaultAsync(s => s.Name == serviceName);

            if (service != null)
            {
                requestBody.ServiceId = service.Id;  // Setze die ServiceId aus der gefundenen Service-Tabelle
            }
            else
            {
                return Results.BadRequest("Ungültiger Service ausgewählt.");
            }

            // Speichere die Order in der Datenbank
            db.Orders.Add(requestBody);
            await db.SaveChangesAsync();

            return Results.Ok(requestBody);  // Erfolgreicher Status
        }
        else
        {
            return Results.BadRequest("Ungültige Daten.");
        }
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);  // Fehlerbehandlung
    }
});


appBuilder.UseCors("AllowWebApp");
appBuilder.UseHttpsRedirection();
appBuilder.UseRouting();
appBuilder.UseAuthentication();
appBuilder.UseAuthorization();

appBuilder.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Registriert die Controller
});

// Authentication und Authorization Middleware
appBuilder.Run();
