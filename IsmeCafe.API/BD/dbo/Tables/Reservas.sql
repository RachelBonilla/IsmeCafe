CREATE TABLE [dbo].[Reservas] (
    [Id]                  UNIQUEIDENTIFIER DEFAULT (NEWID()) NOT NULL,
    [IdServicio]          UNIQUEIDENTIFIER NOT NULL,
    [NombreCliente]       VARCHAR(100)     NOT NULL,
    [Correo]              VARCHAR(150)     NOT NULL,
    [Telefono]            VARCHAR(20)      NOT NULL,
    [FechaReserva]        DATE             NOT NULL,
    [HoraReserva]         TIME             NOT NULL,
    [CantidadPersonas]    INT              NOT NULL
                          CONSTRAINT CK_Reservas_CantidadPersonas CHECK (CantidadPersonas > 0),
    [FechaCreacion]       DATETIME         NOT NULL
                          CONSTRAINT DF_Reservas_FechaCreacion DEFAULT (GETDATE()),

    PRIMARY KEY CLUSTERED ([Id] ASC),

    CONSTRAINT FK_Reservas_Servicios
        FOREIGN KEY ([IdServicio])
        REFERENCES [dbo].[Servicios]([Id])
);