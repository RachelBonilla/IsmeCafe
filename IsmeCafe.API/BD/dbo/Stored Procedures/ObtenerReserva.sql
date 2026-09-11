CREATE PROCEDURE [dbo].[ObtenerReserva]
    @Id UNIQUEIDENTIFIER
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
    WHERE r.Id = @Id;
END;