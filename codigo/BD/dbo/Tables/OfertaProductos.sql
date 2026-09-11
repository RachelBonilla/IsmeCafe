CREATE TABLE [dbo].[OfertaProductos] (
    [Id]         UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [IdOferta]   UNIQUEIDENTIFIER NOT NULL,
    [IdProducto] UNIQUEIDENTIFIER NOT NULL,
    [Cantidad]   INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IdOferta]) REFERENCES [dbo].[Ofertas] ([Id]),
    FOREIGN KEY ([IdProducto]) REFERENCES [dbo].[Productos] ([Id])
);

