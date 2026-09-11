using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.Reglas
{
    public interface IDescuentoReglas
    {
        Task<IEnumerable<DescuentoResponse>> Obtener();
        Task<IEnumerable<DescuentoResponse>> ObtenerActivos();
        Task<DescuentoDetalle?> Obtener(Guid Id);
        Task<Guid> Agregar(DescuentoRequest descuento);
        Task<Guid> Editar(Guid Id, DescuentoRequest descuento);
        Task<Guid> Eliminar(Guid Id);
    }
}
