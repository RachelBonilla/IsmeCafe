CREATE TABLE [dbo].[Pedidos] (
    [Id]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [NumeroPedido]    INT              IDENTITY (1000, 1) NOT NULL,
    [IdUsuario]       UNIQUEIDENTIFIER NOT NULL,
    [IdEmpleado]      UNIQUEIDENTIFIER NULL,
    [Fecha]           DATETIME         DEFAULT (getdate()) NOT NULL,
    [Total]           DECIMAL (18, 2)  NOT NULL,
    [IdEstado]        INT              NOT NULL,
    [PuntosGanados]   INT              DEFAULT ((0)) NOT NULL,
    [PuntosUsados]    INT              DEFAULT ((0)) NOT NULL,
    [DescuentoPuntos] DECIMAL (18, 2)  DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Pedidos_Empleado] FOREIGN KEY ([IdEmpleado]) REFERENCES [dbo].[Usuarios] ([Id]),
    CONSTRAINT [FK_Pedidos_PedidosEstados] FOREIGN KEY ([IdEstado]) REFERENCES [dbo].[PedidosEstados] ([IdEstado]),
    CONSTRAINT [FK_Pedidos_Usuarios] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuarios] ([Id]),
    CONSTRAINT [UQ_Pedidos_NumeroPedido] UNIQUE NONCLUSTERED ([NumeroPedido] ASC)
);

