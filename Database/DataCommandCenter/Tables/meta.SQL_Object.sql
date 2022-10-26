SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [meta].[SQL_Object](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DatabaseID] [int] NULL,
	[SchemaName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ObjectName] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ObjectType] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Rows] [int] NULL,
	[SizeMB] [float] NULL,
	[ObjectDefinition] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Description] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HeaderID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
