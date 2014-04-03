﻿CREATE TABLE [dbo].[Requirements]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(100) NOT NULL, 
    [Description] VARCHAR(MAX) NULL, 
    [Type] VARCHAR(50) NULL, 
    [Time] FLOAT NULL, 
    [Status] VARCHAR(50) NULL,
    [Assignee] NVARCHAR(20) NULL REFERENCES PERSONNEL(Id), 
)
