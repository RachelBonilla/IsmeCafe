using Abstracciones.Constantes;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;
using QuestPDF.Infrastructure;
using Reglas;
using Servicios;
using Web.Seguridad;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// RAZOR PAGES
// --------------------------------------------------
builder.Services.AddRazorPages(options =>
{
    // Acceso para Empleado y Administrador
    options.Conventions.AuthorizeFolder("/Admin", "Personal");

    // Acceso únicamente para Administrador
    options.Conventions.AuthorizeFolder("/Admin/Usuarios", "Administracion");
});

// --------------------------------------------------
// SEGURIDAD JWT PARA LLAMADAS A LA API
// --------------------------------------------------
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<JwtAuthorizationHandler>();

// --------------------------------------------------
// HTTP CLIENTS
// --------------------------------------------------

// Productos
builder.Services.AddHttpClient("ServicioProductos");

// Servicios
builder.Services.AddHttpClient("ServicioServicios");

// Usuarios - envía JWT
builder.Services
    .AddHttpClient("ServicioUsuarios")
    .AddHttpMessageHandler<JwtAuthorizationHandler>();

// Carrito
builder.Services.AddHttpClient("ServicioCarrito");

// Pedidos - envía JWT
builder.Services
    .AddHttpClient("ServicioPedido")
    .AddHttpMessageHandler<JwtAuthorizationHandler>();

// Reportes - envía JWT
builder.Services
    .AddHttpClient("ServicioReporte")
    .AddHttpMessageHandler<JwtAuthorizationHandler>();

// Reservas
builder.Services.AddHttpClient("ReservaServicios");

// Autenticación / Login
builder.Services.AddHttpClient("ServicioAuth");

// --------------------------------------------------
// CONFIGURACIÓN
// --------------------------------------------------
builder.Services.AddScoped<IConfiguracion, Configuracion>();

// --------------------------------------------------
// PRODUCTOS
// --------------------------------------------------
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<IProductoReglas, ProductoReglas>();

// --------------------------------------------------
// SERVICIOS
// --------------------------------------------------
builder.Services.AddScoped<IServicioServicios, ServicioServicios>();
builder.Services.AddScoped<IServicioReglas, ServicioReglas>();

// --------------------------------------------------
// RESERVAS
// --------------------------------------------------
builder.Services.AddScoped<IReservaServicios, ReservaServicios>();
builder.Services.AddScoped<IReservaReglas, ReservaReglas>();

// --------------------------------------------------
// DESCUENTOS
// --------------------------------------------------
builder.Services.AddScoped<IDescuentoServicio, DescuentoServicio>();
builder.Services.AddScoped<IDescuentoReglas, DescuentoReglas>();

// --------------------------------------------------
// OFERTAS
// --------------------------------------------------
builder.Services.AddScoped<IOfertaServicio, OfertaServicio>();
builder.Services.AddScoped<IOfertaReglas, OfertaReglas>();

// --------------------------------------------------
// CAMPAÑAS DE MARKETING
// --------------------------------------------------
builder.Services.AddScoped<ICampanaMarketingServicio, CampanaMarketingServicio>();
builder.Services.AddScoped<ICampanaMarketingReglas, CampanaMarketingReglas>();

// --------------------------------------------------
// LOGIN
// --------------------------------------------------
builder.Services.AddScoped<ILoginServicio, LoginServicio>();
builder.Services.AddScoped<ILoginReglas, LoginReglas>();

// --------------------------------------------------
// USUARIOS
// --------------------------------------------------
builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
builder.Services.AddScoped<IUsuarioReglas, UsuarioReglas>();

// --------------------------------------------------
// ROLES
// --------------------------------------------------
builder.Services.AddScoped<IRolServicio, RolServicio>();
builder.Services.AddScoped<IRolReglas, RolReglas>();

// --------------------------------------------------
// CARRITO
// --------------------------------------------------
builder.Services.AddScoped<ICarritoServicio, CarritoServicio>();
builder.Services.AddScoped<ICarritoReglas, CarritoReglas>();

// --------------------------------------------------
// PEDIDOS
// --------------------------------------------------
builder.Services.AddScoped<IPedidoServicio, PedidoServicio>();
builder.Services.AddScoped<IPedidoReglas, PedidoReglas>();

// --------------------------------------------------
// REPORTES
// --------------------------------------------------
builder.Services.AddScoped<IReporteServicio, ReporteServicio>();
builder.Services.AddScoped<IReporteReglas, ReporteReglas>();

// --------------------------------------------------
// AUTENTICACIÓN POR COOKIE EN LA WEB
// --------------------------------------------------
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Index";
        options.AccessDeniedPath = "/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = false;
    });

// --------------------------------------------------
// AUTORIZACIÓN POR ROLES
// --------------------------------------------------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Personal", policy =>
    {
        policy.RequireRole(
            Roles.Administrador,
            Roles.Empleado
        );
    });

    options.AddPolicy("Administracion", policy =>
    {
        policy.RequireRole(
            Roles.Administrador
        );
    });
});

var app = builder.Build();

// --------------------------------------------------
// CONFIGURACIÓN DEL PIPELINE
// --------------------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// --------------------------------------------------
// SEGURIDAD
// IMPORTANTE: Authentication va antes de Authorization
// --------------------------------------------------
app.UseAuthentication();
app.UseAuthorization();

// --------------------------------------------------
// RAZOR PAGES
// --------------------------------------------------
app.MapRazorPages();

app.Run();