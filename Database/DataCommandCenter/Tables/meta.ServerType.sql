SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [meta].[ServerType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerType] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Description] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]

GO
