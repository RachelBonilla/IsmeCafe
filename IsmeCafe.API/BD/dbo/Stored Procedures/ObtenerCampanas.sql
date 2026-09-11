
--Obtener campañas--
CREATE PROCEDURE ObtenerCampanas
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [Tipo], [IdOferta], [IdDescuento], [Asunto], [FechaEnvio], [CantidadDestinatarios], [Exitosa]
	FROM [dbo].[CampanasMarketing]
	ORDER BY [FechaEnvio] DESC
END