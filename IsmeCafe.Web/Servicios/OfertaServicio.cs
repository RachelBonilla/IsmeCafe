using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Servicios
{
    public class OfertaServicio : IOfertaServicio
    {
        private const string ClienteHttp = "ServicioMarketing";
        private const string LlaveUrlBase = "ApiEndpointOferta:UrlBase";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOpciones = new() { PropertyNameCaseInsensitive = true };

        public OfertaServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        private string UrlBase => _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');
        private HttpClient Cliente => _httpClientFactory.CreateClient(ClienteHttp);

        public async Task<IEnumerable<OfertaResponse>> Obtener()
        {
            var respuesta = await Cliente.GetAsync(UrlBase);
            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<OfertaResponse>();

            respuesta.EnsureSuccessStatusCode();
            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<OfertaResponse>>(contenido, _jsonOpciones)
                   ?? new List<OfertaResponse>();
        }

        public async Task<IEnumerable<OfertaResponse>> ObtenerActivas()
        {
            var respuesta = await Cliente.GetAsync($"{UrlBase}/activas");
            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<OfertaResponse>();

            respuesta.EnsureSuccessStatusCode();
            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<OfertaResponse>>(contenido, _jsonOpciones)
                   ?? new List<OfertaResponse>();
        }

        public async Task<OfertaDetalle?> Obtener(Guid Id)
        {
            var respuesta = await Cliente.GetAsync($"{UrlBase}/{Id}");
            if (respuesta.StatusCode == HttpStatusCode.NotFound)
                return null;

            respuesta.EnsureSuccessStatusCode();
            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OfertaDetalle>(contenido, _jsonOpciones);
        }

        public async Task<Guid> Agregar(OfertaRequest oferta)
        {
            var respuesta = await Cliente.PostAsJsonAsync(UrlBase, oferta);
            respuesta.EnsureSuccessStatusCode();
            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Guid>(contenido, _jsonOpciones);
        }

        public async Task<Guid> Editar(Guid Id, OfertaRequest oferta)
        {
            var respuesta = await Cliente.PutAsJsonAsync($"{UrlBase}/{Id}", oferta);
            respuesta.EnsureSuccessStatusCode();
            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Guid>(contenido, _jsonOpciones);
        }

        public async Task<Guid> Eliminar(Guid Id)
        {
            var respuesta = await Cliente.DeleteAsync($"{UrlBase}/{Id}");
            respuesta.EnsureSuccessStatusCode();
            return Id;
        }
    }
}
