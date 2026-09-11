
--Eliminar/Inactivar ofertas--

CREATE PROCEDURE EliminarOferta
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION

	UPDATE [dbo].[Ofertas]
	SET [Activo] = 0,
		[FechaActualizacion] = GETDATE()
	WHERE (Id = @Id)

	SELECT @Id
	COMMIT TRANSACTION
END