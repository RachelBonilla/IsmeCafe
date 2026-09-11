using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface ICampanaMarketingFlujo
    {
        Task<CampanaResponse> EnviarOferta(CampanaOfertaRequest campana);
        Task<CampanaResponse> EnviarDescuento(CampanaDescuentoRequest campana);
        Task<IEnumerable<CampanaResponse>> Obtener();
    }
}
