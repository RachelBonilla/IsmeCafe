using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.API
{
    public interface IServicioController
    {
        Task<ActionResult> Obtener();
        Task<ActionResult> ObtenerActivos();
        Task<ActionResult> Obtener(Guid Id);
        Task<ActionResult> Agregar(ServicioRequest servicio);
        Task<ActionResult> Editar(Guid Id, ServicioRequest servicio);
        Task<ActionResult> Eliminar(Guid Id);

    }
}
