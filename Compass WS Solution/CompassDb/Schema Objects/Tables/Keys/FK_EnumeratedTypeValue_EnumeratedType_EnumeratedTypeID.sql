ALTER TABLE [dbo].[EnumeratedTypeValue]
	ADD CONSTRAINT [FK_EnumeratedTypeValue_EnumeratedType_EnumeratedTypeID]
	FOREIGN KEY (EnumeratedTypeID)
	REFERENCES [dbo].[EnumeratedType] (EnumeratedTypeID)
