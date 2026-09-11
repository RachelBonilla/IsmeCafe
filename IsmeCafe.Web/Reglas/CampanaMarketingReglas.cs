using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reglas
{
    public class CampanaMarketingReglas : ICampanaMarketingReglas
    {
        private readonly ICampanaMarketingServicio _campanaServicio;

        public CampanaMarketingReglas(ICampanaMarketingServicio campanaServicio)
        {
            _campanaServicio = campanaServicio;
        }

        public Task<IEnumerable<CampanaResponse>> Obtener() => _campanaServicio.Obtener();
        public Task<CampanaResponse?> EnviarOferta(CampanaOfertaRequest campana) => _campanaServicio.EnviarOferta(campana);
        public Task<CampanaResponse?> EnviarDescuento(CampanaDescuentoRequest campana) => _campanaServicio.EnviarDescuento(campana);
    }
}