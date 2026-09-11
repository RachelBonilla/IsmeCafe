using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Web.Pages.Reservas
{
    public class CrearModel : PageModel
    {
        private readonly IReservaReglas _reservaReglas;

        public CrearModel(IReservaReglas reservaReglas)
        {
            _reservaReglas = reservaReglas;
        }

        [BindProperty(SupportsGet = true)]
        public Guid IdServicio { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        public string NombreCliente { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string Correo { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Ingrese un teléfono válido")]
        public string Telefono { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Seleccione una fecha")]
        public DateTime FechaReserva { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Seleccione una hora")]
        public string HoraSeleccionada { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Seleccione AM o PM")]
        public string Periodo { get; set; } = string.Empty;

        public TimeSpan HoraReserva { get; set; }

        [BindProperty]
        [Range(1, 100, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int CantidadPersonas { get; set; }

        public string? Mensaje { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (FechaReserva.Date < DateTime.Today)
            {
                ModelState.AddModelError(
                    nameof(FechaReserva),
                    "La fecha de reserva no puede ser anterior a hoy."
                );
            }

            if (!string.IsNullOrWhiteSpace(HoraSeleccionada) &&
                !string.IsNullOrWhiteSpace(Periodo))
            {
                var partes = HoraSeleccionada.Split(':');

                if (partes.Length == 2 &&
                    int.TryParse(partes[0], out int hora) &&
                    int.TryParse(partes[1], out int minutos))
                {
                    if (Periodo == "PM" && hora != 12)
                    {
                        hora += 12;
                    }

                    if (Periodo == "AM" && hora == 12)
                    {
                        hora = 0;
                    }

                    HoraReserva = new TimeSpan(hora, minutos, 0);

                    var horaMinima = new TimeSpan(7, 0, 0);
                    var horaMaxima = new TimeSpan(18, 0, 0);

                    if (HoraReserva < horaMinima || HoraReserva > horaMaxima)
                    {
                        ModelState.AddModelError(
                            nameof(HoraSeleccionada),
                            "La hora de reserva debe estar entre las 7:00 AM y las 6:00 PM."
                        );
                    }

                    if (FechaReserva.Date == DateTime.Today &&
                        HoraReserva <= DateTime.Now.TimeOfDay)
                    {
                        ModelState.AddModelError(
                            nameof(HoraSeleccionada),
                            "Si la reserva es para hoy, debe seleccionar una hora posterior a la hora actual."
                        );
                    }
                }
                else
                {
                    ModelState.AddModelError(
                        nameof(HoraSeleccionada),
                        "Seleccione una hora válida."
                    );
                }
            }

            if (IdServicio == Guid.Empty)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "No se ha seleccionado un servicio válido."
                );
            }

            if (!ModelState.IsValid)
            {
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
                var resultado = await _reservaReglas.Agregar(reserva);

                if (!resultado)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "No fue posible realizar la reservación."
                    );

                    return Page();
                }

                Mensaje = "Reservación realizada exitosamente.";

                NombreCliente = string.Empty;
                Correo = string.Empty;
                Telefono = string.Empty;
                FechaReserva = default;
                HoraSeleccionada = string.Empty;
                Periodo = string.Empty;
                HoraReserva = default;
                CantidadPersonas = 0;

                ModelState.Clear();

                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Ocurrió un error al realizar la reservación."
                );

                return Page();
            }
        }
    }
}