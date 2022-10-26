SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[LineageFlow](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SourceObjectID] [int] NULL,
	[DestinationObjectID] [int] NULL,
	[IntegrationFlowID] [int] NULL,
	[Operation] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]

GO
