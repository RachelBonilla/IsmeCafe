using System.Globalization;
using System.Net;
using System.Text.Json;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;

namespace Servicios
{

    public class ReporteServicio : IReporteServicio
    {
        private const string ClienteHttp = "ServicioReporte";
        private const string LlaveUrlBase = "ApiEndpointReporte:UrlBase";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOpciones = new() { PropertyNameCaseInsensitive = true };

        public ReporteServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        private string UrlBase => _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');
        private HttpClient Cliente => _httpClientFactory.CreateClient(ClienteHttp);

        public async Task<IEnumerable<ReporteInventarioLinea>> Inventario(Guid? idCategoria)
        {
            var url = $"{UrlBase}/inventario";
            if (idCategoria.HasValue && idCategoria.Value != Guid.Empty)
                url += $"?idCategoria={idCategoria.Value}";

            return await ObtenerLista<ReporteInventarioLinea>(url);
        }

        public async Task<IEnumerable<ReporteCatalogoLinea>> Catalogo(bool? activo)
        {
            var url = $"{UrlBase}/catalogo";
            if (activo.HasValue)
                url += $"?activo={(activo.Value ? "true" : "false")}";

            return await ObtenerLista<ReporteCatalogoLinea>(url);
        }

        public async Task<IEnumerable<ReporteVentasLinea>> VentasPorEmpleado(ReporteVentasFiltro filtro)
        {
            var inicio = filtro.FechaInicio.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var fin = filtro.FechaFin.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var url = $"{UrlBase}/ventas-empleado?fechaInicio={inicio}&fechaFin={fin}";
            if (filtro.IdEmpleado.HasValue && filtro.IdEmpleado.Value != Guid.Empty)
                url += $"&idEmpleado={filtro.IdEmpleado.Value}";

            return await ObtenerLista<ReporteVentasLinea>(url);
        }

        public async Task<IEnumerable<CategoriaResponse>> Categorias() =>
            await ObtenerLista<CategoriaResponse>($"{UrlBase}/categorias");

        private async Task<IEnumerable<T>> ObtenerLista<T>(string url)
        {
            var respuesta = await Cliente.GetAsync(url);
            var cuerpo = await respuesta.Content.ReadAsStringAsync();

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<T>();

            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(ExtraerMensajeError(cuerpo) ?? "No fue posible generar el reporte.");

            if (string.IsNullOrWhiteSpace(cuerpo))
                return Enumerable.Empty<T>();

            return JsonSerializer.Deserialize<List<T>>(cuerpo, _jsonOpciones) ?? new List<T>();
        }

        private string? ExtraerMensajeError(string contenido)
        {
            if (string.IsNullOrWhiteSpace(contenido)) return null;
            try
            {
                using var doc = JsonDocument.Parse(contenido);
                if (doc.RootElement.TryGetProperty("mensaje", out var mensaje))
                    return mensaje.GetString();
            }
            catch (JsonException) { }
            return null;
        }
    }
}
