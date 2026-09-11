CREATE TABLE [dbo].[DetallePedidos] (
    [Id]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [IdPedido]        UNIQUEIDENTIFIER NOT NULL,
    [IdProducto]      UNIQUEIDENTIFIER NOT NULL,
    [Cantidad]        INT              NOT NULL,
    [PrecioUnitario]  DECIMAL (18, 2)  NOT NULL,
    [Subtotal]        DECIMAL (18, 2)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DetallePedidos_Pedidos] FOREIGN KEY ([IdPedido]) REFERENCES [dbo].[Pedidos] ([Id]),
    CONSTRAINT [FK_DetallePedidos_Productos] FOREIGN KEY ([IdProducto]) REFERENCES [dbo].[Productos] ([Id])
);
