CREATE TABLE [dbo].[Parameter]
(
	[ParameterID] INT IDENTITY(1,1) NOT NULL,
	[LocationID] INT NOT NULL,
	[Name] VARCHAR(30) NOT NULL,
    [Value] VARCHAR(1024) NOT NULL
)
