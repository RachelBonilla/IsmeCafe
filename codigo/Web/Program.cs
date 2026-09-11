using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Reglas;
using Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// HttpClients para los servicios externos.
builder.Services.AddHttpClient("ServicioProductos");
builder.Services.AddHttpClient("ServicioServicios");
builder.Services.AddHttpClient("ServicioUsuarios");
builder.Services.AddHttpClient("ServicioCarrito");
builder.Services.AddHttpClient("ServicioPedido");

// Configuración + servicios externos + reglas de negocio.
builder.Services.AddScoped<IConfiguracion, Configuracion>();
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<IProductoReglas, ProductoReglas>();

builder.Services.AddScoped<IServicioReglas, ServicioReglas>();
builder.Services.AddScoped<IServicioServicios, ServicioServicios>();

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

builder.Services.AddScoped<IRolReglas,RolReglas>();
builder.Services.AddScoped<IRolServicio, RolServicio>();

// M5 - Pedidos (HU-21, HU-22, HU-23)
builder.Services.AddScoped<ICarritoServicio, CarritoServicio>();
builder.Services.AddScoped<ICarritoReglas, CarritoReglas>();
builder.Services.AddScoped<IPedidoServicio, PedidoServicio>();
builder.Services.AddScoped<IPedidoReglas, PedidoReglas>();

// Configuración HttpClient para el servicio de autenticación (Seguridad).
builder.Services.AddHttpClient("ServicioAuth");

// Configuración de Sesiones (Seguridad)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Index"; 
        options.AccessDeniedPath = "/Index"; // Pendiente de programar pagina de acceso no autorizado.
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

var app = builder.Build();

// Utilizar autenticación y autorización (Seguridad).
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
