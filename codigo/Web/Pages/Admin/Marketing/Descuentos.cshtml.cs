using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Marketing
{
    public class DescuentosModel : PageModel
    {
        private readonly IDescuentoReglas _descuentoReglas;
        private readonly IProductoReglas _productoReglas;
        private readonly IConfiguration _configuration;

        public DescuentosModel(IDescuentoReglas descuentoReglas, IProductoReglas productoReglas, IConfiguration configuration)
        {
            _descuentoReglas = descuentoReglas;
            _productoReglas = productoReglas;
            _configuration = configuration;
        }

        public IReadOnlyList<DescuentoResponse> Descuentos { get; private set; } = new List<DescuentoResponse>();
        public IReadOnlyList<ProductoResponse> Productos { get; private set; } = new List<ProductoResponse>();
        public string ApiUrl { get; private set; } = string.Empty;

        public async Task OnGet()
        {
            ApiUrl = _configuration.GetSection("ApiEndpointDescuento:UrlBase").Value ?? string.Empty;

            try { Descuentos = (await _descuentoReglas.Obtener()).ToList(); }
            catch { Descuentos = new List<DescuentoResponse>(); }

            try { Productos = (await _productoReglas.ObtenerActivos()).ToList(); }
            catch { Productos = new List<ProductoResponse>(); }
        }
    }
}