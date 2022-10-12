export interface Header {
  id: number;
}

export interface Property {
  id: number;
  headerId: number;
  key: string | null;
  value: string | null;
}

export interface ServerDTO {
  id: number;
  serverName: string | null;
  serverInstance: string | null;
  version: string | null;
  pullMetadata: boolean | null;
  serverType: string | null;
  metadataDictionary: Property[] | null;
}

export interface DatabaseDTO {
  id: number;
  serverId: number | null;
  serverName: string | null;
  databaseName: string | null;
  compatability: string;
  recovery: string | null;
  createdDatetime: string;
  collation: string | null;
  access: string | null;
  readOnly: boolean | null;
  dataSizeMb: number | null;
  logSizeMb: number | null;
  pullMetadata: boolean | null;
  description: string | null;
  metadataDictionary: Property[] | null;
}

export interface ObjectDTO {
  id: number;
  serverName: string | null;
  databaseName: string | null;
  databaseId: number | null;
  schemaName: string | null;
  objectName: string | null;
  objectType: string | null;
  rows: number | null;
  sizeMb: number | null;
  objectDefinition: string | null;
  description: string | null;
  metadataDictionary: Property[] | null;
}

export interface ColumnDTO {
  id: number;
  objectId: number | null;
  serverName: string | null;
  databaseName: string | null;
  objectName: string | null;
  columnName: string | null;
  dataType: string | null;
  maxLength: number | null;
  precision: number | null;
  scale: number | null;
  nullable: boolean | null;
  primaryKey: boolean | null;
  ordinalPosition: number | null;
  description: string | null;
  metadataDictionary: Property[] | null;
}

export interface MetadataDTO {
  servers: ServerDTO[];
  databases: DatabaseDTO[];
  objects: ObjectDTO[];
  columns: ColumnDTO[];
}

export interface ObjectSearch {
  objectType: string;
  id: number;
  searchText: string;
  displayText: string;
}

export interface SearchObjectTypes {
  SearchType: string;
  QueryString: string;
  Servers: boolean;
  Databases: boolean;
  Tables: boolean;
  Views: boolean;
  ProgrammableObjects: boolean;
  Columns: boolean;
  Integrations: boolean;
}

