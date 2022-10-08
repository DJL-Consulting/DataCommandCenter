

insert into LineageFlow ([SourceObjectID], [DestinationObjectID], [IntegrationFlowID], [Operation])
values 
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
