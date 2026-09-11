using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Microsoft.Data.SqlClient;
using Dapper;

namespace DA
{
    public class DescuentoDA : IDescuentoDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public DescuentoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid> Agregar(DescuentoRequest descuento)
        {
            string query = "AgregarDescuento";
            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Guid.NewGuid(),
                IdProducto = descuento.IdProducto,
                PorcentajeDescuento = descuento.PorcentajeDescuento,
                FechaInicio = descuento.FechaInicio,
                FechaFin = descuento.FechaFin,
                Activo = descuento.Activo,
                FechaCreacion = DateTime.Now
            });
            return resultado;
        }

        public async Task<Guid> Editar(Guid Id, DescuentoRequest descuento)
        {
            await VerificarExiste(Id);
            string query = "EditarDescuento";
            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Id,
                IdProducto = descuento.IdProducto,
                PorcentajeDescuento = descuento.PorcentajeDescuento,
                FechaInicio = descuento.FechaInicio,
                FechaFin = descuento.FechaFin,
                Activo = descuento.Activo,
                FechaActualizacion = DateTime.Now
            });
            return resultado;
        }

        public async Task<Guid> Eliminar(Guid Id)
        {
            await VerificarExiste(Id);
            string query = "EliminarDescuento";
            await _sqlConnection.ExecuteScalarAsync<Guid>(query, new { Id = Id });
            return Id;
        }

        public async Task<IEnumerable<DescuentoResponse>> Obtener()
        {
            string query = "ObtenerDescuentos";
            return await _sqlConnection.QueryAsync<DescuentoResponse>(query);
        }

        public async Task<DescuentoDetalle> Obtener(Guid Id)
        {
            string query = "ObtenerDescuento";
            return await _sqlConnection.QueryFirstOrDefaultAsync<DescuentoDetalle>(query, new { Id = Id });
        }

        public async Task<IEnumerable<DescuentoResponse>> ObtenerActivos()
        {
            string query = "ObtenerDescuentosActivos";
            return await _sqlConnection.QueryAsync<DescuentoResponse>(query);
        }

        private async Task VerificarExiste(Guid Id)
        {
            var descuento = await Obtener(Id);
            if (descuento == null)
                throw new Exception("No se encontró el descuento");
        }
    }
}
