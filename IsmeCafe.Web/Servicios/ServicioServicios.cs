using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Servicios
{
    public class ServicioServicios : IServicioServicios
    {

        private const string ClienteHttp = "ServicioServicios";
        private const string LlaveUrlBase = "ApiEndpointServicios:UrlBase";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOpciones = new() { PropertyNameCaseInsensitive = true };

        public ServicioServicios(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        private string UrlBase => _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');
        private HttpClient Cliente => _httpClientFactory.CreateClient(ClienteHttp);

        public async Task<bool> Agregar(ServicioRequest servicio)
        {
            using var contenido = SerializarJson(servicio);
            var respuesta = await Cliente.PostAsync(UrlBase, contenido);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> Editar(Guid id, ServicioRequest servicio)
        {
            using var contenido = SerializarJson(servicio);
            var respuesta = await Cliente.PutAsync($"{UrlBase}/{id}", contenido);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> Eliminar(Guid id)
        {
            var respuesta = await Cliente.DeleteAsync($"{UrlBase}/{id}");
            return respuesta.IsSuccessStatusCode;
        }

        public Task<IEnumerable<ServicioResponse>> ObtenerActivos() => ObtenerLista($"{UrlBase}/activos");

        public Task<IEnumerable<ServicioResponse>> ObtenerTodos() => ObtenerLista(UrlBase);




        #region Helpers
        private StringContent SerializarJson(ServicioRequest servicio)
        {
            var json = JsonSerializer.Serialize(servicio);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<IEnumerable<ServicioResponse>> ObtenerLista(string url)
        {
            var respuesta = await Cliente.GetAsync(url);
            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<ServicioResponse>();

            respuesta.EnsureSuccessStatusCode();

            var contenido = await respuesta.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(contenido))
                return Enumerable.Empty<ServicioResponse>();

            return JsonSerializer.Deserialize<List<ServicioResponse>>(contenido, _jsonOpciones)
                   ?? new List<ServicioResponse>();
        }

#endregion
    
}
}
