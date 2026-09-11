using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flujo
{
    public class ServicioFlujo : IServicioFlujo
    {
        private IServiciosDA _servicioDA;

        public ServicioFlujo(IServiciosDA servicioDA)
        {
            _servicioDA = servicioDA;
        }

        public Task<Guid> Agregar(ServicioRequest servicio)
        {
            return _servicioDA.Agregar(servicio);
        }

        public Task<Guid> Editar(Guid Id, ServicioRequest servicio)
        {
            return _servicioDA.Editar(Id, servicio);
        }

        public Task<Guid> Eliminar(Guid Id)
        {
            return _servicioDA.Eliminar(Id);
        }

        public Task<IEnumerable<ServicioResponse>> Obtener()
        {
            return _servicioDA.Obtener();
        }

        public Task<ServicioDetalle> Obtener(Guid Id)
        {
            return _servicioDA.Obtener(Id);
        }

        public Task<IEnumerable<ServicioResponse>> ObtenerActivos()
        {
            return _servicioDA.ObtenerActivos();
        }
    }
}
