CREATE TABLE [dbo].[Productos] (
    [Id]                  UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [IdCategoria]         UNIQUEIDENTIFIER NULL,
    [Nombre]              VARCHAR (100)    NOT NULL,
    [Descripcion]         VARCHAR (500)    NULL,
    [Precio]              DECIMAL (18, 2)  NOT NULL,
    [Imagen]              VARCHAR (300)    NULL,
    [Cantidad]            INT              NOT NULL,
    [Activo]              BIT              NOT NULL,
    [FechaCreacion]       DATETIME         NULL,
    [FechaActualizacion]  DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IdCategoria]) REFERENCES [dbo].[Categorias] ([Id])
);
