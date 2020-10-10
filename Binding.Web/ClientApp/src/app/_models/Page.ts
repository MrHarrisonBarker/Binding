import {User} from "./User";
import {Block, BlockViewModel} from "./Block";

export interface Page
{
  Id : string;
  Name : string;
  Created : Date;
  Updated : Date;
  Order : number;
  Owner : User;
  Blocks? : Block[];
  Children? : Page[];
  Parent? : Page;
}

export interface  PageWithNoBlocksViewModel
{
  Id : string;
  Name : string;
  Created : Date;
  Updated : Date;
  Order : number;
  Children : PageWithNoBlocksViewModel[];
}

export interface PageWithBlocksViewModel {
  Id : string;
  Name : string;
  Created : Date;
  Updated : Date;
  Order : number;
  Children : PageWithNoBlocksViewModel[];
  Blocks : BlockViewModel[];
  Parent : PageWithNoBlocksViewModel;
}
