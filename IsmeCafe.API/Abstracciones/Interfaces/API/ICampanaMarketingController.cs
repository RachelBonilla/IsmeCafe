using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface ICampanaMarketingController
    {

        Task<ActionResult> EnviarOferta(CampanaOfertaRequest campana);
        Task<ActionResult> EnviarDescuento(CampanaDescuentoRequest campana);
        Task<ActionResult> Obtener();
    }
}
