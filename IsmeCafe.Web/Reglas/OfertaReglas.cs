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
    public class OfertaReglas : IOfertaReglas
    {
        private readonly IOfertaServicio _ofertaServicio;

        public OfertaReglas(IOfertaServicio ofertaServicio)
        {
            _ofertaServicio = ofertaServicio;
        }

        public Task<IEnumerable<OfertaResponse>> Obtener() => _ofertaServicio.Obtener();
        public Task<IEnumerable<OfertaResponse>> ObtenerActivas() => _ofertaServicio.ObtenerActivas();
        public Task<OfertaDetalle?> Obtener(Guid Id) => _ofertaServicio.Obtener(Id);
        public Task<Guid> Agregar(OfertaRequest oferta) => _ofertaServicio.Agregar(oferta);
        public Task<Guid> Editar(Guid Id, OfertaRequest oferta) => _ofertaServicio.Editar(Id, oferta);
        public Task<Guid> Eliminar(Guid Id) => _ofertaServicio.Eliminar(Id);
    }
}
