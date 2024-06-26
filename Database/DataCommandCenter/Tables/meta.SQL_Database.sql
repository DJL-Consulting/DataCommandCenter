SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [meta].[SQL_Database](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerID] [int] NULL,
	[DatabaseName] [sysname] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Compatability] [tinyint] NOT NULL,
	[Recovery] [nvarchar](60) COLLATE Latin1_General_CI_AS_KS_WS NULL,
	[CreatedDatetime] [datetime] NOT NULL,
	[Collation] [sysname] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Access] [nvarchar](60) COLLATE Latin1_General_CI_AS_KS_WS NULL,
	[ReadOnly] [bit] NULL,
	[DataSizeMB] [numeric](38, 6) NULL,
	[LogSizeMB] [numeric](38, 6) NULL,
	[PullMetadata] [bit] NULL,
	[Description] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HeaderID] [int] NULL
) ON [PRIMARY]

GO
