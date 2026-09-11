using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DA
{
    public class PedidoDA : IPedidoDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;

        public PedidoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<PedidoResponse> Confirmar(Guid idUsuario)
        {
            var idPedido = Guid.NewGuid();

            var cabecera = await _sqlConnection.QueryFirstAsync<(int NumeroPedido, decimal Total, DateTime Fecha, string Estado)>(
                "ConfirmarPedido",
                new { IdUsuario = idUsuario, IdPedido = idPedido },
                commandType: CommandType.StoredProcedure);

            var pedido = await Obtener(idPedido);
            return pedido ?? new PedidoResponse
            {
                Id = idPedido,
                NumeroPedido = cabecera.NumeroPedido,
                IdUsuario = idUsuario,
                Fecha = cabecera.Fecha,
                Estado = cabecera.Estado,
                Total = cabecera.Total
            };
        }

        public async Task<PedidoResponse?> Obtener(Guid id)
        {
            using var multi = await _sqlConnection.QueryMultipleAsync(
                "ObtenerPedido",
                new { Id = id },
                commandType: CommandType.StoredProcedure);

            var pedido = await multi.ReadFirstOrDefaultAsync<PedidoResponse>();
            if (pedido == null) return null;

            var detalle = await multi.ReadAsync<PedidoDetalleResponse>();
            pedido.Detalle = detalle.ToList();

            return pedido;
        }

        public async Task<IEnumerable<PedidoResponse>> ObtenerPorUsuario(Guid idUsuario)
        {
            var resultado = await _sqlConnection.QueryAsync<PedidoResponse>(
                "ObtenerPedidosPorUsuario",
                new { IdUsuario = idUsuario },
                commandType: CommandType.StoredProcedure);

            return resultado;
        }
    }
}
