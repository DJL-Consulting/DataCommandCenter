
insert into meta.SQL_Object ([DatabaseID], [SchemaName], [ObjectName], [ObjectType], [Description])
	values (-1, '', 'MarketingLeads.csv', 'File', 'Marketing Lead File')
select SCOPE_IDENTITY()

insert into meta.SQL_Object ([DatabaseID], [SchemaName], [ObjectName], [ObjectType], [Description])
	values (-1, '', 'common.log', 'File', 'Web server log file')
select SCOPE_IDENTITY()


insert into LineageFlow ([SourceObjectID], [DestinationObjectID], [IntegrationFlowID], [Operation])
values 
( (select ID from   [meta].[ObjectSearch] where [objectType] = 'SQL File' and SearchText = 'MarketingLeads.csv Marketing Lead File '),
  (select ID from   [meta].[ObjectSearch] where [objectType] = 'sql table' and DisplayText = 'stg.Marketing_Lead in B2B_Platform on XPS17 '),
  1,
  'Integration'
),

( (select ID from   [meta].[ObjectSearch] where [objectType] = 'SQL File' and SearchText = 'common.log Web server log file '),
  (select ID from   [meta].[ObjectSearch] where [objectType] = 'sql table' and DisplayText = 'etl.Web_Log in B2B_Datamart on XPS17 '),
  1,
  'Integration'
),

( (select ID from   [meta].[ObjectSearch] where [objectType] = 'sql table' and DisplayText = 'Company in B2B_Platform on XPS17 '),
  (select ID from   [meta].[ObjectSearch] where [objectType] = 'sql table' and DisplayText = 'etl.Company in B2B_Datamart on XPS17'),
  1,
  'Integration'
),

( (select ID from   [meta].[ObjectSearch] where [objectType] = 'sql table' and DisplayText = 'Customer in B2B_Platform on XPS17 '),
  (select ID from   [meta].[ObjectSearch] where [objectType] = 'sql table' and DisplayText = 'etl.Customer in B2B_Datamart on XPS17'),
  1,
  'Integration'
),

( (select ID from   [meta].[ObjectSearch] where [objectType] = 'sql table' and DisplayText = 'Order_Header in B2B_Platform on XPS17 '),
  (select ID from   [meta].[ObjectSearch] where [objectType] = 'sql table' and DisplayText = 'etl.Order in B2B_Datamart on XPS17'),
  1,
  'Integration'
),

( (select ID from   [meta].[ObjectSearch] where [objectType] = 'sql table' and DisplayText = 'Order_Line in B2B_Platform on XPS17 '),
  (select ID from   [meta].[ObjectSearch] where [objectType] = 'sql table' and DisplayText = 'etl.Order in B2B_Datamart on XPS17'),
  1,
  'Integration'
),

( (select ID from   [meta].[ObjectSearch] where [objectType] = 'sql table' and DisplayText = 'Product in B2B_Platform on XPS17 '),
  (select ID from   [meta].[ObjectSearch] where [objectType] = 'sql table' and DisplayText = 'etl.Product in B2B_Datamart on XPS17'),
  1,
  'Integration'
)
