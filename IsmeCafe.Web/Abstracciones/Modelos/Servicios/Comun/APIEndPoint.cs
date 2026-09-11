using System.Collections.Generic;

namespace Abstracciones.Modelos.Servicios.Comun
{
    /// <summary>
    /// Describe la URL base de un servicio externo y los métodos disponibles.
    /// Cada método es una plantilla de ruta con marcadores {0}, {1}, ...
    /// </summary>
    public class APIEndPoint
    {
        public string UrlBase { get; set; } = string.Empty;
        public IEnumerable<Metodo>? Metodos { get; set; }
    }

    public class Metodo
    {
        public string Nombre { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
    }
}
