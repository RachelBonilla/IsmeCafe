
CREATE PROCEDURE [dbo].[ObtenerCategorias]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ca.[Id],
        ca.[Nombre],
        ca.[Descripcion]
      FROM [dbo].[Categorias] ca
     ORDER BY ca.[Nombre];
END
