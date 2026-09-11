using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface ICampanaMarketingDA
    {
        Task<Guid> Registrar(string tipo, Guid? idOferta, Guid? idDescuento, string asunto,
                             string contenido, int cantidadDestinatarios, bool exitosa);
        Task<IEnumerable<CampanaResponse>> Obtener();
    }
}
