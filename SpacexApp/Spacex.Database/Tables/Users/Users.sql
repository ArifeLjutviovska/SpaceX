CREATE TABLE [Spacex].[Users]
(
    [Id]                       INT IDENTITY (1, 1) PRIMARY KEY NOT NULL, 
    [CreatedOn]                DATETIME2(0)                    NOT NULL,
    [DeletedOn]				   DATETIME2(0)						   NULL, 
    [Email]					   NVARCHAR(75)					   NOT NULL UNIQUE, 
    [FirstName]				   NVARCHAR(150)			       NOT NULL,
    [LastName]				   NVARCHAR(150)                   NOT NULL,
    [Password]                 NVARCHAR(150)                   NOT NULL 
);

GO
CREATE INDEX IX_Users_Email ON [Spacex].[Users](Email);