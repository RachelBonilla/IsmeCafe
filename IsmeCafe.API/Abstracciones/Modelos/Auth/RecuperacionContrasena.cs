namespace Abstracciones.Modelos.Auth
{
    public class RecuperacionContrasena
    {
        public string Codigo { get; set; } = string.Empty;
        public int Intentos { get; set; }
        public DateTime FechaExpiracion { get; set; }
    }
}
