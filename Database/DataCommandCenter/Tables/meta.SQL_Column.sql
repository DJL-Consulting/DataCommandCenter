SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [meta].[SQL_Column](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectID] [int] NULL,
	[ColumnName] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DataType] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MaxLength] [int] NULL,
	[Precision] [int] NULL,
	[Scale] [int] NULL,
	[Nullable] [bit] NULL,
	[PrimaryKey] [bit] NULL,
	[OrdinalPosition] [int] NULL,
	[Description] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[HeaderID] [int] NULL
) ON [PRIMARY]

GO
