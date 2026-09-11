using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Seguridad;
using API.Seguridad;
using DA;
using DA.Repositorios;
using Flujo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//CORS (Direcciones http para correr localmente las paginas)
builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("PermitirWeb", politica =>
    {
        politica
            .WithOrigins(
            "https://localhost:7224",
            "http://localhost:7224",
            "https://localhost:7045",
            "http://localhost:7045"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductoFlujo, ProductoFlujo>();
builder.Services.AddScoped<IProductoDA, ProductoDA>();
builder.Services.AddScoped<IServicioFlujo, ServicioFlujo>();
builder.Services.AddScoped<IServiciosDA, ServicioDA>();
builder.Services.AddScoped<IRepositorioDapper, RepositorioDapper>();


builder.Services.AddScoped<IDescuentoDA, DescuentoDA>();
builder.Services.AddScoped<IDescuentoFlujo, DescuentoFlujo>();

builder.Services.AddScoped<IOfertaDAcs, OfertaDA>();
builder.Services.AddScoped<IOfertaFlujo, OfertaFlujo>();

builder.Services.AddScoped<ICampanaMarketingDA, CampanaMarketingDA>();
builder.Services.AddScoped<ICampanaMarketingFlujo, CampanaMarketingFlujo>();

builder.Services.AddScoped<IUsuarioDA, UsuarioDA>();
builder.Services.AddScoped<IUsuarioFlujo, UsuarioFlujo>();

builder.Services.AddScoped<ILoginDA, LoginDA>();
builder.Services.AddScoped<ILoginFlujo, LoginFlujo>();
builder.Services.AddScoped<IJwtAuthorization, JwtAuthorization>();

builder.Services.AddScoped<IRolDA, RolDA>();
builder.Services.AddScoped<IRolFlujo, RolFlujo>();

builder.Services.AddScoped<IConfiguracionLealtadDA, ConfiguracionLealtadDA>();

builder.Services.AddScoped<ICarritoDA, CarritoDA>();
builder.Services.AddScoped<ICarritoFlujo, CarritoFlujo>();
builder.Services.AddScoped<IPedidoDA, PedidoDA>();
builder.Services.AddScoped<IPedidoFlujo, PedidoFlujo>();

builder.Services.AddScoped<IReporteDA, ReporteDA>();
builder.Services.AddScoped<IReporteFlujo, ReporteFlujo>();

builder.Services.AddScoped<IReservaFlujo, ReservaFlujo>();
builder.Services.AddScoped<IReservaDA, ReservaDA>();

// Configuración JWT

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"];
        var jwtIssuer = builder.Configuration["Jwt:Issuer"];
        var jwtAudience = builder.Configuration["Jwt:Audience"];

        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new InvalidOperationException("No se ha configurado llave JWT");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Swagger enabled in all environments (incluido producción en Azure).
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Isme Café API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseCors("PermitirWeb");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Endpoint raíz: redirige a Swagger para que la URL base no devuelva 404.
app.MapGet("/", () => Results.Redirect("/swagger"));

// Endpoint simple de salud.
app.MapGet("/health", () => Results.Ok(new { status = "ok", service = "Isme Café API" }));

app.Run();
