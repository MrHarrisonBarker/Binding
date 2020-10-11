import {Component, Input, OnInit} from '@angular/core';
import {Page} from "../../_models/Page";
import {PageService} from "../../_services/page.service";

@Component({
  selector: 'Side-Page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.css']
})
export class SidePageComponent implements OnInit
{

  @Input() Page: Page;

  constructor (private pageService: PageService)
  {
  }

  ngOnInit ()
  {
  }

  SelectPage ()
  {
    this.pageService.GetPage(this.Page.Id).subscribe(page =>
    {
      console.log("Got page", page);
      this.pageService.CurrentPage = page;
    });
  }
}
