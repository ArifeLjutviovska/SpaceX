CREATE TABLE [Spacex].[User]
(
	[Id]                 INT                NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [CreatedOn]          DATETIME2(0)       NOT NULL,
    [DeletedOn]          DATETIME2(0)       NULL         DEFAULT NULL,
    [Name]               NVARCHAR(100)      NOT NULL
)