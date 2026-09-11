using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class ServicioDA : IServiciosDA
    {

        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;

        public ServicioDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        #region Operaciones

        public async Task<Guid> Agregar(ServicioRequest servicio)
        {
            string query = @"AgregarServicio";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Guid.NewGuid(),
                Nombre = servicio.Nombre,
                Descripcion = servicio.Descripcion,
                Duracion = servicio.Duracion,
                Precio = servicio.Precio,
                CupoMaximo = servicio.CupoMaximo,
                Activo = servicio.Activo,
                FechaCreacion = DateTime.Now
            }, commandType: System.Data.CommandType.StoredProcedure);

            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid Id, ServicioRequest servicio)
        {
            await VerificarServicioExiste(Id);
            string query = @"EditarServicio";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Id,
                Nombre = servicio.Nombre,
                Descripcion = servicio.Descripcion,
                Duracion = servicio.Duracion,
                Precio = servicio.Precio,
                CupoMaximo = servicio.CupoMaximo,
                Activo = servicio.Activo
            }, commandType: System.Data.CommandType.StoredProcedure);

            return resultadoConsulta;
        }

        public async Task<Guid> Eliminar(Guid Id)
        {
            await VerificarServicioExiste(Id);
            string query = @"EliminarServicio";
            await _sqlConnection.ExecuteScalarAsync<Guid>(query, new { Id = Id }, commandType: System.Data.CommandType.StoredProcedure);

            return Id;
        }

        public async Task<IEnumerable<ServicioResponse>> Obtener()
        {
            string query = @"ObtenerServicios";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ServicioResponse>(query, commandType: System.Data.CommandType.StoredProcedure);

            return resultadoConsulta;
        }

        public async Task<ServicioDetalle> Obtener(Guid Id)
        {
            string query = @"ObtenerServicio";
            var resultadoConsulta = await _sqlConnection.QueryFirstOrDefaultAsync<ServicioDetalle>(query, new
            {
                Id = Id
            }, commandType: System.Data.CommandType.StoredProcedure);

            return resultadoConsulta;
        }

        public async Task<IEnumerable<ServicioResponse>> ObtenerActivos()
        {
            string query = @"ObtenerServiciosActivos";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ServicioResponse>(query, commandType: System.Data.CommandType.StoredProcedure);

            return resultadoConsulta;
        }

        #endregion

        #region Helpers

        private async Task VerificarServicioExiste(Guid Id)
        {
            ServicioDetalle? resultadoConsultaServicio = await Obtener(Id);

            if (resultadoConsultaServicio == null)
            {
                throw new Exception("No se encontró el servicio");
            }
        }

        #endregion

    }
}
