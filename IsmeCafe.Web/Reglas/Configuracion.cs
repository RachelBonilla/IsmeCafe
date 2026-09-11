using System;
using System.Linq;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Servicios.Comun;
using Microsoft.Extensions.Configuration;

namespace Reglas
{
    public class Configuracion : IConfiguracion
    {
        private readonly IConfiguration _configuracion;

        public Configuracion(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        /// <summary>
        /// Devuelve la plantilla de URL completa ("{UrlBase}/{Valor}") para el
        /// método indicado dentro de una sección de endpoints.
        /// </summary>
        public string ObtenerMetodo(string seccion, string nombre)
        {
            var endpoint = _configuracion.GetSection(seccion).Get<APIEndPoint>()
                ?? throw new InvalidOperationException($"No existe la sección de configuración '{seccion}'.");

            var metodo = endpoint.Metodos?.FirstOrDefault(m => m.Nombre == nombre)
                ?? throw new InvalidOperationException($"No existe el método '{nombre}' en la sección '{seccion}'.");

            return $"{endpoint.UrlBase}/{metodo.Valor}";
        }

        public string ObtenerValor(string llave)
        {
            return _configuracion.GetSection(llave).Value ?? string.Empty;
        }
    }
}
