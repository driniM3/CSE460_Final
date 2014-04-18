﻿CREATE TABLE [dbo].[Tenants]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) UNIQUE NOT NULL, 
    [Style] VARCHAR(50) NULL, 
    [Logo] VARCHAR(MAX) NULL
)
