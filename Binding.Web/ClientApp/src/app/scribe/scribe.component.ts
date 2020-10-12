import {Component, Input, OnInit} from '@angular/core';
import {BlockType} from "../_models/BlockType";
import {Block} from "../_models/Block";
import {BlockService} from "../_services/block.service";
import {Page} from "../_models/Page";

@Component({
  selector: 'scribe',
  templateUrl: './scribe.component.html',
  styleUrls: ['./scribe.component.css']
})
export class ScribeComponent implements OnInit
{
  @Input() Page: Page;
  public SearchedTypes: string[];
  private Types: string[] = Object.keys(BlockType).filter(e => !isNaN(+e)).map(o =>
  {
    return '/' + BlockType[o];
  });
  public SelectedType: string;
  public EditMode: boolean = false;
  ContentInput: string;

  constructor (private blockService: BlockService)
  {
  }

  ngOnInit ()
  {
  }

  public Backspace (event: any, blockContentInput: string)
  {
    this.SearchTypes(event,blockContentInput);
    this.EditModeOff(blockContentInput);
  }


  SendBlock (event: any, blockContentInput: string)
  {
    console.log("Sending block", blockContentInput, this.SelectedType);

    let block: Block = {
      Type: BlockType[this.SelectedType],
      Content: blockContentInput
    };
    event.srcElement.innerText = "";
    this.blockService.Create(block, this.Page).subscribe();
  }

  SearchTypes (event: KeyboardEvent, blockContentInput: string)
  {
    if (!this.EditMode)
    {
      console.log("Searching", blockContentInput.length);
      if (blockContentInput.length < 1)
      {
        this.SearchedTypes = this.Types;
      } else
      {
        this.SearchedTypes = this.Types.filter(x => x.toLowerCase().includes(blockContentInput.toLowerCase()));
        if (this.SearchedTypes.length == 0)
        {
          this.EditMode = true;
          this.SelectedType = 'Text';
        }
      }

      this.SelectedType = this.SearchedTypes[0];
    }
    return event.which != 13;
  }

  public Enter (event: any, blockContentInput: string)
  {
    if (!this.EditMode)
    {
      this.EditMode = true;
      console.log("edit mode", this.EditMode);
      event.srcElement.innerText = "";
    } else
    {
      this.SendBlock(event,blockContentInput);
    }
  }

  EditModeOff (blockContentInput: string)
  {
    if (blockContentInput.length < 1)
    {
      this.EditMode = false;
    }
  }
}
