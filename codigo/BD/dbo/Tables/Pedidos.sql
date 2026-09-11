CREATE TABLE [dbo].[Pedidos] (
    [Id]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [NumeroPedido]    INT              IDENTITY (1000, 1) NOT NULL,
    [IdUsuario]       UNIQUEIDENTIFIER NOT NULL,
    [Fecha]           DATETIME         DEFAULT (getdate()) NOT NULL,
    [Estado]          VARCHAR (30)     DEFAULT ('Pendiente') NOT NULL,
    [Total]           DECIMAL (18, 2)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Pedidos_NumeroPedido] UNIQUE NONCLUSTERED ([NumeroPedido] ASC),
    CONSTRAINT [FK_Pedidos_Usuarios] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuarios] ([Id])
);
