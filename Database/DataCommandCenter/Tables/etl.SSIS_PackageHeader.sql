SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [etl].[SSIS_PackageHeader](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PackagePath] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PackageName] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DTSID] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LastModified] [datetime] NULL
) ON [PRIMARY]

GO
