import {Component, Input, OnInit} from '@angular/core';
import {Page} from "../_models/Page";
import {BlockType} from "../_models/BlockType";
import {BlockService} from "../_services/block.service";
import {Block} from "../_models/Block";

@Component({
  selector: 'page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.css']
})
export class PageComponent implements OnInit
{

  @Input() Page: Page;
  Types = BlockType;

  constructor (private blockService: BlockService)
  {
  }

  ngOnInit ()
  {
    console.log("Page loaded", this.Page);
  }

  checkPress ($event: KeyboardEvent)
  {
    return $event.which != 13;
  }

  SendBlock (blockContentInput: string, blockTypeInput: string)
  {
    console.log("Sending block",blockContentInput,blockTypeInput);

    let block: Block = {
      Type: BlockType[blockTypeInput],
      Content: blockContentInput
    };

    this.blockService.CreateBlock(block,this.Page).subscribe();
  }
}
