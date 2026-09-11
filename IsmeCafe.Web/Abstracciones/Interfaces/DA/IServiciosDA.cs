using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.DA
{
    public interface IServiciosDA
    {

        Task<IEnumerable<ServicioResponse>> Obtener();
        Task<ServicioDetalle> Obtener(Guid Id);
        Task<IEnumerable<ServicioResponse>> ObtenerActivos();
        Task<Guid> Agregar(ServicioRequest servicio);
        Task<Guid> Editar(Guid Id, ServicioRequest servicio);
        Task<Guid> Eliminar(Guid Id);
    }
}
