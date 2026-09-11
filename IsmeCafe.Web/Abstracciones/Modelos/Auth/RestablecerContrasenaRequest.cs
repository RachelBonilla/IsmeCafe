namespace Abstracciones.Modelos.Auth
{
    public class RestablecerContrasenaRequest
    {
        public string Correo { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string NuevaContrasena { get; set; } = string.Empty;
    }
}