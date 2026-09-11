using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;

namespace Servicios
{
    public class ProductoServicio : IProductoServicio
    {
        private const string ClienteHttp = "ServicioProductos";
        private const string LlaveUrlBase = "ApiEndpointProducto:UrlBase";

        private readonly IConfiguracion _configuracion;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOpciones = new() { PropertyNameCaseInsensitive = true };

        public ProductoServicio(IConfiguracion configuracion, IHttpClientFactory httpClientFactory)
        {
            _configuracion = configuracion;
            _httpClientFactory = httpClientFactory;
        }

        private string UrlBase => _configuracion.ObtenerValor(LlaveUrlBase).TrimEnd('/');
        private HttpClient Cliente => _httpClientFactory.CreateClient(ClienteHttp);

        public Task<IEnumerable<ProductoResponse>> ObtenerActivos() => ObtenerLista($"{UrlBase}/activos");

        public Task<IEnumerable<ProductoResponse>> ObtenerTodos() => ObtenerLista(UrlBase);

        public async Task<bool> Agregar(ProductoRequest producto)
        {
            using var contenido = SerializarJson(producto);
            var respuesta = await Cliente.PostAsync(UrlBase, contenido);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> Editar(Guid id, ProductoRequest producto)
        {
            using var contenido = SerializarJson(producto);
            var respuesta = await Cliente.PutAsync($"{UrlBase}/{id}", contenido);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> Eliminar(Guid id)
        {
            var respuesta = await Cliente.DeleteAsync($"{UrlBase}/{id}");
            return respuesta.IsSuccessStatusCode;
        }

        #region Helpers

        private StringContent SerializarJson(ProductoRequest producto)
        {
            var json = JsonSerializer.Serialize(producto);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<IEnumerable<ProductoResponse>> ObtenerLista(string url)
        {
            var respuesta = await Cliente.GetAsync(url);
            if (respuesta.StatusCode == HttpStatusCode.NoContent)
                return Enumerable.Empty<ProductoResponse>();

            respuesta.EnsureSuccessStatusCode();

            var contenido = await respuesta.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(contenido))
                return Enumerable.Empty<ProductoResponse>();

            return JsonSerializer.Deserialize<List<ProductoResponse>>(contenido, _jsonOpciones)
                   ?? new List<ProductoResponse>();
        }

        #endregion
    }
}
