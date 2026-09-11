namespace Abstracciones.Modelos.Auth
{
    public class LoginResponse
    {
        public Guid Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string NombreRol { get; set; } = string.Empty;
        public bool EsEmpleado { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
