using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.Servicios
{
    public interface IOfertaServicio
    {
        Task<IEnumerable<OfertaResponse>> Obtener();
        Task<IEnumerable<OfertaResponse>> ObtenerActivas();
        Task<OfertaDetalle?> Obtener(Guid Id);
        Task<Guid> Agregar(OfertaRequest oferta);
        Task<Guid> Editar(Guid Id, OfertaRequest oferta);
        Task<Guid> Eliminar(Guid Id);
    }
}