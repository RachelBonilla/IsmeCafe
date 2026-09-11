using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Microsoft.Data.SqlClient;
using Dapper;

namespace DA
{
    public class OfertaDA : IOfertaDAcs
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public OfertaDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid> Agregar(OfertaRequest oferta)
        {
            var id = Guid.NewGuid();

            string queryOferta = "AgregarOferta";
            await _sqlConnection.ExecuteScalarAsync<Guid>(queryOferta, new
            {
                Id = id,
                Nombre = oferta.Nombre,
                Descripcion = oferta.Descripcion,
                TipoOferta = oferta.TipoOferta,
                PrecioCombo = oferta.PrecioCombo,
                FechaInicio = oferta.FechaInicio,
                FechaFin = oferta.FechaFin,
                Activo = oferta.Activo,
                FechaCreacion = DateTime.Now
            });

            string queryProducto = "AgregarOfertaProducto";
            foreach (var producto in oferta.Productos)
            {
                await _sqlConnection.ExecuteAsync(queryProducto, new
                {
                    Id = Guid.NewGuid(),
                    IdOferta = id,
                    IdProducto = producto.IdProducto,
                    Cantidad = producto.Cantidad
                });
            }

            return id;
        }

        public async Task<Guid> Editar(Guid Id, OfertaRequest oferta)
        {
            await VerificarExiste(Id);

            string queryOferta = "EditarOferta";
            await _sqlConnection.ExecuteScalarAsync<Guid>(queryOferta, new
            {
                Id = Id,
                Nombre = oferta.Nombre,
                Descripcion = oferta.Descripcion,
                TipoOferta = oferta.TipoOferta,
                PrecioCombo = oferta.PrecioCombo,
                FechaInicio = oferta.FechaInicio,
                FechaFin = oferta.FechaFin,
                Activo = oferta.Activo,
                FechaActualizacion = DateTime.Now
            });

            await _sqlConnection.ExecuteAsync("EliminarOfertaProductos", new { IdOferta = Id });

            string queryProducto = "AgregarOfertaProducto";
            foreach (var producto in oferta.Productos)
            {
                await _sqlConnection.ExecuteAsync(queryProducto, new
                {
                    Id = Guid.NewGuid(),
                    IdOferta = Id,
                    IdProducto = producto.IdProducto,
                    Cantidad = producto.Cantidad
                });
            }

            return Id;
        }

        public async Task<Guid> Eliminar(Guid Id)
        {
            await VerificarExiste(Id);
            await _sqlConnection.ExecuteAsync("EliminarOfertaProductos", new { IdOferta = Id });
            await _sqlConnection.ExecuteScalarAsync<Guid>("EliminarOferta", new { Id = Id });
            return Id;
        }

        public async Task<IEnumerable<OfertaResponse>> Obtener()
        {
            string query = "ObtenerOfertas";
            return await _sqlConnection.QueryAsync<OfertaResponse>(query);
        }

        public async Task<OfertaDetalle> Obtener(Guid Id)
        {
            string query = "ObtenerOferta";
            using var multi = await _sqlConnection.QueryMultipleAsync(query, new { Id = Id });

            var oferta = await multi.ReadFirstOrDefaultAsync<OfertaDetalle>();
            if (oferta != null)
            {
                var productos = await multi.ReadAsync<OfertaProductoResponse>();
                oferta.Productos = productos.ToList();
            }

            return oferta!;
        }

        public async Task<IEnumerable<OfertaResponse>> ObtenerActivas()
        {
            string query = "ObtenerOfertasActivas";
            return await _sqlConnection.QueryAsync<OfertaResponse>(query);
        }

        private async Task VerificarExiste(Guid Id)
        {
            using var multi = await _sqlConnection.QueryMultipleAsync("ObtenerOferta", new { Id = Id });
            var oferta = await multi.ReadFirstOrDefaultAsync<OfertaResponse>();
            if (oferta == null)
                throw new Exception("No se encontró la oferta");
        }
    }
}
