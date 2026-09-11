
--Obtener ofertas activas--
CREATE PROCEDURE ObtenerOfertasActivas
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[Id], [Nombre], [Descripcion], [TipoOferta], [PrecioCombo],
		[FechaInicio], [FechaFin], [Activo], [FechaCreacion], [FechaActualizacion]
	FROM [dbo].[Ofertas]
	WHERE [Activo] = 1 AND GETDATE() BETWEEN [FechaInicio] AND [FechaFin]
END