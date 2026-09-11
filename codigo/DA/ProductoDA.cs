using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DA
{
    public class ProductoDA : IProductoDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public ProductoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        #region Operaciones

        public async Task<Guid> Agregar(ProductoRequest producto)
        {
            string query = @"AgregarProducto";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Guid.NewGuid(),
                IdCategoria = producto.IdCategoria,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Imagen = string.IsNullOrWhiteSpace(producto.Imagen) ? "/img/productos/default.png" : producto.Imagen,
                Cantidad = producto.Cantidad,
                Activo = producto.Activo,
                FechaCreacion = DateTime.Now
            });

            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid Id, ProductoRequest producto)
        {
            await VerificarProductoExiste(Id);
            string query = @"EditarProducto";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Id,
                IdCategoria = producto.IdCategoria,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Imagen = producto.Imagen,
                Cantidad = producto.Cantidad,
                Activo = producto.Activo,
                FechaActualizacion = DateTime.Now
            });

            return resultadoConsulta;
        }

        public async Task<Guid> Eliminar(Guid Id)
        {
            await VerificarProductoExiste(Id);
            string query = @"EliminarProducto";
            await _sqlConnection.ExecuteScalarAsync<Guid>(query, new { Id = Id });

            return Id;
        }

        public async Task<IEnumerable<ProductoResponse>> Obtener()
        {
            string query = @"ObtenerProductos";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProductoResponse>(query);

            return resultadoConsulta;
        }

        public async Task<ProductoDetalle> Obtener(Guid Id)
        {
            string query = @"ObtenerProducto";
            var resultadoConsulta = await _sqlConnection.QueryFirstOrDefaultAsync<ProductoDetalle>(query, new
            {
                Id = Id
            });

            return resultadoConsulta;
        }

        public async Task<IEnumerable<ProductoResponse>> ObtenerActivos()
        {
            string query = @"ObtenerProductosActivos";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProductoResponse>(query);

            return resultadoConsulta;
        }

        #endregion

        #region Helpers

        private async Task VerificarProductoExiste(Guid Id)
        {
            ProductoResponse? resultadoConsultaProducto = await Obtener(Id);

            if (resultadoConsultaProducto == null)
            {
                throw new Exception("No se encontró el producto");
            }
        }

        #endregion
    }
}
