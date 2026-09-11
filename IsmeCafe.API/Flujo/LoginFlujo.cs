using Abstracciones.Constantes;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Seguridad;
using Abstracciones.Modelos.Auth;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
namespace Flujo
{
    public class LoginFlujo : ILoginFlujo
    {
        private readonly ILoginDA _loginDA;
        private readonly IJwtAuthorization _JwtAuthorization;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuracion;

        public LoginFlujo(ILoginDA loginDA, IJwtAuthorization jwtAuthorization,
            IMemoryCache memoryCache,IConfiguration configuracion)
        {
            _loginDA = loginDA;
            _JwtAuthorization = jwtAuthorization;
            _memoryCache = memoryCache;
            _configuracion = configuracion;
        }

        public async Task<LoginResponse> IniciarSesion(LoginRequest request)
        {

            var usuarioDb = await _loginDA.ObtenerUsuarioPorCorreo(request.Correo);

            // Validaciones de datos del usuario
            if (usuarioDb == null)
                throw new Exception("Correo o contraseña incorrectos.");

            if (!usuarioDb.Activo)
                throw new Exception("Su cuenta se encuentra desactivada. Contacte al administrador.");

            if (!BCrypt.Net.BCrypt.Verify(request.Contrasena, usuarioDb.Contrasena))
                throw new Exception("Correo o contraseña incorrectos.");

            // Validación de roles
            bool esRolEmpleado = usuarioDb.NombreRol == Roles.Administrador ||
                                 usuarioDb.NombreRol == Roles.Empleado;

            if (request.EsEmpleado && !esRolEmpleado)
                throw new Exception("Acceso denegado. No tiene permisos para ingresar como empleado.");

            if (!request.EsEmpleado && esRolEmpleado)
                throw new Exception("Acceso denegado. Los empleados deben utilizar el acceso especial.");

            // Construir Token JWT
            var nombreCompleto = $"{usuarioDb.Nombre} {usuarioDb.Apellidos}".Trim();
            var token = _JwtAuthorization.GenerarToken(usuarioDb.Id,nombreCompleto,usuarioDb.Correo,usuarioDb.NombreRol);

            // Retornar respuesta exitosa (se incluye datos basicos del usuario).
            return new LoginResponse
            {
                Id = usuarioDb.Id,
                NombreCompleto = $"{usuarioDb.Nombre} {usuarioDb.Apellidos}".Trim(),
                Correo = usuarioDb.Correo,
                NombreRol = usuarioDb.NombreRol,
                EsEmpleado = request.EsEmpleado,
                Token = token
            };
        }

        public async Task SolicitarRecuperacion(SolicitarRecuperacionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Correo))
                throw new Exception("Debe ingresar un correo electrónico.");

            var usuario = await _loginDA.ObtenerUsuarioPorCorreo(request.Correo);

            if (usuario == null)
                return;

            if (!usuario.Activo)
                return;

            // Generar OTP seguro de 6 dígitos.
            string codigo = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            var fechaExpiracion = DateTime.UtcNow.AddMinutes(5);
            var recuperacion = new RecuperacionContrasena
            {
                Codigo = codigo,
                Intentos = 0,
                FechaExpiracion = fechaExpiracion
            };

            // Guardar temporalmente durante 5 minutos.
            string cacheKey = $"recuperacion-contrasena:{request.Correo.ToLower()}";

            _memoryCache.Set(cacheKey,recuperacion,TimeSpan.FromMinutes(5));

            await EnviarCodigoRecuperacion(request.Correo, codigo);
        }

        private async Task EnviarCodigoRecuperacion(string correo,string codigo)
        {
            var seccion = _configuracion.GetSection("Smtp");

            bool modoSimulacion =
                seccion.GetValue<bool>("ModoSimulacion");

            if (modoSimulacion)
            {
                return;
            }

            string host = seccion["Host"] ?? "smtp.gmail.com";
            int puerto = seccion.GetValue<int>("Puerto");
            string usuario = seccion["Usuario"] ?? "";
            string clave = seccion["Clave"] ?? "";
            bool ssl = seccion.GetValue<bool>("Ssl");

            using var mensaje = new MailMessage
            {
                From = new MailAddress(usuario, "Isme Café"),
                Subject = "Código para restablecer tu contraseña",
                Body = $"""
                Hola,

                Hemos recibido una solicitud para restablecer tu contraseña.

                Tu código de verificación es:

                {codigo}

                Este código será válido durante 5 minutos.

                Si no solicitaste este cambio, puedes ignorar este mensaje.

                Isme Café
                """,
                IsBodyHtml = false
            };

            mensaje.To.Add(correo);

            using var cliente = new SmtpClient(host, puerto)
            {
                EnableSsl = ssl,
                Credentials = new NetworkCredential(usuario, clave)
            };

            await cliente.SendMailAsync(mensaje);
        }

        public async Task RestablecerContrasena( RestablecerContrasenaRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Correo))
                throw new Exception("Debe ingresar el correo electrónico.");

            if (string.IsNullOrWhiteSpace(request.Codigo))
                throw new Exception("Debe ingresar el código de recuperación.");

            if (string.IsNullOrWhiteSpace(request.NuevaContrasena))
                throw new Exception("Debe ingresar la nueva contraseña.");

            string cacheKey = $"recuperacion-contrasena:{request.Correo.Trim().ToLowerInvariant()}";

            if (!_memoryCache.TryGetValue(cacheKey,out RecuperacionContrasena? recuperacion) || recuperacion == null)
            {
                throw new Exception("El código de recuperación es inválido o ha expirado.");
            }

            if (DateTime.UtcNow > recuperacion.FechaExpiracion)
            {
                _memoryCache.Remove(cacheKey);
                throw new Exception("El código de recuperación ha expirado.");
            }

            if (recuperacion.Intentos >= 5)
            {
                _memoryCache.Remove(cacheKey);
                throw new Exception("Se ha excedido la cantidad máxima de intentos.");
            }

            if (recuperacion.Codigo != request.Codigo)
            {
                recuperacion.Intentos++;

                if (recuperacion.Intentos >= 5)
                {
                    _memoryCache.Remove(cacheKey);

                    throw new Exception("Se ha excedido la cantidad máxima de intentos.");
                }

                throw new Exception("El código de recuperación es incorrecto.");
            }

            var usuario = await _loginDA.ObtenerUsuarioPorCorreo(request.Correo.Trim());

            if (usuario == null || !usuario.Activo)
                throw new Exception( "No fue posible restablecer la contraseña.");

            string nuevaContrasenaHash = BCrypt.Net.BCrypt.HashPassword(request.NuevaContrasena);

            await _loginDA.ActualizarContrasena( usuario.Id, nuevaContrasenaHash);

            _memoryCache.Remove(cacheKey);
        }
    }
}
