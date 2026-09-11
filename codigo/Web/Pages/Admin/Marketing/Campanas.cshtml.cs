using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Marketing
{
    public class CampanasModel : PageModel
    {
        private readonly ICampanaMarketingReglas _campanaReglas;
        private readonly IDescuentoReglas _descuentoReglas;
        private readonly IOfertaReglas _ofertaReglas;
        private readonly IConfiguration _configuration;

        public CampanasModel(
            ICampanaMarketingReglas campanaReglas,
            IDescuentoReglas descuentoReglas,
            IOfertaReglas ofertaReglas,
            IConfiguration configuration)
        {
            _campanaReglas = campanaReglas;
            _descuentoReglas = descuentoReglas;
            _ofertaReglas = ofertaReglas;
            _configuration = configuration;
        }

        public IReadOnlyList<CampanaResponse> Campanas { get; private set; } = new List<CampanaResponse>();
        public IReadOnlyList<DescuentoResponse> Descuentos { get; private set; } = new List<DescuentoResponse>();
        public IReadOnlyList<OfertaResponse> Ofertas { get; private set; } = new List<OfertaResponse>();
        public string ApiUrl { get; private set; } = string.Empty;

        public async Task OnGet()
        {
            ApiUrl = _configuration.GetSection("ApiEndpointCampanaMarketing:UrlBase").Value ?? string.Empty;

            try { Campanas = (await _campanaReglas.Obtener()).ToList(); }
            catch { Campanas = new List<CampanaResponse>(); }

            try { Descuentos = (await _descuentoReglas.ObtenerActivos()).ToList(); }
            catch { Descuentos = new List<DescuentoResponse>(); }

            try { Ofertas = (await _ofertaReglas.ObtenerActivas()).ToList(); }
            catch { Ofertas = new List<OfertaResponse>(); }
        }
    }
}
