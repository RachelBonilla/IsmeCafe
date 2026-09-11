using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private const string ClienteHttp = "ServicioUsuarios";
        private const string LlaveUrlBase = "ApiEndpointUsuario:UrlBase";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOpciones = new() { PropertyNameCaseInsensitive = true };

        public UsuarioServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        private string UrlBase => _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');
        private HttpClient Cliente => _httpClientFactory.CreateClient(ClienteHttp);

        public async Task<bool> Agregar(UsuarioRequest usuario)
        {
            using var contenido = SerializarJson(usuario);
            var respuesta = await Cliente.PostAsync($"{UrlBase}/registrar-usuario", contenido);
            return respuesta.IsSuccessStatusCode;
        }
        public async Task<bool> AgregarCliente(UsuarioClienteRequest usuario)
        {
            using var contenido = SerializarClienteJson(usuario);
            var respuesta = await Cliente.PostAsync($"{UrlBase}/registrar-cliente", contenido);
            return respuesta.IsSuccessStatusCode;
        }

        public Task<IEnumerable<UsuarioDetalle>> Obtener() => ObtenerLista($"{UrlBase}");

        public async Task<UsuarioDetalle> Obtener(Guid id)
        {
            var respuesta = await Cliente.GetAsync($"{UrlBase}/{id}");

            if (respuesta.StatusCode == HttpStatusCode.NotFound || respuesta.StatusCode == HttpStatusCode.NoContent)
                return null;

            respuesta.EnsureSuccessStatusCode();

            var contenido = await respuesta.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(contenido))
                return null;

            return JsonSerializer.Deserialize<UsuarioDetalle>(contenido, _jsonOpciones);
        }

        public async Task<bool> Desactivar(Guid id)
        {
            var respuesta = await Cliente.PutAsync($"{UrlBase}/desactivar/{id}", null);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> Editar(Guid id, UsuarioEditarRequest usuario)
        {
            using var contenido = SerializarEditarJson(usuario);
            var respuesta = await Cliente.PutAsync($"{UrlBase}/editar/{id}", contenido);
            return respuesta.IsSuccessStatusCode;
        }

        #region Helpers

        private StringContent SerializarJson(UsuarioRequest usuario)
        {
            var json = JsonSerializer.Serialize(usuario);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private StringContent SerializarEditarJson(UsuarioEditarRequest usuario)
        {
            var json = JsonSerializer.Serialize(usuario);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        private StringContent SerializarClienteJson(UsuarioClienteRequest usuario)
        {
            var json = JsonSerializer.Serialize(usuario);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<IEnumerable<UsuarioDetalle>> ObtenerLista(string url)
        {
            var respuesta = await Cliente.GetAsync(url);
            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<UsuarioDetalle>();

            respuesta.EnsureSuccessStatusCode();

            var contenido = await respuesta.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(contenido))
                return Enumerable.Empty<UsuarioDetalle>();

            return JsonSerializer.Deserialize<List<UsuarioDetalle>>(contenido, _jsonOpciones)
                   ?? new List<UsuarioDetalle>();
        }

        #endregion
    }
}