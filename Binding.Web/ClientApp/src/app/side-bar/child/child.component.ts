import {Component, Input, OnInit} from '@angular/core';
import {Page} from "../../_models/Page";

@Component({
  selector: 'child',
  templateUrl: './child.component.html',
  styleUrls: ['./child.component.css']
})
export class ChildComponent implements OnInit {

  @Input() Child: Page;

  constructor() { }

  ngOnInit() {
  }

}
