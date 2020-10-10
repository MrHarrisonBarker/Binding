import {Component, OnInit} from '@angular/core';
import {PageService} from "../_services/page.service";

@Component({
  selector: 'side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css']
})
export class SideBarComponent implements OnInit
{

  constructor (public pageService: PageService)
  {
  }

  ngOnInit ()
  {
  }

}
