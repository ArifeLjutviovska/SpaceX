/*
Deployment script for Spacex
This script ensures that the database, schema, and tables are created automatically in Docker.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;
SET NUMERIC_ROUNDABORT OFF;
GO

-- Ensure the database exists before using it
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Spacex')
BEGIN
    CREATE DATABASE [Spacex];
    PRINT 'Database Spacex created successfully.';
END
ELSE
BEGIN
    PRINT 'Database Spacex already exists.';
END
GO

-- Switch to the database
USE [Spacex];
GO

-- Ensure the schema exists before creating tables
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Spacex')
BEGIN
    EXEC('CREATE SCHEMA [Spacex]');
    PRINT 'Schema Spacex created successfully.';
END
ELSE
BEGIN
    PRINT 'Schema Spacex already exists.';
END
GO

-- Ensure the table exists inside the schema
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users' AND schema_id = SCHEMA_ID('Spacex'))
BEGIN
	CREATE TABLE [Spacex].[Users]
	(
		[Id]                       INT IDENTITY (1, 1) PRIMARY KEY NOT NULL, 
		[CreatedOn]                DATETIME2(0)  NOT NULL,
		[DeletedOn]				   DATETIME2(0)    	 NULL, 
		[Email]					   NVARCHAR(75)	 NOT NULL UNIQUE, 
		[FirstName]				   NVARCHAR(100) NOT NULL,
		[LastName]				   NVARCHAR(100) NOT NULL,
		[Password]                 NVARCHAR(50)  NOT NULL 
	);
    PRINT 'Table Spacex.Users created successfully.';
END
ELSE
BEGIN
    PRINT 'Table Spacex.Users already exists.';
END
GO

IF NOT EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'IX_Users_Email' AND object_id = OBJECT_ID('[Spacex].[Users]')
)
BEGIN
    CREATE UNIQUE INDEX IX_Users_Email ON [Spacex].[Users](Email);
    PRINT 'Index IX_Users_Email created successfully.';
END
ELSE
BEGIN
    PRINT 'Index IX_Users_Email already exists.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RefreshTokens' AND schema_id = SCHEMA_ID('Spacex'))
BEGIN
    CREATE TABLE [Spacex].[RefreshTokens] (
        [Id]            INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
        [UserFk]        INT           NOT NULL,
        [Token]         NVARCHAR(255) NOT NULL UNIQUE,
        [ExpiresAt]     DATETIME2(0)  NOT NULL,
        [CreatedAt]     DATETIME2(0)  NOT NULL DEFAULT GETUTCDATE(),
        [RevokedAt]     DATETIME2(0)      NULL,
        CONSTRAINT FK_RefreshTokens_Users FOREIGN KEY (UserFk) REFERENCES [Spacex].[Users](Id) ON DELETE CASCADE
    );
    PRINT 'Table Spacex.RefreshTokens created successfully.';
END
ELSE
BEGIN
    PRINT 'Table Spacex.RefreshTokens already exists.';
END
GO

IF NOT EXISTS (
    SELECT * FROM sys.indexes 
    WHERE name = 'IX_RefreshTokens_UserFk' AND object_id = OBJECT_ID('[Spacex].[RefreshTokens]')
)
BEGIN
    CREATE INDEX IX_RefreshTokens_UserFk ON [Spacex].[RefreshTokens](UserFk);
    PRINT 'Index IX_RefreshTokens_UserFk created successfully.';
END
ELSE
BEGIN
    PRINT 'Index IX_RefreshTokens_UserFk already exists.';
END
GO

PRINT N'Update complete.';
GO
