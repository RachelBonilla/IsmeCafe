CREATE PROCEDURE [dbo].[AgregarReserva]
    @Id UNIQUEIDENTIFIER,
    @IdServicio UNIQUEIDENTIFIER,
    @NombreCliente VARCHAR(100),
    @Correo VARCHAR(150),
    @Telefono VARCHAR(20),
    @FechaReserva DATE,
    @HoraReserva TIME,
    @CantidadPersonas INT,
    @FechaCreacion DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Reservas]
    (
        Id,
        IdServicio,
        NombreCliente,
        Correo,
        Telefono,
        FechaReserva,
        HoraReserva,
        CantidadPersonas,
        FechaCreacion
    )
    VALUES
    (
        @Id,
        @IdServicio,
        @NombreCliente,
        @Correo,
        @Telefono,
        @FechaReserva,
        @HoraReserva,
        @CantidadPersonas,
        @FechaCreacion
    );

    SELECT @Id;
END;