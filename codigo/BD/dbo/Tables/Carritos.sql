CREATE TABLE [dbo].[Carritos] (
    [Id]                UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [IdUsuario]         UNIQUEIDENTIFIER NOT NULL,
    [IdProducto]        UNIQUEIDENTIFIER NOT NULL,
    [Cantidad]          INT              NOT NULL,
    [FechaCreacion]     DATETIME         DEFAULT (getdate()) NOT NULL,
    [FechaActualizacion] DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Carritos_Usuarios] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuarios] ([Id]),
    CONSTRAINT [FK_Carritos_Productos] FOREIGN KEY ([IdProducto]) REFERENCES [dbo].[Productos] ([Id]),
    CONSTRAINT [UQ_Carritos_UsuarioProducto] UNIQUE NONCLUSTERED ([IdUsuario] ASC, [IdProducto] ASC)
);
