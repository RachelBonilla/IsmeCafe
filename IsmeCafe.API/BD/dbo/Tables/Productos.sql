CREATE TABLE [dbo].[Productos] (
    [Id]                  UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [IdCategoria]         UNIQUEIDENTIFIER NULL,
    [Nombre]              VARCHAR (100)    NOT NULL,
    [Descripcion]         VARCHAR (500)    NULL,
    [Precio]              DECIMAL (18, 2)  NOT NULL,
    [Imagen]              VARCHAR (300)    NULL,
    [Cantidad]            INT              NOT NULL,
    [StockMinimo]         INT              CONSTRAINT [DF_Productos_StockMinimo] DEFAULT ((5)) NOT NULL,
    [Activo]              BIT              NOT NULL,
    [Eliminado]           BIT              CONSTRAINT [DF_Productos_Eliminado] DEFAULT ((0)) NOT NULL,
    [FechaCreacion]       DATETIME         NULL,
    [FechaActualizacion]  DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Productos_Categorias] FOREIGN KEY ([IdCategoria]) REFERENCES [dbo].[Categorias] ([Id])
);

