using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Headers;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly JsonSerializerOptions _jsonOpciones = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public UsuarioServicio(
            IConfiguracion configuracion,
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private string UrlBase =>
            _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');

        // Crea el HttpClient y agrega el JWT si existe
        private HttpClient ObtenerCliente()
        {
            var cliente = _httpClientFactory.CreateClient(ClienteHttp);

            var token = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst("Token")?
                .Value;

            if (!string.IsNullOrWhiteSpace(token))
            {
                cliente.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return cliente;
        }

        public async Task<bool> Agregar(UsuarioRequest usuario)
        {
            using var contenido = SerializarJson(usuario);

            var cliente = ObtenerCliente();

            var respuesta = await cliente.PostAsync(
                $"{UrlBase}/registrar-usuario",
                contenido
            );

            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> AgregarCliente(UsuarioClienteRequest usuario)
        {
            using var contenido = SerializarClienteJson(usuario);

            var cliente = ObtenerCliente();

            var respuesta = await cliente.PostAsync(
                $"{UrlBase}/registrar-cliente",
                contenido
            );

            return respuesta.IsSuccessStatusCode;
        }

        public Task<IEnumerable<UsuarioDetalle>> Obtener()
        {
            return ObtenerLista(UrlBase);
        }

        public async Task<UsuarioDetalle?> Obtener(Guid id)
        {
            var cliente = ObtenerCliente();

            var respuesta = await cliente.GetAsync(
                $"{UrlBase}/{id}"
            );

            if (respuesta.StatusCode == HttpStatusCode.NotFound ||
                respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            respuesta.EnsureSuccessStatusCode();

            var contenido =
                await respuesta.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(contenido))
                return null;

            return JsonSerializer.Deserialize<UsuarioDetalle>(
                contenido,
                _jsonOpciones
            );
        }

        public async Task<bool> Desactivar(Guid id)
        {
            var cliente = ObtenerCliente();

            var respuesta = await cliente.PutAsync(
                $"{UrlBase}/desactivar/{id}",
                null
            );

            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> Editar(
            Guid id,
            UsuarioEditarRequest usuario)
        {
            using var contenido = SerializarEditarJson(usuario);

            var cliente = ObtenerCliente();

            var respuesta = await cliente.PutAsync(
                $"{UrlBase}/editar/{id}",
                contenido
            );

            return respuesta.IsSuccessStatusCode;
        }

        #region Helpers

        private StringContent SerializarJson(
            UsuarioRequest usuario)
        {
            var json = JsonSerializer.Serialize(usuario);

            return new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );
        }

        private StringContent SerializarEditarJson(
            UsuarioEditarRequest usuario)
        {
            var json = JsonSerializer.Serialize(usuario);

            return new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );
        }

        private StringContent SerializarClienteJson(
            UsuarioClienteRequest usuario)
        {
            var json = JsonSerializer.Serialize(usuario);

            return new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );
        }

        private async Task<IEnumerable<UsuarioDetalle>> ObtenerLista(
            string url)
        {
            var cliente = ObtenerCliente();

            var respuesta = await cliente.GetAsync(url);

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                return Enumerable.Empty<UsuarioDetalle>();
            }

            respuesta.EnsureSuccessStatusCode();

            var contenido =
                await respuesta.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(contenido))
            {
                return Enumerable.Empty<UsuarioDetalle>();
            }

            return JsonSerializer.Deserialize<List<UsuarioDetalle>>(
                       contenido,
                       _jsonOpciones
                   )
                   ?? new List<UsuarioDetalle>();
        }

        #endregion
    }
}