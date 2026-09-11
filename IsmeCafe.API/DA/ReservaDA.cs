using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DA
{
    public class ReservaDA : IReservaDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;

        public ReservaDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        #region Operaciones

        public async Task<Guid> Agregar(ReservaRequest reserva)
        {
            string query = @"AgregarReserva";

            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(
                query,
                new
                {
                    Id = Guid.NewGuid(),
                    IdServicio = reserva.IdServicio,
                    NombreCliente = reserva.NombreCliente,
                    Correo = reserva.Correo,
                    Telefono = reserva.Telefono,
                    FechaReserva = reserva.FechaReserva,
                    HoraReserva = reserva.HoraReserva,
                    CantidadPersonas = reserva.CantidadPersonas,
                    FechaCreacion = DateTime.Now
                },
                commandType: CommandType.StoredProcedure
            );

            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid Id, ReservaRequest reserva)
        {
            await VerificarReservaExiste(Id);

            string query = @"EditarReserva";

            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(
                query,
                new
                {
                    Id = Id,
                    IdServicio = reserva.IdServicio,
                    NombreCliente = reserva.NombreCliente,
                    Correo = reserva.Correo,
                    Telefono = reserva.Telefono,
                    FechaReserva = reserva.FechaReserva,
                    HoraReserva = reserva.HoraReserva,
                    CantidadPersonas = reserva.CantidadPersonas
                },
                commandType: CommandType.StoredProcedure
            );

            return resultadoConsulta;
        }

        public async Task<Guid> Eliminar(Guid Id)
        {
            await VerificarReservaExiste(Id);

            string query = @"EliminarReserva";

            await _sqlConnection.ExecuteScalarAsync<Guid>(
                query,
                new { Id = Id },
                commandType: CommandType.StoredProcedure
            );

            return Id;
        }

        public async Task<IEnumerable<ReservaResponse>> Obtener()
        {
            string query = @"ObtenerReservas";

            var resultadoConsulta =
                await _sqlConnection.QueryAsync<ReservaResponse>(
                    query,
                    commandType: CommandType.StoredProcedure
                );

            return resultadoConsulta;
        }

        public async Task<ReservaResponse> Obtener(Guid Id)
        {
            string query = @"ObtenerReserva";

            var resultadoConsulta =
                await _sqlConnection.QueryFirstOrDefaultAsync<ReservaResponse>(
                    query,
                    new
                    {
                        Id = Id
                    },
                    commandType: CommandType.StoredProcedure
                );

            return resultadoConsulta;
        }

        #endregion

        #region Helpers

        private async Task VerificarReservaExiste(Guid Id)
        {
            ReservaResponse? resultadoConsultaReserva = await Obtener(Id);

            if (resultadoConsultaReserva == null)
            {
                throw new Exception("No se encontró la reserva");
            }
        }

        #endregion
    }
}