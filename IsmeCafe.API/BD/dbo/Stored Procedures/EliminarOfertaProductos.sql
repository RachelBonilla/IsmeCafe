
--Eliminar oferta a un producto--
CREATE PROCEDURE EliminarOfertaProductos
	@IdOferta UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION

	DELETE FROM [dbo].[OfertaProductos] WHERE [IdOferta] = @IdOferta

	COMMIT TRANSACTION
END