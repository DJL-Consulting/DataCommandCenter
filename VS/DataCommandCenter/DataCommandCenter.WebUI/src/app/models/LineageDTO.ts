import { PropertyDTO } from "./PropertyDTO";

export interface LineageNode {
  id: number;
  title: string;
  schemaName: string | null;
  objectName: string | null;
  objectType: string | null;
  rows: number | null;
  sizeMb: number | null;
  objectDefinition: string | null;
  description: string | null;
  properties: PropertyDTO[];
  level: number | null;
}

export interface LineageLink {
  id: number;
  sourceObjectId: number | null;
  destinationObjectId: number | null;
  integrationFlowId: number | null;
  integrationInfo: string;
  operation: string | null;
}

export interface LineageDTO {
  nodes: LineageNode[];
  flows: LineageLink[];
}
