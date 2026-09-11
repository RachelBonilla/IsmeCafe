using Abstracciones.Modelos;


namespace Abstracciones.Interfaces.Servicios
{
    public interface IServicioServicios
    {
        Task<IEnumerable<ServicioResponse>> ObtenerActivos();
        Task<IEnumerable<ServicioResponse>> ObtenerTodos();
        Task<bool> Agregar(ServicioRequest servicio);
        Task<bool> Editar(Guid id, ServicioRequest servicio);
        Task<bool> Eliminar(Guid id);
    }
}
