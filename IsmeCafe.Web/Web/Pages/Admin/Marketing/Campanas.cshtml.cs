using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Marketing
{
    public class CampanasModel : PageModel
    {
        private readonly ICampanaMarketingReglas _campanaReglas;
        private readonly IOfertaReglas _ofertaReglas;
        private readonly IDescuentoReglas _descuentoReglas;
        private readonly ILogger<CampanasModel> _logger;

        public CampanasModel(
            ICampanaMarketingReglas campanaReglas,
            IOfertaReglas ofertaReglas,
            IDescuentoReglas descuentoReglas,
            ILogger<CampanasModel> logger)
        {
            _campanaReglas = campanaReglas;
            _ofertaReglas = ofertaReglas;
            _descuentoReglas = descuentoReglas;
            _logger = logger;
        }

        [BindProperty]
        public CampanaOfertaRequest CampanaOferta { get; set; } = new();

        [BindProperty]
        public CampanaDescuentoRequest CampanaDescuento { get; set; } = new();

        public IReadOnlyList<CampanaResponse> Campanas { get; private set; } = new List<CampanaResponse>();
        public IReadOnlyList<OfertaResponse> OfertasVigentes { get; private set; } = new List<OfertaResponse>();
        public IReadOnlyList<DescuentoResponse> DescuentosVigentes { get; private set; } = new List<DescuentoResponse>();

        public string? Mensaje { get; private set; }
        public bool EsExito { get; private set; }

        public string PestanaActiva { get; private set; } = "Oferta";

        public async Task OnGetAsync()
        {
            await CargarDatosAsync();
        }

        public async Task<IActionResult> OnPostEnviarOfertaAsync()
        {
            PestanaActiva = "Oferta";

            ModelState.Clear();
            TryValidateModel(CampanaOferta, nameof(CampanaOferta));

            await CargarDatosAsync();

            if (!ModelState.IsValid)
                return Page();

            try
            {
                var resultado = await _campanaReglas.EnviarOferta(CampanaOferta);
                EsExito = resultado?.Exitosa ?? false;
                Mensaje = EsExito
                    ? $"Campaña enviada correctamente a {resultado!.CantidadDestinatarios} destinatario(s)."
                    : $"La campaña se registró (a {resultado?.CantidadDestinatarios ?? 0} destinatario(s)) pero el envío del correo falló. Revisa la configuración SMTP y los logs del servidor.";
                CampanaOferta = new CampanaOfertaRequest();
                await CargarDatosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar la campaña de oferta");
                Mensaje = $"No se pudo enviar la campaña: {ex.Message}";
                EsExito = false;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEnviarDescuentoAsync()
        {
            PestanaActiva = "Descuento";

            ModelState.Clear();
            TryValidateModel(CampanaDescuento, nameof(CampanaDescuento));

            await CargarDatosAsync();

            if (!ModelState.IsValid)
                return Page();

            try
            {
                var resultado = await _campanaReglas.EnviarDescuento(CampanaDescuento);
                EsExito = resultado?.Exitosa ?? false;
                Mensaje = EsExito
                    ? $"Campaña enviada correctamente a {resultado!.CantidadDestinatarios} destinatario(s)."
                    : $"La campaña se registró (a {resultado?.CantidadDestinatarios ?? 0} destinatario(s)) pero el envío del correo falló. Revisa la configuración SMTP y los logs del servidor.";
                CampanaDescuento = new CampanaDescuentoRequest();
                await CargarDatosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar la campaña de descuento");
                Mensaje = $"No se pudo enviar la campaña: {ex.Message}";
                EsExito = false;
            }

            return Page();
        }

        private async Task CargarDatosAsync()
        {
            try { Campanas = (await _campanaReglas.Obtener()).ToList(); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de campañas");
                Campanas = new List<CampanaResponse>();
            }

            try { OfertasVigentes = (await _ofertaReglas.ObtenerActivas()).ToList(); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ofertas vigentes");
                OfertasVigentes = new List<OfertaResponse>();
            }

            try { DescuentosVigentes = (await _descuentoReglas.ObtenerActivos()).ToList(); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener descuentos vigentes");
                DescuentosVigentes = new List<DescuentoResponse>();
            }
        }
    }
}