ALTER TABLE [dbo].[Location]
	ADD CONSTRAINT [DF_Location_CreatedDate]
	DEFAULT (getdate())
	FOR [CreatedDate]
