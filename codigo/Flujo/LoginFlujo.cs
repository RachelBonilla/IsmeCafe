using Abstracciones.Constantes;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos.Auth;

namespace Flujo
{
    public class LoginFlujo : ILoginFlujo
    {
        private readonly ILoginDA _loginDA;

        public LoginFlujo(ILoginDA loginDA)
        {
            _loginDA = loginDA;
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

            // Retornar respuesta exitosa (se incluye datos basicos del usuario).
            return new LoginResponse
            {
                Id = usuarioDb.Id,
                NombreCompleto = $"{usuarioDb.Nombre} {usuarioDb.Apellidos}".Trim(),
                Correo = usuarioDb.Correo,
                NombreRol = usuarioDb.NombreRol,
                EsEmpleado = request.EsEmpleado
            };
        }
    }
}
