CREATE TABLE [Spacex].[RefreshTokens] (
    [Id]            INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    [UserFk]        INT NOT NULL,
    [Token]         NVARCHAR(255) NOT NULL UNIQUE,
    [ExpiresAt]     DATETIME2(0) NOT NULL,
    [CreatedAt]     DATETIME2(0) NOT NULL DEFAULT GETUTCDATE(),
    [RevokedAt]     DATETIME2(0) NULL,
    CONSTRAINT FK_RefreshTokens_Users FOREIGN KEY (UserFk) REFERENCES [Spacex].[Users](Id) ON DELETE CASCADE
);

GO
CREATE INDEX IX_RefreshTokens_UserFk ON [Spacex].[RefreshTokens](UserFk);
