namespace Abstracciones.Modelos.Auth
{
    public class UsuarioAutenticacion
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public Guid IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
    }
}
