SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [meta].[SQL_DatabaseHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DatabaseID] [int] NOT NULL,
	[CheckDatetime] [datetime] NOT NULL,
	[DataSizeMB] [numeric](38, 6) NULL,
	[LogSizeMB] [numeric](38, 6) NULL
) ON [PRIMARY]

GO
