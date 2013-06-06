CREATE TABLE [dbo].[EnumeratedTypeValue]
(
	[EnumeratedTypeValueID] INT IDENTITY(1,1) NOT NULL,
	[EnumeratedTypeID] INT NOT NULL,
	[Code] INT NOT NULL,
	[Name] VARCHAR(25) NOT NULL,
    [DescriptionEn] VARCHAR(40) NOT NULL,
	[DescriptionEs] VARCHAR(40) NOT NULL
)
