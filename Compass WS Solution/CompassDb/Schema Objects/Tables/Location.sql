CREATE TABLE [dbo].[Location]
(
	[LocationID] INT IDENTITY(1,1) NOT NULL,
	[LocationTypeEnumCode] INT NOT NULL,
	[LocationGroupEnumCode] INT NOT NULL,
	[SourceIntUniqueKey] INT NULL,
	[Name] VARCHAR(40) NOT NULL,
    [DescriptionEn] VARCHAR(50) NOT NULL,
	[DescriptionEs] VARCHAR(50) NOT NULL,
	[Address1] VARCHAR(50) NOT NULL,
	[Address2] VARCHAR(50) NOT NULL,
	[City] VARCHAR(30) NOT NULL,
	[State] VARCHAR(30) NOT NULL,
	[ZipCode] VARCHAR(10) NOT NULL,
	[Latitude] FLOAT NULL,
	[Longitude] FLOAT NULL,
	[Z] FLOAT NULL,
	[M] FLOAT NULL,
	[Coordinate]  AS ([geography]::Point([Latitude],[Longitude],(4326))) PERSISTED,
	[CreatedDate] SMALLDATETIME NOT NULL,
	[ModifiedDate] SMALLDATETIME NULL,
	[IncidentDate] SMALLDATETIME NULL,
	[Custom] VARCHAR(512) NULL
)
