
USE IsmeCafe
GO

IF OBJECT_ID('Productos', 'U') IS NOT NULL DROP TABLE Productos;
IF OBJECT_ID('Categorias', 'U') IS NOT NULL DROP TABLE Categorias;
GO

CREATE TABLE Categorias (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200) NULL
);
GO

CREATE TABLE Productos (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IdCategoria UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Categorias(Id),
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(500) NULL,
    Precio DECIMAL(18,2) NOT NULL,
    Imagen VARCHAR(300) NULL,
    Cantidad INT NOT NULL,
    Activo BIT NOT NULL,
    FechaCreacion DATETIME NULL,
    FechaActualizacion DATETIME NULL
);
GO

-- ============================================================
--  PROCEDIMIENTOS ALMACENADOS
-- ============================================================
CREATE OR ALTER PROCEDURE ObtenerProductos
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		pr.[Id], pr.[Nombre], pr.[Descripcion], pr.[Precio], pr.[Imagen],
		pr.[Cantidad], pr.[Activo], pr.[FechaCreacion], pr.[FechaActualizacion],
		ca.[Nombre] AS [Categoria]
	FROM [dbo].[Productos] pr
	LEFT JOIN [dbo].[Categorias] ca ON ca.[Id] = pr.[IdCategoria]
END
GO

CREATE OR ALTER PROCEDURE ObtenerProducto
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		pr.[Id], pr.[IdCategoria], pr.[Nombre], pr.[Descripcion], pr.[Precio], pr.[Imagen],
		pr.[Cantidad], pr.[Activo], pr.[FechaCreacion], pr.[FechaActualizacion],
		ca.[Nombre] AS [Categoria]
	FROM [dbo].[Productos] pr
	LEFT JOIN [dbo].[Categorias] ca ON ca.[Id] = pr.[IdCategoria]
	WHERE pr.[Id] = @Id
END
GO

CREATE OR ALTER PROCEDURE ObtenerProductosActivos
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		pr.[Id], pr.[Nombre], pr.[Descripcion], pr.[Precio], pr.[Imagen],
		pr.[Cantidad], pr.[Activo], pr.[FechaCreacion], pr.[FechaActualizacion],
		ca.[Nombre] AS [Categoria]
	FROM [dbo].[Productos] pr
	LEFT JOIN [dbo].[Categorias] ca ON ca.[Id] = pr.[IdCategoria]
	WHERE pr.[Activo] = 1
END
GO

CREATE OR ALTER PROCEDURE AgregarProducto
	@Id UNIQUEIDENTIFIER, @IdCategoria UNIQUEIDENTIFIER, @Nombre VARCHAR(100),
	@Descripcion VARCHAR(500), @Precio DECIMAL(18,2), @Imagen VARCHAR(300),
	@Cantidad INT, @Activo BIT, @FechaCreacion DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	INSERT INTO [dbo].[Productos]
		([Id], [IdCategoria], [Nombre], [Descripcion], [Precio], [Imagen], [Cantidad], [Activo], [FechaCreacion])
	VALUES
		(@Id, @IdCategoria, @Nombre, @Descripcion, @Precio, @Imagen, @Cantidad, @Activo, @FechaCreacion)
	SELECT @Id
	COMMIT TRANSACTION
END
GO

CREATE OR ALTER PROCEDURE EditarProducto
	@Id UNIQUEIDENTIFIER, @IdCategoria UNIQUEIDENTIFIER, @Nombre VARCHAR(100),
	@Descripcion VARCHAR(500), @Precio DECIMAL(18,2), @Imagen VARCHAR(300),
	@Cantidad INT, @Activo BIT, @FechaActualizacion DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	UPDATE [dbo].[Productos]
	SET [IdCategoria] = @IdCategoria, [Nombre] = @Nombre, [Descripcion] = @Descripcion,
		[Precio] = @Precio, [Imagen] = @Imagen, [Cantidad] = @Cantidad,
		[Activo] = @Activo, [FechaActualizacion] = @FechaActualizacion
	WHERE (Id = @Id)
	SELECT @Id
	COMMIT TRANSACTION
END
GO

CREATE OR ALTER PROCEDURE EliminarProducto
	@Id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION
	UPDATE [dbo].[Productos]
	SET [Activo] = 0, [FechaActualizacion] = GETDATE()
	WHERE (Id = @Id)
	SELECT @Id
	COMMIT TRANSACTION
END
GO

-- ============================================================
--  DATOS DE EJEMPLO
-- ============================================================
SET NOCOUNT ON;
GO

-- Categorias con identificadores fijos para referenciarlas desde Swagger / la API.
INSERT INTO Categorias (Id, Nombre, Descripcion) VALUES
('B0000000-0000-0000-0000-000000000001', 'Café en grano', 'Café tostado en grano y molido'),
('B0000000-0000-0000-0000-000000000002', 'Bebidas', 'Bebidas calientes y frías'),
('B0000000-0000-0000-0000-000000000003', 'Repostería', 'Pasteles, galletas y postres'),
('B0000000-0000-0000-0000-000000000004', 'Merchandising', 'Tazas, termos y accesorios');
GO

-- Producto Nº1 con Id fijo para usarlo como ejemplo por defecto en Swagger.
INSERT INTO Productos (Id, IdCategoria, Nombre, Descripcion, Precio, Imagen, Cantidad, Activo, FechaCreacion) VALUES
('C0000001-0000-0000-0000-000000000001', 'B0000000-0000-0000-0000-000000000001', 'Café de altura 250g', 'Café molido de tueste medio, notas a chocolate y cítricos.', 4500.00, 'https://images.unsplash.com/photo-1642437271884-fe1d3393eef9?auto=format&fit=crop&w=800&q=80', 25, 1, GETDATE()),
(NEWID(), 'B0000000-0000-0000-0000-000000000001', 'Espresso premium 500g', 'Mezcla intensa ideal para espresso.', 8200.00, 'https://images.unsplash.com/photo-1544486864-3087e2e20d91?auto=format&fit=crop&w=800&q=80', 15, 1, GETDATE()),
(NEWID(), 'B0000000-0000-0000-0000-000000000002', 'Latte de la casa', 'Latte caliente con leche vaporizada.', 2200.00, 'https://images.unsplash.com/photo-1559001724-fbad036dbc9e?auto=format&fit=crop&w=800&q=80', 50, 1, GETDATE()),
(NEWID(), 'B0000000-0000-0000-0000-000000000003', 'Cheesecake de frutos rojos', 'Porción individual de cheesecake.', 3100.00, 'https://images.unsplash.com/photo-1533134242443-d4fd215305ad?auto=format&fit=crop&w=800&q=80', 12, 1, GETDATE()),
(NEWID(), 'B0000000-0000-0000-0000-000000000004', 'Taza Isme Café', 'Taza de cerámica con logo del café.', 5000.00, 'https://images.unsplash.com/photo-1630595478342-5b257b95f783?auto=format&fit=crop&w=800&q=80', 0, 0, GETDATE());
GO

-- Verificación rápida
SELECT 'Categorias' AS Tabla, COUNT(*) AS Total FROM Categorias
UNION ALL SELECT 'Productos', COUNT(*) FROM Productos;
GO

-- Identificadores para usar en la API (POST/PUT api/Producto)
SELECT Id, Nombre FROM Categorias ORDER BY Nombre;
GO
