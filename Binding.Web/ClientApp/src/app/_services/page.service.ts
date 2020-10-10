import {Injectable} from '@angular/core';
import {Page} from "../_models/Page";

@Injectable({
  providedIn: 'root'
})
export class PageService
{

  public Pages: Page[];

  constructor ()
  {
  }
}
