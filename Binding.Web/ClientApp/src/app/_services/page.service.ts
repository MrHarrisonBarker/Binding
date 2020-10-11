import {Inject, Injectable} from '@angular/core';
import {Page, PageWithBlocksViewModel} from "../_models/Page";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {map} from "rxjs/operators";
import 'automapper-ts';
import {BuildingBlocksService} from "../../../projects/building-blocks/src/lib/building-blocks.service";

@Injectable({
    providedIn: 'root'
})
export class PageService
{

    public Pages: Page[];
    public CurrentPage: Page;
    private BaseUrl: string;


    constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private buildingBlocks: BuildingBlocksService)
    {
        this.BaseUrl = baseUrl;
    }

    public GetPage (id: string): Observable<Page>
    {
        automapper.createMap('BlockViewModel', 'Block');

        return this.http.get<PageWithBlocksViewModel>(this.BaseUrl + `api/page/${id}`).pipe(map(page =>
        {

            let newPage: Page = {
                Id: page.Id,
                Updated: page.Updated,
                Created: page.Created,
                Name: page.Name,
                Order: page.Order,
                Blocks: automapper.map('BlockViewModel', 'Block', page.Blocks),
                Children: automapper.map('PageWithNoBlocksViewModel', 'Page', page.Children)
            }

            console.log("searching for page", this.buildingBlocks.searchTree(this.Pages[0], page.Id));
            console.log("updating", this.buildingBlocks.updateTree(this.Pages[0], page.Id, newPage));


            let result: boolean = false;
            for (let i = 0; !result && i < this.Pages.length; i++)
            {
                if (this.buildingBlocks.updateTree(this.Pages[i], page.Id, newPage)) {
                    console.log("Updated node");
                    result = true;
                }
            }


            // console.log(this.Pages[0]);

            // let pageIndex = this.Pages.findIndex(p => p.Id == page.Id);
            //
            // if (pageIndex == -1)
            // {
            //     this.Pages.push(newPage);
            // } else
            // {
            //     this.Pages[pageIndex] = newPage;
            // }
            //
            console.log("Page store", this.Pages);

            return newPage;
        }));
    }
}
