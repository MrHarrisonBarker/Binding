import {Component, OnInit} from '@angular/core';
import {PageService} from "../_services/page.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit
{
  constructor (public pageService: PageService)
  {
  }

  ngOnInit (): void
  {
    this.pageService.CurrentPage = this.pageService.Pages[0];
    this.pageService.GetPage(this.pageService.CurrentPage.Id).subscribe(page =>
    {
      console.log("Got page", page);
      this.pageService.CurrentPage = page;
    });
  }
}
