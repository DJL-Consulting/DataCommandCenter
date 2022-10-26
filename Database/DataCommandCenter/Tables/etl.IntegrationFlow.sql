SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [etl].[IntegrationFlow](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IntegrationID] [int] NOT NULL,
	[SourceQuery] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Description] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
