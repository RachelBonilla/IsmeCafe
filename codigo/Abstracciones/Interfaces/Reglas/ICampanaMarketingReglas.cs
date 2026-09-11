using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.Reglas
{
    public interface ICampanaMarketingReglas
    {
        Task<IEnumerable<CampanaResponse>> Obtener();
        Task<CampanaResponse?> EnviarOferta(CampanaOfertaRequest campana);
        Task<CampanaResponse?> EnviarDescuento(CampanaDescuentoRequest campana);
    }
}