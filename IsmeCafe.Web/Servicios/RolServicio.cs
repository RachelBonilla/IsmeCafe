using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using System.Net;
using System.Text.Json;

namespace Servicios
{
    public class RolServicio : IRolServicio
    {
        private const string ClienteHttp = "ServicioUsuarios";
        private const string LlaveUrlBase = "ApiEndpointRoles:UrlBase";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOpciones = new() { PropertyNameCaseInsensitive = true };

        public RolServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        private string UrlBase => _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');
        private HttpClient Cliente => _httpClientFactory.CreateClient(ClienteHttp);

        public async Task<IEnumerable<Rol>> Obtener()
        {
            var respuesta = await Cliente.GetAsync(UrlBase);

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<Rol>();

            respuesta.EnsureSuccessStatusCode();

            var contenido = await respuesta.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(contenido))
                return Enumerable.Empty<Rol>();

            return JsonSerializer.Deserialize<List<Rol>>(contenido, _jsonOpciones)
                   ?? new List<Rol>();
        }
    }
}