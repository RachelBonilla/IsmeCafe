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
    public class DescuentoServicio : IDescuentoServicio
    {
        private const string ClienteHttp = "ServicioMarketing";
        private const string LlaveUrlBase = "ApiEndpointDescuento:UrlBase";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOpciones = new() { PropertyNameCaseInsensitive = true };

        public DescuentoServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        private string UrlBase => _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');
        private HttpClient Cliente => _httpClientFactory.CreateClient(ClienteHttp);

        public async Task<IEnumerable<DescuentoResponse>> Obtener()
        {
            var respuesta = await Cliente.GetAsync(UrlBase);
            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<DescuentoResponse>();

            respuesta.EnsureSuccessStatusCode();
            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<DescuentoResponse>>(contenido, _jsonOpciones)
                   ?? new List<DescuentoResponse>();
        }

        public async Task<IEnumerable<DescuentoResponse>> ObtenerActivos()
        {
            var respuesta = await Cliente.GetAsync($"{UrlBase}/activos");
            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<DescuentoResponse>();

            respuesta.EnsureSuccessStatusCode();
            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<DescuentoResponse>>(contenido, _jsonOpciones)
                   ?? new List<DescuentoResponse>();
        }

        public async Task<DescuentoDetalle?> Obtener(Guid Id)
        {
            var respuesta = await Cliente.GetAsync($"{UrlBase}/{Id}");
            if (respuesta.StatusCode == HttpStatusCode.NotFound)
                return null;

            respuesta.EnsureSuccessStatusCode();
            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DescuentoDetalle>(contenido, _jsonOpciones);
        }

        public async Task<Guid> Agregar(DescuentoRequest descuento)
        {
            var respuesta = await Cliente.PostAsJsonAsync(UrlBase, descuento);
            respuesta.EnsureSuccessStatusCode();
            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Guid>(contenido, _jsonOpciones);
        }

        public async Task<Guid> Editar(Guid Id, DescuentoRequest descuento)
        {
            var respuesta = await Cliente.PutAsJsonAsync($"{UrlBase}/{Id}", descuento);
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