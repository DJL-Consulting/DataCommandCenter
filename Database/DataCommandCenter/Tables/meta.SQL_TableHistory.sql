SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [meta].[SQL_TableHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectID] [int] NULL,
	[CheckDatetime] [datetime] NULL,
	[Rows] [int] NULL,
	[SizeMB] [float] NULL
) ON [PRIMARY]

GO
