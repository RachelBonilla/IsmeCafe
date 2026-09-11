using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Marketing
{

    public class OfertasModel : PageModel
    {
        private readonly IOfertaReglas _ofertaReglas;
        private readonly IProductoReglas _productoReglas;
        private readonly ILogger<OfertasModel> _logger;

        public OfertasModel(IOfertaReglas ofertaReglas, IProductoReglas productoReglas, ILogger<OfertasModel> logger)
        {
            _ofertaReglas = ofertaReglas;
            _productoReglas = productoReglas;
            _logger = logger;
        }

        public static readonly string[] TiposOferta = { "2x1", "3x2", "3x1", "4x3" };

        public class LineaProducto
        {
            public Guid IdProducto { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public decimal Precio { get; set; }
            public bool Incluido { get; set; }
            public int Cantidad { get; set; } = 1;
        }

        [BindProperty]
        public OfertaBase Datos { get; set; } = new() { Activo = true, TipoOferta = "2x1" };

        [BindProperty]
        public List<LineaProducto> Lineas { get; set; } = new();

        [BindProperty]
        public Guid? EditId { get; set; }

        public IReadOnlyList<OfertaDetalle> Ofertas { get; private set; } = new List<OfertaDetalle>();
        public IReadOnlyList<ProductoResponse> Productos { get; private set; } = new List<ProductoResponse>();

        public string? Mensaje { get; private set; }
        public bool EsExito { get; private set; }
        public bool EnEdicion => EditId.HasValue;
        public bool AbrirModalCrear { get; private set; }
        public Guid? AbrirModalEditar { get; private set; }

        public async Task OnGetAsync()
        {
            await CargarProductosAsync();
            await CargarOfertasAsync();
            CargarLineasFrescas();
        }

        public async Task<IActionResult> OnPostGuardarAsync()
        {
            await CargarProductosAsync();
            await CargarOfertasAsync();

            var seleccionados = Lineas.Where(l => l.Incluido).ToList();

            if (Datos.FechaInicio >= Datos.FechaFin)
                ModelState.AddModelError("Datos.FechaFin", "La fecha de fin debe ser posterior a la de inicio");

            if (seleccionados.Count == 0)
                ModelState.AddModelError(string.Empty, "Debe incluir al menos un producto en la oferta");

            if (!ModelState.IsValid)
            {
                if (EnEdicion) AbrirModalEditar = EditId;
                else AbrirModalCrear = true;
                return Page();
            }

            var solicitud = new OfertaRequest
            {
                Nombre = Datos.Nombre,
                Descripcion = Datos.Descripcion,
                TipoOferta = Datos.TipoOferta,
                PrecioCombo = Datos.PrecioCombo,
                FechaInicio = Datos.FechaInicio,
                FechaFin = Datos.FechaFin,
                Activo = Datos.Activo,
                Productos = seleccionados
                    .Select(l => new OfertaProductoRequest { IdProducto = l.IdProducto, Cantidad = l.Cantidad })
                    .ToList()
            };

            try
            {
                string accion;
                if (EnEdicion)
                {
                    await _ofertaReglas.Editar(EditId!.Value, solicitud);
                    accion = "actualizada";
                }
                else
                {
                    await _ofertaReglas.Agregar(solicitud);
                    accion = "registrada";
                }

                Mensaje = $"Oferta {accion} correctamente.";
                EsExito = true;
                ModelState.Clear();
                Datos = new OfertaBase { Activo = true, TipoOferta = "2x1" };
                EditId = null;
                await CargarOfertasAsync();
                CargarLineasFrescas();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la oferta");
                Mensaje = "No fue posible guardar la oferta. Verifica los datos e inténtalo de nuevo.";
                EsExito = false;
                if (EnEdicion) AbrirModalEditar = EditId;
                else AbrirModalCrear = true;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(Guid id)
        {
            try
            {
                await _ofertaReglas.Eliminar(id);
                Mensaje = "Oferta desactivada.";
                EsExito = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar la oferta");
                Mensaje = "No fue posible desactivar la oferta.";
                EsExito = false;
            }

            await CargarProductosAsync();
            await CargarOfertasAsync();
            CargarLineasFrescas();
            return Page();
        }
        private async Task CargarOfertasAsync()
        {
            try
            {
                var resumen = await _ofertaReglas.Obtener();
                var detalles = new List<OfertaDetalle>();
                foreach (var o in resumen)
                {
                    var detalle = await _ofertaReglas.Obtener(o.Id);
                    if (detalle != null) detalles.Add(detalle);
                }
                Ofertas = detalles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ofertas");
                Ofertas = new List<OfertaDetalle>();
            }
        }

        private async Task CargarProductosAsync()
        {
            try { Productos = (await _productoReglas.ObtenerActivos()).ToList(); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                Productos = new List<ProductoResponse>();
            }
        }

        private void CargarLineasFrescas()
        {
            Lineas = Productos.Select(p => new LineaProducto
            {
                IdProducto = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Incluido = false,
                Cantidad = 1
            }).ToList();
        }
    }
}