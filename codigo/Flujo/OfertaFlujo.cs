using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flujo
{
    public class OfertaFlujo : IOfertaFlujo
    {
        private IOfertaDAcs _ofertaDA;

        public OfertaFlujo(IOfertaDAcs ofertaDA)
        {
            _ofertaDA = ofertaDA;
        }

        public Task<Guid> Agregar(OfertaRequest oferta)
        {
            return _ofertaDA.Agregar(oferta);
        }

        public Task<Guid> Editar(Guid Id, OfertaRequest oferta)
        {
            return _ofertaDA.Editar(Id, oferta);
        }

        public Task<Guid> Eliminar(Guid Id)
        {
            return _ofertaDA.Eliminar(Id);
        }

        public Task<IEnumerable<OfertaResponse>> Obtener()
        {
            return _ofertaDA.Obtener();
        }

        public Task<OfertaDetalle> Obtener(Guid Id)
        {
            return _ofertaDA.Obtener(Id);
        }

        public Task<IEnumerable<OfertaResponse>> ObtenerActivas()
        {
            return _ofertaDA.ObtenerActivas();
        }
    }
}