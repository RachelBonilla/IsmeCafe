using System.Net;
using System.Text;
using System.Text.Json;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;

namespace Servicios
{
    public class PedidoServicio : IPedidoServicio
    {
        private const string ClienteHttp = "ServicioPedido";
        private const string LlaveUrlBase = "ApiEndpointPedido:UrlBase";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOpciones = new() { PropertyNameCaseInsensitive = true };

        public PedidoServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        private string UrlBase => _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');
        private HttpClient Cliente => _httpClientFactory.CreateClient(ClienteHttp);

        public async Task<PedidoResponse> Confirmar(PedidoRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            using var contenido = new StringContent(json, Encoding.UTF8, "application/json");
            var respuesta = await Cliente.PostAsync($"{UrlBase}/confirmar", contenido);
            var cuerpo = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(ExtraerMensajeError(cuerpo) ?? "No se pudo confirmar el pedido.");

            return JsonSerializer.Deserialize<PedidoResponse>(cuerpo, _jsonOpciones)
                   ?? throw new Exception("Respuesta vacía al confirmar el pedido.");
        }

        public async Task<PedidoResponse?> Obtener(Guid id)
        {
            var respuesta = await Cliente.GetAsync($"{UrlBase}/{id}");
            if (respuesta.StatusCode == HttpStatusCode.NotFound) return null;

            respuesta.EnsureSuccessStatusCode();
            var cuerpo = await respuesta.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(cuerpo)) return null;

            return JsonSerializer.Deserialize<PedidoResponse>(cuerpo, _jsonOpciones);
        }

        public async Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario)
        {
            var respuesta = await Cliente.GetAsync($"{UrlBase}/usuario/{idUsuario}");
            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<PedidoResponse>();

            respuesta.EnsureSuccessStatusCode();
            var cuerpo = await respuesta.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(cuerpo))
                return Enumerable.Empty<PedidoResponse>();

            return JsonSerializer.Deserialize<List<PedidoResponse>>(cuerpo, _jsonOpciones)
                   ?? new List<PedidoResponse>();
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

        public async Task<IEnumerable<PedidoResponse>> ObtenerTodos()
        {
            var respuesta = await Cliente.GetAsync($"{UrlBase}");

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<PedidoResponse>();

            respuesta.EnsureSuccessStatusCode();

            var cuerpo = await respuesta.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(cuerpo))
                return Enumerable.Empty<PedidoResponse>();

            return JsonSerializer.Deserialize<List<PedidoResponse>>(cuerpo, _jsonOpciones)
                   ?? new List<PedidoResponse>();
        }

        public async Task<IEnumerable<PedidoEstadoResponse>> ObtenerEstados()
        {
            var respuesta = await Cliente.GetAsync($"{UrlBase}/estados");

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<PedidoEstadoResponse>();

            respuesta.EnsureSuccessStatusCode();

            var cuerpo = await respuesta.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(cuerpo))
                return Enumerable.Empty<PedidoEstadoResponse>();

            return JsonSerializer.Deserialize<List<PedidoEstadoResponse>>(cuerpo, _jsonOpciones)
                   ?? new List<PedidoEstadoResponse>();
        }

        public async Task<PedidoResponse> ActualizarEstado( Guid idPedido, ActualizarPedidoEstadoRequest request)
        {
            var json = JsonSerializer.Serialize(request);
            using var contenido = new StringContent( json, Encoding.UTF8, "application/json");
            var respuesta = await Cliente.PutAsync( $"{UrlBase}/{idPedido}/estado", contenido);
            var cuerpo = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
                throw new Exception(ExtraerMensajeError(cuerpo) 
                    ?? "No se pudo actualizar el estado del pedido.");

            return JsonSerializer.Deserialize<PedidoResponse>( cuerpo, _jsonOpciones) 
                ?? throw new Exception( "Respuesta vacía al actualizar el estado del pedido.");
        }
    }
}
