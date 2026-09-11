using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Auth;
using System.Text.Json;

namespace Servicios
{
    public class LoginServicio : ILoginServicio
    {

        private const string ClienteHttp = "ServicioAuth";
        private const string LlaveUrlBase = "ApiEndpointIniciarSesion:UrlBase";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOpciones = new() { PropertyNameCaseInsensitive = true };

        private string UrlBase => _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');
        private HttpClient Cliente => _httpClientFactory.CreateClient(ClienteHttp);

        public LoginServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<LoginResponse> IniciarSesion(LoginRequest credenciales)
        {
            var contenido = new StringContent(JsonSerializer.Serialize(credenciales), System.Text.Encoding.UTF8, "application/json");
            var respuesta = await Cliente.PostAsync(UrlBase, contenido);

            if (!respuesta.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponse>(json, _jsonOpciones);
        }
    }
}
