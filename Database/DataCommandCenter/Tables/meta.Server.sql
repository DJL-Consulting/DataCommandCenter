SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [meta].[Server](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerTypeID] [int] NULL,
	[ServerName] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ServerInstance] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Version] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PullMetadata] [bit] NULL,
	[Description] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HeaderID] [int] NULL
) ON [PRIMARY]

GO
