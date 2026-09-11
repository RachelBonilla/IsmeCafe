using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IRolController
    {
        Task<ActionResult> Obtener();
    }
}
