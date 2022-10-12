import { PropertyDTO } from "./PropertyDTO";
import { Property } from "./MetadataDTOs"

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
  metadataDictionary: Property[] | null;
  level: number | null;
}

export interface IntegrationDTO {
  id: number;
  name: string | null;
  integrationType: string | null;
  path: string | null;
  description: string | null;
  created: string | null;
  lastModified: string | null;
  metadataDictionary: Property[] | null;
}
export interface LineageLink {
  id: number;
  sourceObjectId: number | null;
  destinationObjectId: number | null;
  integrationFlowId: number | null;
  integration: IntegrationDTO;
  integrationInfo: string;
  operation: string | null;
}

export interface LineageDTO {
  nodes: LineageNode[];
  flows: LineageLink[];
}
