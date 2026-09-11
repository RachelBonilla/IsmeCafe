using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DA
{
    public class CarritoDA : ICarritoDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;

        public CarritoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> Agregar(Guid idUsuario, Guid idProducto, int cantidad)
        {
            var resultado = await _sqlConnection.QueryFirstAsync<(int CantidadFinal, int StockDisponible)>(
                "AgregarProductoCarrito",
                new { IdUsuario = idUsuario, IdProducto = idProducto, Cantidad = cantidad },
                commandType: CommandType.StoredProcedure);

            return resultado.CantidadFinal;
        }

        public async Task<int> Actualizar(Guid idUsuario, Guid idProducto, int cantidad)
        {
            var resultado = await _sqlConnection.QueryFirstAsync<int>(
                "ActualizarCantidadCarrito",
                new { IdUsuario = idUsuario, IdProducto = idProducto, Cantidad = cantidad },
                commandType: CommandType.StoredProcedure);

            return resultado;
        }

        public async Task<int> Eliminar(Guid idUsuario, Guid idProducto)
        {
            var restantes = await _sqlConnection.QueryFirstAsync<int>(
                "EliminarProductoCarrito",
                new { IdUsuario = idUsuario, IdProducto = idProducto },
                commandType: CommandType.StoredProcedure);

            return restantes;
        }

        public async Task<IEnumerable<CarritoItemResponse>> Obtener(Guid idUsuario)
        {
            var resultado = await _sqlConnection.QueryAsync<CarritoItemResponse>(
                "ObtenerCarrito",
                new { IdUsuario = idUsuario },
                commandType: CommandType.StoredProcedure);

            return resultado;
        }

        public async Task Vaciar(Guid idUsuario)
        {
            await _sqlConnection.ExecuteAsync(
                "VaciarCarrito",
                new { IdUsuario = idUsuario },
                commandType: CommandType.StoredProcedure);
        }
    }
}
