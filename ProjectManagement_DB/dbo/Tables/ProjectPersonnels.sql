CREATE TABLE [dbo].[ProjectPersonnels]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [ProjectId] INT NOT NULL REFERENCES Projects(Id), 
    [PersonnelId] NVARCHAR(20) NOT NULL REFERENCES Personnel(Id)
)
