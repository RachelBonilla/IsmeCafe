
CREATE PROCEDURE [dbo].[ReporteCatalogo]
    @Activo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        pr.[Id],
        'Producto'                              AS [Tipo],
        pr.[Nombre],
        ISNULL(ca.[Nombre], 'Sin categoria')    AS [Categoria],
        pr.[Precio],
        pr.[Activo],
        pr.[Cantidad]                           AS [Existencias],
        pr.[FechaCreacion]
      FROM [dbo].[Productos] pr
      LEFT JOIN [dbo].[Categorias] ca ON ca.[Id] = pr.[IdCategoria]
     WHERE pr.[Eliminado] = 0
       AND (@Activo IS NULL OR pr.[Activo] = @Activo)

    UNION ALL

    SELECT
        se.[Id],
        'Servicio'                              AS [Tipo],
        se.[Nombre],
        'Servicio'                              AS [Categoria],
        se.[Precio],
        se.[Activo],
        se.[CupoMaximo]                         AS [Existencias],
        se.[FechaCreacion]
      FROM [dbo].[Servicios] se
     WHERE (@Activo IS NULL OR se.[Activo] = @Activo)

     ORDER BY [Tipo], [Categoria], [Nombre];
END
