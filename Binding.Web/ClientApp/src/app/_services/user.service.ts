import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {User, UserViewModel} from "../_models/User";
import {map} from "rxjs/operators";
import {PageService} from "./page.service";
import 'automapper-ts';
import {Page} from "../_models/Page";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService
{
  public accessToken: string = null;
  private readonly BaseUrl: string;
  private user: User;
  //
  // private autoMapper: AutoMapp

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private pageService: PageService)
  {
    this.BaseUrl = baseUrl;
  }

  public Authenticate (email: string, password: string): Observable<UserViewModel>
  {
    let auth = {
      Email: email,
      Password: password
    }

    automapper.createMap('PageWithNoBlocksViewModel', 'Page');


    return this.http.post<UserViewModel>(this.BaseUrl + 'api/user/authenticate', auth).pipe(map(user =>
    {
      console.log("Authed -> ", user);
      this.accessToken = user.Token;
      this.user = {
        Id: user.Id,
        Created: user.Created,
        Updated: user.Updated,
        DisplayName: user.DisplayName,
        Email: user.Email,
        Token: user.Token,
      };

      this.pageService.Pages = user.Pages.map(page =>
      {
        let p: Page = {
          Id: page.Id,
          Name: page.Name,
          Order: page.Order,
          Created: page.Created,
          Updated: page.Updated,
          Children: automapper.map('PageWithNoBlocksViewModel', 'Page', page.Children),
          Owner: user,
          Top: true,
          Blocks: []
        }
        return p;
      }).sort((a, b) =>
      {
        if (a.Order < b.Order)
        {
          return -1;
        }
        if (b.Order > a.Order)
        {
          return 1;
        }
        return 0;
      });

      console.log("Pages -> ", this.pageService.Pages);
      return user;
    }));
  }

  public Authenticated (): boolean
  {
    return this.accessToken != null;
  }

}
