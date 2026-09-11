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
    public class CampanaMarketingServicio : ICampanaMarketingServicio
    {
        private const string ClienteHttp = "ServicioMarketing";
        private const string LlaveUrlBase = "ApiEndpointCampanaMarketing:UrlBase";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOpciones = new() { PropertyNameCaseInsensitive = true };

        public CampanaMarketingServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        private string UrlBase => _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');
        private HttpClient Cliente => _httpClientFactory.CreateClient(ClienteHttp);

        public async Task<IEnumerable<CampanaResponse>> Obtener()
        {
            var respuesta = await Cliente.GetAsync(UrlBase);
            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<CampanaResponse>();

            respuesta.EnsureSuccessStatusCode();
            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CampanaResponse>>(contenido, _jsonOpciones)
                   ?? new List<CampanaResponse>();
        }

        public async Task<CampanaResponse?> EnviarOferta(CampanaOfertaRequest campana)
        {
            var respuesta = await Cliente.PostAsJsonAsync($"{UrlBase}/enviar-oferta", campana);
            if (!respuesta.IsSuccessStatusCode)
            {
                var error = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Error al enviar la campaña de oferta"
                    : error);
            }

            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CampanaResponse>(contenido, _jsonOpciones);
        }

        public async Task<CampanaResponse?> EnviarDescuento(CampanaDescuentoRequest campana)
        {
            var respuesta = await Cliente.PostAsJsonAsync($"{UrlBase}/enviar-descuento", campana);
            if (!respuesta.IsSuccessStatusCode)
            {
                var error = await respuesta.Content.ReadAsStringAsync();
                throw new Exception(string.IsNullOrWhiteSpace(error)
                    ? "Error al enviar la campaña de descuento"
                    : error);
            }

            var contenido = await respuesta.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CampanaResponse>(contenido, _jsonOpciones);
        }
    }
}