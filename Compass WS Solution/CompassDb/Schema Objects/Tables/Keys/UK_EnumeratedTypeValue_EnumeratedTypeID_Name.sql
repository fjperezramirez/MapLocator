ALTER TABLE [dbo].[EnumeratedTypeValue]
	ADD CONSTRAINT [UK_EnumeratedTypeValue_EnumeratedTypeID_Name]
	UNIQUE ([EnumeratedTypeID], [Name])
