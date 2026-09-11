using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class DescuentoFlujo : IDescuentoFlujo
    {
        private IDescuentoDA _descuentoDA;

        public DescuentoFlujo(IDescuentoDA descuentoDA)
        {
            _descuentoDA = descuentoDA;
        }

        public Task<Guid> Agregar(DescuentoRequest descuento)
        {
            return _descuentoDA.Agregar(descuento);
        }

        public Task<Guid> Editar(Guid Id, DescuentoRequest descuento)
        {
            return _descuentoDA.Editar(Id, descuento);
        }

        public Task<Guid> Eliminar(Guid Id)
        {
            return _descuentoDA.Eliminar(Id);
        }

        public Task<IEnumerable<DescuentoResponse>> Obtener()
        {
            return _descuentoDA.Obtener();
        }

        public Task<DescuentoDetalle> Obtener(Guid Id)
        {
            return _descuentoDA.Obtener(Id);
        }

        public Task<IEnumerable<DescuentoResponse>> ObtenerActivos()
        {
            return _descuentoDA.ObtenerActivos();
        }
    }
}