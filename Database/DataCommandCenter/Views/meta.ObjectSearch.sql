SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON


/****** Script for SelectTopNRows command from SSMS  ******/
CREATE VIEW [meta].[ObjectSearch]

as

SELECT 
	'Server' as ObjectType, 
	s.ID, 
	s.ServerName 
		+ case when isnull(s.ServerInstance, '') <> '' THEN '\'+ s.ServerInstance ELSE '' END
		+' '+st.ServerType
		+ ' ' + case when s.Version = '160' THEN '2022'
					 when s.Version = '150' THEN '2019'
					 when s.Version = '140' THEN '2017'
					 when s.Version = '130' THEN '2016'
					 when s.Version = '120' THEN '2014'
					 when s.Version = '110' THEN '2012'
					 when s.Version = '100' THEN '2008'
					 else s.Version END
		+' '+ISNULL(S.Description, '') AS SearchText,
	'Server '+
		+ s.ServerName 
		+ case when isnull(s.ServerInstance, '') <> '' THEN '\'+ s.ServerInstance ELSE '' END
		+' '+st.ServerType
		+ ' ' + case when s.Version = '160' THEN '2022'
					 when s.Version = '150' THEN '2019'
					 when s.Version = '140' THEN '2017'
					 when s.Version = '130' THEN '2016'
					 when s.Version = '120' THEN '2014'
					 when s.Version = '110' THEN '2012'
					 when s.Version = '100' THEN '2008'
					 else s.Version END
		+' '+ISNULL(S.Description, '') AS DisplayText
FROM [DataCommandCenter].[meta].[Server] s
inner join meta.ServerType st
	on s.ServerTypeID = st.ID

UNION ALL

SELECT 
	'Database' as ObjectType, 
	d.ID, 
		d.DatabaseName +' '+ISNULL(d.Description, '') AS SearchText,
	'SQL Database '+
		+ d.DatabaseName
		+ ' on ' + s.ServerName + case when isnull(s.ServerInstance, '') <> '' THEN '\'+ s.ServerInstance ELSE '' END
		+' '+ISNULL(d.Description, '') AS DisplayText

FROM [DataCommandCenter].[meta].SQL_Database d
	inner join meta.Server s
		on s.ID = d.ServerID

UNION ALL

SELECT 
	'SQL ' +
	case when o.ObjectType = 'USER_TABLE' then 'Table'
		 when o.ObjectType = 'SQL_STORED_PROCEDURE' then 'Stored Procedure'
		 when o.ObjectType = 'SQL_SCALAR_FUNCTION' then 'Function'
		 else REPLACE(REPLACE(o.ObjectType, '_', ' '), 'SQL', '') END as ObjectType,
	o.ID, 
	case when isnull(o.SchemaName, '') not in ('dbo', '') then o.SchemaName+'.' else '' END + o.ObjectName 
		+' '+ISNULL(o.Description, '') AS SearchText,

	case when isnull(o.SchemaName, '') <> 'dbo' then o.SchemaName+'.' else '' END + o.ObjectName + ' in '+
		+ d.DatabaseName
		+ ' on ' + s.ServerName + case when isnull(s.ServerInstance, '') <> '' THEN '\'+ s.ServerInstance ELSE '' END
		+' '+ISNULL(o.Description, '') AS DisplayText
FROM [DataCommandCenter].meta.SQL_Object o
	left outer join [meta].SQL_Database d
		on d.ID = o.DatabaseID
	left outer join meta.Server s
		on s.ID = d.ServerID

UNION ALL

SELECT 
	'SQL Column' as ObjectType,
	c.ID, 
	c.ColumnName
		+' '+ISNULL(c.Description, '') AS SearchText,
	c.ColumnName
		+ ' of ' + case when isnull(o.SchemaName, '') <> 'dbo' then o.SchemaName+'.' else '' END + o.ObjectName + ' in '+
		+ d.DatabaseName
		+ ' on ' + s.ServerName + case when isnull(s.ServerInstance, '') <> '' THEN '\'+ s.ServerInstance ELSE '' END
		+' '+ISNULL(c.Description, '') AS DisplayText
FROM [DataCommandCenter].meta.SQL_Column c
	inner join meta.SQL_Object o
		on o.ID = c.ObjectID
	inner join [meta].SQL_Database d
		on d.ID = o.DatabaseID
	inner join meta.Server s
		on s.ID = d.ServerID

UNION ALL

SELECT 
	'Integration' as ObjectType,
	i.ID, 
	i.Name +' '+i.Description AS SearchText,
	i.Name +' '+i.Description AS DisplayText
FROM [DataCommandCenter].etl.Integration i

GO
