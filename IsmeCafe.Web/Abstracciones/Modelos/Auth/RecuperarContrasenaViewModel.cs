namespace Abstracciones.Modelos.Auth
{
    public class RecuperarContrasenaViewModel
    {
        public string Correo { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string NuevaContrasena { get; set; } = string.Empty;
        public string ConfirmarContrasena { get; set; } = string.Empty;
    }
}
