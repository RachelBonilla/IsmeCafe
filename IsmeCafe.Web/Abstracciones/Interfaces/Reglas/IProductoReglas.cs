using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Reglas
{
    public interface IProductoReglas
    {
        Task<IEnumerable<ProductoResponse>> ObtenerActivos();
        Task<IEnumerable<ProductoResponse>> ObtenerTodos();
        Task<bool> Agregar(ProductoRequest producto);
        Task<bool> Editar(Guid id, ProductoRequest producto);
        Task<bool> Eliminar(Guid id);
    }
}
