using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.Reglas
{
    public interface IServicioReglas
    {
        Task<IEnumerable<ServicioResponse>> ObtenerActivos();
        Task<IEnumerable<ServicioResponse>> ObtenerTodos();
        Task<bool> Agregar(ServicioRequest servicio);
        Task<bool> Editar(Guid id, ServicioRequest servicio);
        Task<bool> Eliminar(Guid id);
    }
}
