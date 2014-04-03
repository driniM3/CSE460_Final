CREATE TABLE [dbo].[UserProfile] (
    [UserId]   INT            IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (20) UNIQUE NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

