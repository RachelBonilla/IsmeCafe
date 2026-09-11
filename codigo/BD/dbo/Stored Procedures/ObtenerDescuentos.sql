
--Obtener descuentos--
CREATE PROCEDURE ObtenerDescuentos
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		de.[Id], de.[IdProducto], de.[PorcentajeDescuento], de.[FechaInicio], de.[FechaFin],
		de.[Activo], de.[FechaCreacion], de.[FechaActualizacion],
		pr.[Nombre] AS [NombreProducto], pr.[Precio] AS [PrecioProducto]
	FROM [dbo].[Descuentos] de
	INNER JOIN [dbo].[Productos] pr ON pr.[Id] = de.[IdProducto]
END