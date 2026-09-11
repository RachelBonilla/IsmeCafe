using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Flujo
{
    public class CampanaMarketingFlujo : ICampanaMarketingFlujo
    {
        private ICampanaMarketingDA _campanaDA;
        private IOfertaDAcs _ofertaDA;
        private IDescuentoDA _descuentoDA;
        private IUsuarioDA _usuarioDA;
        private IConfiguration _configuracion;
        private ILogger<CampanaMarketingFlujo> _logger;

        public CampanaMarketingFlujo(
            ICampanaMarketingDA campanaDA,
            IOfertaDAcs ofertaDA,
            IDescuentoDA descuentoDA,
            IUsuarioDA usuarioDA,
            IConfiguration configuracion,
            ILogger<CampanaMarketingFlujo> logger)
        {
            _campanaDA = campanaDA;
            _ofertaDA = ofertaDA;
            _descuentoDA = descuentoDA;
            _usuarioDA = usuarioDA;
            _configuracion = configuracion;
            _logger = logger;
        }

        public async Task<CampanaResponse> EnviarOferta(CampanaOfertaRequest campana)
        {
            var oferta = await _ofertaDA.Obtener(campana.IdOferta);
            if (oferta == null)
                throw new Exception("La oferta no existe");

            if (!oferta.Activo)
                throw new Exception("La oferta no está activa");

            if (DateTime.Now < oferta.FechaInicio || DateTime.Now > oferta.FechaFin)
                throw new Exception("La oferta no está vigente");

            if (oferta.Productos == null || oferta.Productos.Count == 0)
                throw new Exception("La oferta no tiene productos asociados");

            var destinatarios = await ObtenerDestinatariosAsync();
            if (!destinatarios.Any())
                throw new Exception("No hay clientes suscritos para enviar la campaña");

            string cuerpo = ArmarCuerpoOferta(oferta, campana.Contenido);

            bool exitosa = await EnviarCorreo(destinatarios, campana.Asunto, cuerpo);

            var id = await _campanaDA.Registrar(
                tipo: "Oferta",
                idOferta: campana.IdOferta,
                idDescuento: null,
                asunto: campana.Asunto,
                contenido: cuerpo,
                cantidadDestinatarios: destinatarios.Count,
                exitosa: exitosa
            );

            return new CampanaResponse
            {
                Id = id,
                Tipo = "Oferta",
                IdOferta = campana.IdOferta,
                IdDescuento = null,
                Asunto = campana.Asunto,
                FechaEnvio = DateTime.Now,
                CantidadDestinatarios = destinatarios.Count,
                Exitosa = exitosa
            };
        }

        public async Task<CampanaResponse> EnviarDescuento(CampanaDescuentoRequest campana)
        {
            var descuento = await _descuentoDA.Obtener(campana.IdDescuento);
            if (descuento == null)
                throw new Exception("El descuento no existe");

            if (!descuento.Activo)
                throw new Exception("El descuento no está activo");

            if (DateTime.Now < descuento.FechaInicio || DateTime.Now > descuento.FechaFin)
                throw new Exception("El descuento no está vigente");

            var destinatarios = await ObtenerDestinatariosAsync();
            if (!destinatarios.Any())
                throw new Exception("No hay clientes suscritos para enviar la campaña");

            string cuerpo = ArmarCuerpoDescuento(descuento, campana.Contenido);

            bool exitosa = await EnviarCorreo(destinatarios, campana.Asunto, cuerpo);

            var id = await _campanaDA.Registrar(
                tipo: "Descuento",
                idOferta: null,
                idDescuento: campana.IdDescuento,
                asunto: campana.Asunto,
                contenido: cuerpo,
                cantidadDestinatarios: destinatarios.Count,
                exitosa: exitosa
            );

            return new CampanaResponse
            {
                Id = id,
                Tipo = "Descuento",
                IdOferta = null,
                IdDescuento = campana.IdDescuento,
                Asunto = campana.Asunto,
                FechaEnvio = DateTime.Now,
                CantidadDestinatarios = destinatarios.Count,
                Exitosa = exitosa
            };
        }

        public Task<IEnumerable<CampanaResponse>> Obtener()
        {
            return _campanaDA.Obtener();
        }

        #region Helpers

        private async Task<List<string>> ObtenerDestinatariosAsync()
        {
            var correos = await _usuarioDA.ObtenerCorreosSuscritosMarketing();
            return correos?.ToList() ?? new List<string>();
        }

        private string ArmarCuerpoOferta(OfertaDetalle oferta, string? contenidoExtra)
        {
            var sb = new StringBuilder();
            sb.AppendLine("¡Hola!");
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(contenidoExtra))
            {
                sb.AppendLine(contenidoExtra);
                sb.AppendLine();
            }

            sb.AppendLine($"Oferta: {oferta.Nombre} ({oferta.TipoOferta})");
            sb.AppendLine($"Descripción: {oferta.Descripcion}");
            sb.AppendLine($"Vigente: {oferta.FechaInicio:dd/MM/yyyy} → {oferta.FechaFin:dd/MM/yyyy}");

            if (oferta.PrecioCombo.HasValue)
                sb.AppendLine($"Precio especial del combo: ₡{oferta.PrecioCombo:N0}");

            sb.AppendLine();
            sb.AppendLine("Productos incluidos:");

            foreach (var p in oferta.Productos)
            {
                sb.AppendLine($"  • {p.NombreProducto} — Cantidad: {p.Cantidad} — Precio unit.: ₡{p.PrecioProducto:N0}");
            }

            sb.AppendLine();
            sb.AppendLine("¡Visítanos en Isme Café!");
            return sb.ToString();
        }

        private string ArmarCuerpoDescuento(DescuentoDetalle descuento, string? contenidoExtra)
        {
            var sb = new StringBuilder();
            sb.AppendLine("¡Hola!");
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(contenidoExtra))
            {
                sb.AppendLine(contenidoExtra);
                sb.AppendLine();
            }

            decimal precioConDescuento = descuento.PrecioProducto * (1 - descuento.PorcentajeDescuento / 100m);

            sb.AppendLine($"Descuento especial en: {descuento.NombreProducto}");
            sb.AppendLine($"Descuento: {descuento.PorcentajeDescuento}%");
            sb.AppendLine($"Precio regular: ₡{descuento.PrecioProducto:N0}");
            sb.AppendLine($"Precio con descuento: ₡{precioConDescuento:N0}");
            sb.AppendLine($"Vigente: {descuento.FechaInicio:dd/MM/yyyy} → {descuento.FechaFin:dd/MM/yyyy}");
            sb.AppendLine();
            sb.AppendLine("¡Visítanos en Isme Café!");
            return sb.ToString();
        }

        private async Task<bool> EnviarCorreo(List<string> destinatarios, string asunto, string cuerpo)
        {
            var seccion = _configuracion.GetSection("Smtp");
            bool modoSimulacion = seccion.GetValue<bool>("ModoSimulacion");

            if (modoSimulacion)
            {
                _logger.LogInformation(
                    "[SIMULACIÓN CORREO] Asunto: {Asunto} | Destinatarios: {Count} | Cuerpo: {Cuerpo}",
                    asunto, destinatarios.Count, cuerpo);
                return true;
            }

            try
            {
                string host = seccion["Host"] ?? "smtp.gmail.com";
                int puerto = seccion.GetValue<int>("Puerto");
                string usuario = seccion["Usuario"] ?? "";
                string clave = seccion["Clave"] ?? "";
                bool ssl = seccion.GetValue<bool>("Ssl");

                using var mensaje = new MailMessage
                {
                    From = new MailAddress(usuario, "Isme Café"),
                    Subject = asunto,
                    Body = cuerpo,
                    IsBodyHtml = false
                };

                foreach (var correo in destinatarios)
                    mensaje.Bcc.Add(correo);

                using var cliente = new SmtpClient(host, puerto)
                {
                    EnableSsl = ssl,
                    Credentials = new NetworkCredential(usuario, clave)
                };

                await cliente.SendMailAsync(mensaje);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar correo de campaña");
                return false;
            }
        }

        #endregion
    }
}