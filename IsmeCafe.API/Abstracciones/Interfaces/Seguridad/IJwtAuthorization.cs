namespace Abstracciones.Interfaces.Seguridad
{
    public interface IJwtAuthorization
    {
        string GenerarToken(
            Guid idUsuario,
            string nombreCompleto,
            string correo,
            string nombreRol);
    }
}
