using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Marketing
{
    public class OfertasModel : PageModel
    {
        private readonly IOfertaReglas _ofertaReglas;
        private readonly IProductoReglas _productoReglas;
        private readonly IConfiguration _configuration;

        public OfertasModel(IOfertaReglas ofertaReglas, IProductoReglas productoReglas, IConfiguration configuration)
        {
            _ofertaReglas = ofertaReglas;
            _productoReglas = productoReglas;
            _configuration = configuration;
        }

        public IReadOnlyList<OfertaResponse> Ofertas { get; private set; } = new List<OfertaResponse>();
        public IReadOnlyList<ProductoResponse> Productos { get; private set; } = new List<ProductoResponse>();
        public string ApiUrl { get; private set; } = string.Empty;

        public async Task OnGet()
        {
            ApiUrl = _configuration.GetSection("ApiEndpointOferta:UrlBase").Value ?? string.Empty;

            try { Ofertas = (await _ofertaReglas.Obtener()).ToList(); }
            catch { Ofertas = new List<OfertaResponse>(); }

            try { Productos = (await _productoReglas.ObtenerActivos()).ToList(); }
            catch { Productos = new List<ProductoResponse>(); }
        }
    }
}