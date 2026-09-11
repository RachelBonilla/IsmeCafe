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
    public class DescuentoReglas : IDescuentoReglas
    {
        private readonly IDescuentoServicio _descuentoServicio;

        public DescuentoReglas(IDescuentoServicio descuentoServicio)
        {
            _descuentoServicio = descuentoServicio;
        }

        public Task<IEnumerable<DescuentoResponse>> Obtener() => _descuentoServicio.Obtener();
        public Task<IEnumerable<DescuentoResponse>> ObtenerActivos() => _descuentoServicio.ObtenerActivos();
        public Task<DescuentoDetalle?> Obtener(Guid Id) => _descuentoServicio.Obtener(Id);
        public Task<Guid> Agregar(DescuentoRequest descuento) => _descuentoServicio.Agregar(descuento);
        public Task<Guid> Editar(Guid Id, DescuentoRequest descuento) => _descuentoServicio.Editar(Id, descuento);
        public Task<Guid> Eliminar(Guid Id) => _descuentoServicio.Eliminar(Id);
    }
}