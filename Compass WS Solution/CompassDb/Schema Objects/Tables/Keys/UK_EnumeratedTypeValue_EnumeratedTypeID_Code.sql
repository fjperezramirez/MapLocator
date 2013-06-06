ALTER TABLE [dbo].[EnumeratedTypeValue]
	ADD CONSTRAINT [UK_EnumeratedTypeValue_EnumeratedTypeID_Code]
	UNIQUE ([EnumeratedTypeID], [Code])
