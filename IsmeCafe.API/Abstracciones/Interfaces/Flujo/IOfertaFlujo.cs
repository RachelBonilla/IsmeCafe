using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IOfertaFlujo
    {
        Task<IEnumerable<OfertaResponse>> Obtener();
        Task<OfertaDetalle> Obtener(Guid Id);
        Task<IEnumerable<OfertaResponse>> ObtenerActivas();
        Task<Guid> Agregar(OfertaRequest oferta);
        Task<Guid> Editar(Guid Id, OfertaRequest oferta);
        Task<Guid> Eliminar(Guid Id);
    }
}
