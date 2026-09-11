CREATE TABLE [dbo].[Usuarios] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [Nombre]             VARCHAR (100)    NOT NULL,
    [Apellidos]          VARCHAR (100)    NOT NULL,
    [Correo]             VARCHAR (100)    NOT NULL,
    [Telefono]           VARCHAR (20)     NOT NULL,
    [Contrasena]         VARCHAR (100)    NOT NULL,
    [IdRol]              UNIQUEIDENTIFIER NOT NULL,
    [Activo]             BIT              DEFAULT ((1)) NOT NULL,
    [FechaCreacion]      DATETIME         DEFAULT (getdate()) NOT NULL,
    [FechaActualizacion] DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Usuarios_Roles] FOREIGN KEY ([IdRol]) REFERENCES [dbo].[Roles] ([Id]),
    UNIQUE NONCLUSTERED ([Correo] ASC)
);

