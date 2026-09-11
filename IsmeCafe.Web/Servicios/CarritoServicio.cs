using System.Net;
using System.Text;
using System.Text.Json;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;

namespace Servicios
{
    public class CarritoServicio : ICarritoServicio
    {
        private const string ClienteHttp = "ServicioCarrito";
        private const string LlaveUrlBase = "ApiEndpointCarrito:UrlBase";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOpciones = new() { PropertyNameCaseInsensitive = true };

        public CarritoServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        private string UrlBase => _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');
        private HttpClient Cliente => _httpClientFactory.CreateClient(ClienteHttp);

        public async Task<CarritoResponse> Obtener(Guid idUsuario, bool usarPuntos = false)
        {
            var respuesta = await Cliente.GetAsync($"{UrlBase}/{idUsuario}?usarPuntos={usarPuntos}");
            return await DeserializarCarrito(respuesta, idUsuario);
        }

        public async Task<CarritoResponse> Agregar(CarritoItemRequest item)
        {
            using var contenido = Serializar(item);
            var respuesta = await Cliente.PostAsync(UrlBase, contenido);
            return await DeserializarCarrito(respuesta, item.IdUsuario);
        }

        public async Task<CarritoResponse> Actualizar(CarritoItemRequest item)
        {
            using var contenido = Serializar(item);
            var respuesta = await Cliente.PutAsync(UrlBase, contenido);
            return await DeserializarCarrito(respuesta, item.IdUsuario);
        }

        public async Task<CarritoResponse> Eliminar(Guid idUsuario, Guid idProducto)
        {
            var respuesta = await Cliente.DeleteAsync($"{UrlBase}/{idUsuario}/{idProducto}");
            return await DeserializarCarrito(respuesta, idUsuario);
        }

        private StringContent Serializar<T>(T objeto)
        {
            var json = JsonSerializer.Serialize(objeto);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<CarritoResponse> DeserializarCarrito(HttpResponseMessage respuesta, Guid idUsuario)
        {
            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return new CarritoResponse { IdUsuario = idUsuario };

            var contenido = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
            {
                var mensaje = ExtraerMensajeError(contenido) ?? respuesta.ReasonPhrase ?? "Error consultando el carrito.";
                throw new Exception(mensaje);
            }

            if (string.IsNullOrWhiteSpace(contenido))
                return new CarritoResponse { IdUsuario = idUsuario };

            return JsonSerializer.Deserialize<CarritoResponse>(contenido, _jsonOpciones)
                   ?? new CarritoResponse { IdUsuario = idUsuario };
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
