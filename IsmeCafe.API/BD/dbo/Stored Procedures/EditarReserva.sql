CREATE PROCEDURE [dbo].[EditarReserva]
    @Id UNIQUEIDENTIFIER,
    @IdServicio UNIQUEIDENTIFIER,
    @NombreCliente VARCHAR(100),
    @Correo VARCHAR(150),
    @Telefono VARCHAR(20),
    @FechaReserva DATE,
    @HoraReserva TIME,
    @CantidadPersonas INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Reservas]
    SET
        IdServicio = @IdServicio,
        NombreCliente = @NombreCliente,
        Correo = @Correo,
        Telefono = @Telefono,
        FechaReserva = @FechaReserva,
        HoraReserva = @HoraReserva,
        CantidadPersonas = @CantidadPersonas
    WHERE Id = @Id;

    SELECT @Id;
END;