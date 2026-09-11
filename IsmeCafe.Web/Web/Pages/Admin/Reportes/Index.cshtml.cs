using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Constantes;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Web.Pages.Admin.Reportes
{
    public class IndexModel : PageModel
    {
        private readonly IReporteReglas _reporteReglas;
        private readonly IUsuarioReglas _usuarioReglas;
        private readonly ILogger<IndexModel> _logger;

        private static readonly CultureInfo Cultura = new("es-CR");

        public IndexModel(IReporteReglas reporteReglas, IUsuarioReglas usuarioReglas, ILogger<IndexModel> logger)
        {
            _reporteReglas = reporteReglas;
            _usuarioReglas = usuarioReglas;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public string Reporte { get; set; } = "inventario";

        [BindProperty(SupportsGet = true)]
        public Guid? IdCategoria { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? Activo { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaInicio { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? FechaFin { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid? IdEmpleado { get; set; }

        public IReadOnlyList<ReporteInventarioLinea> Inventario { get; private set; } = new List<ReporteInventarioLinea>();
        public IReadOnlyList<ReporteCatalogoLinea> Catalogo { get; private set; } = new List<ReporteCatalogoLinea>();
        public IReadOnlyList<ReporteVentasLinea> Ventas { get; private set; } = new List<ReporteVentasLinea>();

        public List<SelectListItem> Categorias { get; private set; } = new();
        public List<SelectListItem> Empleados { get; private set; } = new();

        public string? Mensaje { get; private set; }
        public string? Error { get; private set; }

        public bool VentasGeneradas { get; private set; }

        public int TotalProductos => Inventario.Count;
        public int TotalStockBajo => Inventario.Count(l => l.StockBajo);
        public int TotalAgotados => Inventario.Count(l => l.StockActual == 0);
        public int TotalCatalogo => Catalogo.Count;
        public int TotalCatalogoActivos => Catalogo.Count(l => l.Activo);
        public int TotalOrdenes => Ventas.Sum(v => v.CantidadOrdenes);
        public int TotalUnidades => Ventas.Sum(v => v.UnidadesVendidas);
        public decimal MontoVendido => Ventas.Sum(v => v.MontoTotal);

        public string Colones(decimal valor) => valor.ToString("C0", Cultura);

        public string Crudo(decimal valor) => valor.ToString("0.####", CultureInfo.InvariantCulture);

        public async Task OnGetAsync()
        {
            Reporte = NormalizarReporte(Reporte);
            await CargarFiltrosAsync();

            switch (Reporte)
            {
                case "catalogo":
                    await CargarCatalogoAsync();
                    break;

                case "ventas":
                    await CargarVentasAsync();
                    break;

                default:
                    await CargarInventarioAsync();
                    break;
            }
        }

        public async Task<IActionResult> OnGetExportarAsync()
        {
            Reporte = NormalizarReporte(Reporte);

            try
            {
                switch (Reporte)
                {
                    case "catalogo":
                        await CargarCatalogoAsync();
                        if (Catalogo.Count == 0) return await RegresarSinDatosAsync();
                        return ArchivoCsv(
                            ConstruirCsv(
                                new[] { "Tipo", "Nombre", "Categoria", "Precio", "Estado", "Existencias", "FechaCreacion" },
                                Catalogo.Select(l => new[]
                                {
                                    l.Tipo,
                                    l.Nombre,
                                    l.Categoria,
                                    l.Precio.ToString("0.00", CultureInfo.InvariantCulture),
                                    l.Activo ? "Activo" : "Inactivo",
                                    l.Existencias.ToString(CultureInfo.InvariantCulture),
                                    l.FechaCreacion?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty
                                })),
                            "reporte-catalogo");

                    case "ventas":
                        await CargarVentasAsync();
                        if (!VentasGeneradas || Ventas.Count == 0) return await RegresarSinDatosAsync();
                        return ArchivoCsv(
                            ConstruirCsv(
                                new[] { "Empleado", "Correo", "Ordenes", "MontoTotal", "TicketPromedio", "UnidadesVendidas", "PrimeraVenta", "UltimaVenta" },
                                Ventas.Select(v => new[]
                                {
                                    v.Empleado,
                                    v.Correo,
                                    v.CantidadOrdenes.ToString(CultureInfo.InvariantCulture),
                                    v.MontoTotal.ToString("0.00", CultureInfo.InvariantCulture),
                                    v.TicketPromedio.ToString("0.00", CultureInfo.InvariantCulture),
                                    v.UnidadesVendidas.ToString(CultureInfo.InvariantCulture),
                                    v.PrimeraVenta?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty,
                                    v.UltimaVenta?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty
                                })),
                            "reporte-ventas-empleado");

                    default:
                        await CargarInventarioAsync();
                        if (Inventario.Count == 0) return await RegresarSinDatosAsync();
                        return ArchivoCsv(
                            ConstruirCsv(
                                new[] { "Producto", "Categoria", "StockActual", "StockMinimo", "EstadoExistencias", "Precio", "Estado" },
                                Inventario.Select(l => new[]
                                {
                                    l.Nombre,
                                    l.Categoria,
                                    l.StockActual.ToString(CultureInfo.InvariantCulture),
                                    l.StockMinimo.ToString(CultureInfo.InvariantCulture),
                                    l.EstadoExistencias,
                                    l.Precio.ToString("0.00", CultureInfo.InvariantCulture),
                                    l.Activo ? "Activo" : "Inactivo"
                                })),
                            "reporte-inventario");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar el reporte {Reporte}", Reporte);
                Error = "No fue posible exportar el reporte.";
                await CargarFiltrosAsync();
                return Page();
            }
        }

        private async Task CargarInventarioAsync()
        {
            try
            {
                var lineas = await _reporteReglas.Inventario(IdCategoria);
                Inventario = lineas.ToList();

                if (Inventario.Count == 0)
                    Mensaje = IdCategoria.HasValue
                        ? "No hay productos registrados en la categoría seleccionada."
                        : "No hay productos registrados en el sistema.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de inventario");
                Error = "No fue posible generar el reporte de inventario. Verifica la conexión con el API.";
            }
        }

        private async Task CargarCatalogoAsync()
        {
            try
            {
                var lineas = await _reporteReglas.Catalogo(Activo);
                Catalogo = lineas.ToList();

                if (Catalogo.Count == 0)
                    Mensaje = Activo.HasValue
                        ? $"No hay elementos {(Activo.Value ? "activos" : "inactivos")} en el catálogo."
                        : "No hay productos ni servicios registrados en el catálogo.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el reporte de catálogo");
                Error = "No fue posible generar el reporte de catálogo. Verifica la conexión con el API.";
            }
        }

        private async Task CargarVentasAsync()
        {
            if (!FechaInicio.HasValue || !FechaFin.HasValue)
            {
                var hoy = DateTime.Today;
                FechaInicio ??= new DateTime(hoy.Year, hoy.Month, 1);
                FechaFin ??= hoy;
            }

            if (FechaInicio!.Value.Date > FechaFin!.Value.Date)
            {
                Error = "El rango de fechas no es válido: la fecha de inicio es posterior a la fecha de fin.";
                return;
            }

            try
            {
                var lineas = await _reporteReglas.VentasPorEmpleado(new ReporteVentasFiltro
                {
                    FechaInicio = FechaInicio.Value,
                    FechaFin = FechaFin.Value,
                    IdEmpleado = IdEmpleado
                });

                Ventas = lineas.ToList();
                VentasGeneradas = true;

                if (Ventas.Count == 0)
                    Mensaje = "No existen ventas registradas en el periodo seleccionado.";
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error al generar el reporte de ventas por empleado");
                Error = ex.Message;
            }
        }

        private async Task CargarFiltrosAsync()
        {
            try
            {
                var categorias = await _reporteReglas.Categorias();
                Categorias = categorias
                    .Select(c => new SelectListItem
                    {
                        Text = c.Nombre,
                        Value = c.Id.ToString(),
                        Selected = IdCategoria.HasValue && IdCategoria.Value == c.Id
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No fue posible cargar las categorías del filtro");
            }

            try
            {
                var usuarios = await _usuarioReglas.Obtener();
                Empleados = usuarios
                    .Where(u => u.NombreRol == Roles.Empleado || u.NombreRol == Roles.Administrador)
                    .OrderBy(u => u.Nombre)
                    .Select(u => new SelectListItem
                    {
                        Text = $"{u.Nombre} {u.Apellidos}".Trim(),
                        Value = u.Id.ToString(),
                        Selected = IdEmpleado.HasValue && IdEmpleado.Value == u.Id
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No fue posible cargar los empleados del filtro");
            }
        }

        private static string NormalizarReporte(string? valor) => valor?.ToLowerInvariant() switch
        {
            "catalogo" => "catalogo",
            "ventas" => "ventas",
            _ => "inventario"
        };

        private async Task<IActionResult> RegresarSinDatosAsync()
        {
            Error = "No hay datos para exportar con los filtros seleccionados.";
            await CargarFiltrosAsync();
            return Page();
        }

        private FileContentResult ArchivoCsv(string contenido, string nombre)
        {
            var bytes = Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(contenido))
                .ToArray();

            var archivo = $"{nombre}-{DateTime.Now:yyyyMMdd-HHmm}.csv";
            return File(bytes, "text/csv", archivo);
        }

        private static string ConstruirCsv(IEnumerable<string> encabezados, IEnumerable<string[]> filas)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Join(",", encabezados.Select(Escapar)));

            foreach (var fila in filas)
                sb.AppendLine(string.Join(",", fila.Select(Escapar)));

            return sb.ToString();
        }

        private static string Escapar(string? valor)
        {
            valor ??= string.Empty;

            if (valor.Contains('"') || valor.Contains(',') || valor.Contains('\n') || valor.Contains('\r'))
                return $"\"{valor.Replace("\"", "\"\"")}\"";

            return valor;
        }
    }
}
