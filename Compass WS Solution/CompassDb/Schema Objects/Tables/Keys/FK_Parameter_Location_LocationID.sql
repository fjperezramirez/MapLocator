ALTER TABLE [dbo].[Parameter]
	ADD CONSTRAINT [FK_Parameter_Location_LocationID]
	FOREIGN KEY (LocationID)
	REFERENCES [dbo].[Location] (LocationID)
