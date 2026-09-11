using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Auth;
using System.Text;
using System.Text.Json;

namespace Servicios
{
    public class LoginServicio : ILoginServicio
    {

        private const string ClienteHttp = "ServicioAuth";
        private const string LlaveUrlBase = "ApiEndpointIniciarSesion";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOpciones = new() { PropertyNameCaseInsensitive = true };

        private HttpClient Cliente => _httpClientFactory.CreateClient(ClienteHttp);

        public LoginServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<LoginResponse> IniciarSesion(LoginRequest credenciales)
        {
            var url = _configuracion.ObtenerMetodo(LlaveUrlBase, "IniciarSesion");
            var contenido = new StringContent(JsonSerializer.Serialize(credenciales),Encoding.UTF8,"application/json");
            var respuesta = await Cliente.PostAsync(url,contenido);

            if (!respuesta.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await respuesta.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<LoginResponse>(json,_jsonOpciones);
        }

        public async Task<bool> SolicitarRecuperacion(SolicitarRecuperacionRequest request)
        {
            var url = _configuracion.ObtenerMetodo(LlaveUrlBase, "SolicitarRecuperacion");
            var json = JsonSerializer.Serialize(request);

            using var contenido = new StringContent(json,Encoding.UTF8,"application/json");

            var respuesta = await Cliente.PostAsync(url,contenido);

            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> RestablecerContrasena(RestablecerContrasenaRequest request)
        {
            var url = _configuracion.ObtenerMetodo(LlaveUrlBase, "RestablecerContrasena");
            var json = JsonSerializer.Serialize(request);

            using var contenido = new StringContent(json,Encoding.UTF8,"application/json");

            var respuesta = await Cliente.PostAsync(url, contenido);

            return respuesta.IsSuccessStatusCode;
        }
    }
}
