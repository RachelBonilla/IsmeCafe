using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;

namespace Reglas
{
    public class ProductoReglas : IProductoReglas
    {
        private readonly IProductoServicio _productoServicio;

        public ProductoReglas(IProductoServicio productoServicio)
        {
            _productoServicio = productoServicio;
        }

        public Task<IEnumerable<ProductoResponse>> ObtenerActivos() => _productoServicio.ObtenerActivos();

        public Task<IEnumerable<ProductoResponse>> ObtenerTodos() => _productoServicio.ObtenerTodos();

        public Task<bool> Agregar(ProductoRequest producto) => _productoServicio.Agregar(producto);

        public Task<bool> Editar(Guid id, ProductoRequest producto) => _productoServicio.Editar(id, producto);

        public Task<bool> Eliminar(Guid id) => _productoServicio.Eliminar(id);
    }
}
