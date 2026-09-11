using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Admin.Reservas
{
    public class GestionReservasModel : PageModel
    {
        private readonly IReservaReglas _reservaReglas;
        private readonly ILogger<GestionReservasModel> _logger;

        public GestionReservasModel(
            IReservaReglas reservaReglas,
            ILogger<GestionReservasModel> logger)
        {
            _reservaReglas = reservaReglas;
            _logger = logger;
        }

        public IEnumerable<ReservaResponse> Reservas { get; set; }
            = new List<ReservaResponse>();

        public string? Mensaje { get; set; }

        public bool EsExito { get; set; }

        public async Task OnGetAsync()
        {
            await CargarReservasAsync();
        }

        public async Task<IActionResult> OnPostEditarAsync(
            Guid id,
            Guid IdServicio,
            string NombreCliente,
            string Correo,
            string Telefono,
            DateTime FechaReserva,
            TimeSpan HoraReserva,
            int CantidadPersonas)
        {
            if (FechaReserva.Date < DateTime.Today)
            {
                Mensaje = "La fecha de reserva no puede ser anterior a hoy.";
                EsExito = false;

                await CargarReservasAsync();

                return Page();
            }

            var horaMinima = new TimeSpan(7, 0, 0);
            var horaMaxima = new TimeSpan(18, 0, 0);

            if (HoraReserva < horaMinima || HoraReserva > horaMaxima)
            {
                Mensaje = "La hora de reserva debe estar entre las 7:00 AM y las 6:00 PM.";
                EsExito = false;

                await CargarReservasAsync();

                return Page();
            }

            if (CantidadPersonas < 1)
            {
                Mensaje = "La cantidad de personas debe ser mayor a cero.";
                EsExito = false;

                await CargarReservasAsync();

                return Page();
            }

            var reserva = new ReservaRequest
            {
                IdServicio = IdServicio,
                NombreCliente = NombreCliente,
                Correo = Correo,
                Telefono = Telefono,
                FechaReserva = FechaReserva,
                HoraReserva = HoraReserva,
                CantidadPersonas = CantidadPersonas
            };

            try
            {
                var resultado = await _reservaReglas.Editar(id, reserva);

                Mensaje = resultado
                    ? "Reserva actualizada correctamente."
                    : "No fue posible actualizar la reserva.";

                EsExito = resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al editar la reserva {Id}",
                    id);

                Mensaje = "Ocurrió un error al actualizar la reserva.";
                EsExito = false;
            }

            await CargarReservasAsync();

            return Page();
        }

        private async Task CargarReservasAsync()
        {
            try
            {
                Reservas = await _reservaReglas.ObtenerTodos();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al cargar las reservas");

                Reservas = new List<ReservaResponse>();

                Mensaje = "No fue posible cargar las reservas.";
                EsExito = false;
            }
        }
    }
}