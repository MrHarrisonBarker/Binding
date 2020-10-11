import {BlockType} from "./BlockType";
import {Page} from "./Page";

export interface Block
{
  Id?: string;
  Type: BlockType;
  Content: string;
  Created?: Date;
  Updated?: Date;
  Order?: number;
  Page?: Page;
}

export interface BlockViewModel
{
  Id: string;
  Type: BlockType;
  Content: string;
  Created: Date;
  Updated: Date;
  Order: number;
}
