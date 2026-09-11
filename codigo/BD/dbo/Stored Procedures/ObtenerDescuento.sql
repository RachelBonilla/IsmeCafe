
--Obtener descuento(Por id de producto)--
CREATE PROCEDURE ObtenerDescuento
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		de.[Id], de.[IdProducto], de.[PorcentajeDescuento], de.[FechaInicio], de.[FechaFin],
		de.[Activo], de.[FechaCreacion], de.[FechaActualizacion],
		pr.[Nombre] AS [NombreProducto], pr.[Precio] AS [PrecioProducto]
	FROM [dbo].[Descuentos] de
	INNER JOIN [dbo].[Productos] pr ON pr.[Id] = de.[IdProducto]
	WHERE de.[Id] = @Id
END