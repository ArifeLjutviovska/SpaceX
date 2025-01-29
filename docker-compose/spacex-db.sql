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
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'User' AND schema_id = SCHEMA_ID('Spacex'))
BEGIN
    CREATE TABLE [Spacex].[User]
    (
		[Id]                 INT                NOT NULL PRIMARY KEY IDENTITY(1, 1), 
		[CreatedOn]          DATETIME2(0)       NOT NULL,
		[DeletedOn]          DATETIME2(0)       NULL         DEFAULT NULL,
		[Name]               NVARCHAR(100)      NOT NULL
    );
    PRINT 'Table User created successfully.';
END
ELSE
BEGIN
    PRINT 'Table User already exists.';
END
GO

PRINT N'Update complete.';
GO
