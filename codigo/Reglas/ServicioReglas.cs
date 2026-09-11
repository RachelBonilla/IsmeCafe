using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reglas
{
    public class ServicioReglas : IServicioReglas
    {
        private readonly IServicioServicios _servicioServicio;

        public ServicioReglas(IServicioServicios servicioServicio)
        {
            _servicioServicio = servicioServicio;
        }

        public Task<IEnumerable<ServicioResponse>> ObtenerActivos() => _servicioServicio.ObtenerActivos();

        public Task<IEnumerable<ServicioResponse>> ObtenerTodos() => _servicioServicio.ObtenerTodos();

        public Task<bool> Agregar(ServicioRequest servicio) => _servicioServicio.Agregar(servicio);

        public Task<bool> Editar(Guid id, ServicioRequest servicio) => _servicioServicio.Editar(id, servicio);

        public Task<bool> Eliminar(Guid id) => _servicioServicio.Eliminar(id);
    }
}
