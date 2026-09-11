using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Reglas;
using Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;
using Web.Seguridad;
using Abstracciones.Constantes;

using QuestPDF.Infrastructure;


QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    // Acceso para Empleado y Administrador
    options.Conventions.AuthorizeFolder( "/Admin", "Personal");

    // Acceso unicamente para Administrador
    options.Conventions.AuthorizeFolder( "/Admin/Usuarios", "Administracion");
});

// Configuración de Seguridad JWT.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<JwtAuthorizationHandler>();

// HttpClients para los servicios externos.
builder.Services.AddHttpClient("ServicioProductos");
builder.Services.AddHttpClient("ServicioServicios");
builder.Services.AddHttpClient("ServicioUsuarios");
builder.Services.AddHttpClient("ServicioCarrito");
builder.Services.AddHttpClient("ServicioPedido").AddHttpMessageHandler<JwtAuthorizationHandler>();
builder.Services.AddHttpClient("ServicioReporte");
builder.Services.AddHttpClient("ReservaServicios");

// Configuración + servicios externos + reglas de negocio.
builder.Services.AddScoped<IConfiguracion, Configuracion>();
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<IProductoReglas, ProductoReglas>();

builder.Services.AddScoped<IServicioReglas, ServicioReglas>();
builder.Services.AddScoped<IServicioServicios, ServicioServicios>();

builder.Services.AddScoped<IReservaServicios, ReservaServicios>();
builder.Services.AddScoped<IReservaReglas, ReservaReglas>();

builder.Services.AddScoped<IDescuentoServicio, DescuentoServicio>();
builder.Services.AddScoped<IDescuentoReglas, DescuentoReglas>();

builder.Services.AddScoped<IOfertaServicio, OfertaServicio>();
builder.Services.AddScoped<IOfertaReglas, OfertaReglas>();

builder.Services.AddScoped<ICampanaMarketingServicio, CampanaMarketingServicio>();
builder.Services.AddScoped<ICampanaMarketingReglas, CampanaMarketingReglas>();

builder.Services.AddScoped<ILoginServicio, LoginServicio>();
builder.Services.AddScoped<ILoginReglas, LoginReglas>();

builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
builder.Services.AddScoped<IUsuarioReglas, UsuarioReglas>();

builder.Services.AddScoped<IRolReglas, RolReglas>();
builder.Services.AddScoped<IRolServicio, RolServicio>();

// M5 - Pedidos (HU-21, HU-22, HU-23)
builder.Services.AddScoped<ICarritoServicio, CarritoServicio>();
builder.Services.AddScoped<ICarritoReglas, CarritoReglas>();
builder.Services.AddScoped<IPedidoServicio, PedidoServicio>();
builder.Services.AddScoped<IPedidoReglas, PedidoReglas>();

builder.Services.AddScoped<IReporteServicio, ReporteServicio>();
builder.Services.AddScoped<IReporteReglas, ReporteReglas>();

// Configuración HttpClient para el servicio de autenticación (Seguridad).
builder.Services.AddHttpClient("ServicioAuth");

// Configuración de Sesiones (Seguridad)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Index";
        options.AccessDeniedPath = "/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = false;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Personal", policy =>
    {
        policy.RequireRole(Roles.Administrador,Roles.Empleado);
    });

    options.AddPolicy("Administracion", policy =>
    {
        policy.RequireRole(Roles.Administrador);
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Seguridad
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();