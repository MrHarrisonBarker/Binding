import {Component, Input, OnInit} from '@angular/core';
import {Page} from "../_models/Page";
import {BlockType} from "../_models/BlockType";
import {BlockService} from "../_services/block.service";
import {Block} from "../_models/Block";
import {CdkDragDrop} from "@angular/cdk/drag-drop";

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

  drop (event: CdkDragDrop<any>)
  {
    // console.log(event, this.Page.Blocks[event.previousIndex].Content, this.Page.Blocks[event.currentIndex].Content);
    if (event.previousIndex != event.currentIndex) {
      this.blockService.ReOrder(this.Page.Blocks[event.previousIndex].Id, this.Page.Blocks[event.currentIndex].Id, this.Page).subscribe(reOrdered =>
      {
        console.log("reordered?");
      });
    }
  }
}
