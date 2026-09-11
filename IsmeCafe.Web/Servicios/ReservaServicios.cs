using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Servicios
{
    public class ReservaServicios : IReservaServicios
    {
        private const string ClienteHttp = "ReservaServicios";
        private const string LlaveUrlBase = "ApiEndpointReserva:UrlBase";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly JsonSerializerOptions _jsonOpciones = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ReservaServicios(
            IConfiguracion configuracion,
            IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        private string UrlBase =>
            _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');

        private HttpClient Cliente =>
            _httpClientFactory.CreateClient(ClienteHttp);

        public async Task<bool> Agregar(ReservaRequest reserva)
        {
            using var contenido = SerializarJson(reserva);

            var respuesta = await Cliente.PostAsync(
                UrlBase,
                contenido
            );

            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> Editar(Guid id, ReservaRequest reserva)
        {
            using var contenido = SerializarJson(reserva);

            var respuesta = await Cliente.PutAsync(
                $"{UrlBase}/{id}",
                contenido
            );

            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> Eliminar(Guid id)
        {
            var respuesta = await Cliente.DeleteAsync(
                $"{UrlBase}/{id}"
            );

            return respuesta.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ReservaResponse>> ObtenerTodos()
        {
            var respuesta = await Cliente.GetAsync(UrlBase);

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<ReservaResponse>();

            respuesta.EnsureSuccessStatusCode();

            var contenido =
                await respuesta.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(contenido))
                return Enumerable.Empty<ReservaResponse>();

            return JsonSerializer.Deserialize<List<ReservaResponse>>(
                       contenido,
                       _jsonOpciones
                   )
                   ?? new List<ReservaResponse>();
        }

        public async Task<ReservaResponse?> Obtener(Guid id)
        {
            var respuesta = await Cliente.GetAsync(
                $"{UrlBase}/{id}"
            );

            if (respuesta.StatusCode == HttpStatusCode.NotFound)
                return null;

            respuesta.EnsureSuccessStatusCode();

            var contenido =
                await respuesta.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(contenido))
                return null;

            return JsonSerializer.Deserialize<ReservaResponse>(
                contenido,
                _jsonOpciones
            );
        }

        private StringContent SerializarJson(
            ReservaRequest reserva)
        {
            var json = JsonSerializer.Serialize(reserva);

            return new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );
        }
    }
}