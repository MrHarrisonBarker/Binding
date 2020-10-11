import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Block} from "../_models/Block";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import {PageService} from "./page.service";
import {Page} from "../_models/Page";
import {BuildingBlocksService} from "../../../projects/building-blocks/src/lib/building-blocks.service";

@Injectable({
  providedIn: 'root'
})
export class BlockService
{

  private BaseUrl: string;


  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private pageService: PageService, private buildingBlocks: BuildingBlocksService)
  {
    this.BaseUrl = baseUrl;
  }

  public CreateBlock (block: Block, page: Page): Observable<Block>
  {
    return this.http.post<Block>(this.BaseUrl + 'api/block',{
      Block: block,
      PageId: page.Id
    }).pipe(map(block => {

      if (block){

        console.log("Created block",block);

        // this.pageService.Pages[this.pageService.Pages.findIndex(x => x.Id == pageId)].Blocks.push(block);

        if (page.Blocks == null) {
          page.Blocks = [];
        }
        page.Blocks.push(block);

        let result: boolean = false;
        for (let i = 0; !result && i < this.pageService.Pages.length; i++)
        {
          if (this.buildingBlocks.updateTree(this.pageService.Pages[i], page.Id, page)) {
            console.log("Updated node");
            result = true;
          }
        }

      }

      return block;
    }));
  }

  public UpdateBlock ()
  {

  }
}
