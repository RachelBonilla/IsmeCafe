CREATE TABLE [dbo].[PedidosEstados] (
    [IdEstado] INT          IDENTITY (1, 1) NOT NULL,
    [Nombre]   VARCHAR (30) NOT NULL,
    [Activo]   BIT          DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([IdEstado] ASC)
);

