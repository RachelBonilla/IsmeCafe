CREATE PROCEDURE [dbo].[ObtenerReservas]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        r.Id,
        r.IdServicio,
        s.Nombre AS Servicio,
        r.NombreCliente,
        r.Correo,
        r.Telefono,
        r.FechaReserva,
        r.HoraReserva,
        r.CantidadPersonas,
        r.FechaCreacion
    FROM [dbo].[Reservas] r
    INNER JOIN [dbo].[Servicios] s
        ON r.IdServicio = s.Id
    ORDER BY r.FechaReserva, r.HoraReserva;
END;