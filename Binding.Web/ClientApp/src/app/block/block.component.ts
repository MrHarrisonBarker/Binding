import {Component, Input, OnInit} from '@angular/core';
import {Block} from "../_models/Block";

@Component({
  selector: 'block',
  templateUrl: './block.component.html',
  styleUrls: ['./block.component.css']
})
export class BlockComponent implements OnInit
{

  @Input() Block: Block
  EditMode: boolean = false;
  Hover: boolean = false;

  constructor ()
  {
  }

  ngOnInit ()
  {
    console.log("Block loaded", this.Block)
  }

}
