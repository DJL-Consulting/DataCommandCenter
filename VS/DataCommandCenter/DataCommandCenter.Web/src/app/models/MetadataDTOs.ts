export interface ServerDTO {
  id: number;
  serverName: string;
  serverInstance: string;
  version: string;
  pullMetadata: boolean;
  serverType: string;
}

export interface ObjectSearch {
  objectType: string;
  id: number;
  searchText: string;
  displayText: string;
}

export interface SearchObjectTypes {
  QueryString: string;
  Servers: boolean;
  Databases: boolean;
  Tables: boolean;
  Views: boolean;
  ProgrammableObjects: boolean;
  Columns: boolean;
}
