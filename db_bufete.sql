/*
Script de implementación para db_blank

Una herramienta generó este código.
Los cambios realizados en este archivo podrían generar un comportamiento incorrecto y se perderán si
se vuelve a generar el código.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "db_blank"
:setvar DefaultFilePrefix "db_blank"
:setvar DefaultDataPath "C:\Users\Usuario\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"
:setvar DefaultLogPath "C:\Users\Usuario\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"

GO
:on error exit
GO
/*
Detectar el modo SQLCMD y deshabilitar la ejecución del script si no se admite el modo SQLCMD.
Para volver a habilitar el script después de habilitar el modo SQLCMD, ejecute lo siguiente:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'El modo SQLCMD debe estar habilitado para ejecutar correctamente este script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Creando Tabla [dbo].[__EFMigrationsHistory]...';


GO
CREATE TABLE [dbo].[__EFMigrationsHistory] (
    [MigrationId]    NVARCHAR (150) NOT NULL,
    [ProductVersion] NVARCHAR (32)  NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[Cita]...';


GO
CREATE TABLE [dbo].[Cita] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Id_cliente]      INT            NOT NULL,
    [Especialidad]    NVARCHAR (MAX) NOT NULL,
    [Hora]            NVARCHAR (MAX) NOT NULL,
    [Fecha]           DATETIME2 (7)  NOT NULL,
    [Nombre_cliente]  NVARCHAR (MAX) NOT NULL,
    [Fecha_solicitud] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Cita] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Tabla [dbo].[Cliente]...';


GO
CREATE TABLE [dbo].[Cliente] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Nombre_completo]  NVARCHAR (MAX) NOT NULL,
    [Identificacion]   NVARCHAR (MAX) NOT NULL,
    [Medio_pago]       NVARCHAR (MAX) NOT NULL,
    [Fecha_nacimiento] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[Cita]...';


GO
ALTER TABLE [dbo].[Cita]
    ADD DEFAULT ('0001-01-01T00:00:00.0000000') FOR [Fecha];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[Cita]...';


GO
ALTER TABLE [dbo].[Cita]
    ADD DEFAULT (N'') FOR [Nombre_cliente];


GO
PRINT N'Creando Restricción DEFAULT restricción sin nombre en [dbo].[Cita]...';


GO
ALTER TABLE [dbo].[Cita]
    ADD DEFAULT ('0001-01-01T00:00:00.0000000') FOR [Fecha_solicitud];


GO
PRINT N'Actualización completada.';


GO
