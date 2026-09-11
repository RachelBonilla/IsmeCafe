--Obtener ofertas--
CREATE PROCEDURE ObtenerOfertas
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[Id], [Nombre], [Descripcion], [TipoOferta], [PrecioCombo],
		[FechaInicio], [FechaFin], [Activo], [FechaCreacion], [FechaActualizacion]
	FROM [dbo].[Ofertas]
END