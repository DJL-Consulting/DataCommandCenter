SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [etl].[Integration](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IntegrationType] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Path] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Description] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Created] [datetime] NULL,
	[LastModified] [datetime] NULL
) ON [PRIMARY]

GO
