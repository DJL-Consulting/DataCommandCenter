SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON




CREATE VIEW [meta].[SQL_ObjectMetadata]

as

select 
	s.ID as ServerID,
	db.ID as DatabaseID,
	obj.ID as ObjectID,
	s.ServerName,
	isnull(s.ServerInstance, '') as ServerInstance,
	s.Version,
	st.ServerType,
	db.DatabaseName,
	db.Recovery,
	db.CreatedDatetime,
	db.Collation,
	db.Access,
	db.ReadOnly,
	db.DataSizeMB,
	db.LogSizeMB,
	obj.SchemaName,
	obj.ObjectName,
	obj.ObjectType,
	isnull(obj.Rows, -1) as [RowCount],
	isnull(obj.SizeMB, 0) as SizeMB,
	isnull(obj.ObjectDefinition, '') as ObjectDefinition
--select *
from meta.Server s
inner join meta.ServerType st
	on s.ServerTypeID = st.ID
inner join meta.SQL_Database db
	on s.ID = db.ServerID
inner join meta.SQL_Object obj
	on db.ID = obj.DatabaseID
--order by S.ServerName, db.DatabaseName, obj.ObjectName, col.OrdinalPosition

GO
