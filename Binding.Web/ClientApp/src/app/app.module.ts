import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import {RouterModule} from '@angular/router';

import {AppComponent} from './app.component';
import {NavMenuComponent} from './nav-menu/nav-menu.component';
import {HomeComponent} from './home/home.component';
import {PageComponent} from './page/page.component';
import {BlockComponent} from './block/block.component';
import {SideBarComponent} from './side-bar/side-bar.component';
import {LoginComponent} from './login/login.component';
import {AuthGuard} from "./_guards/auth.guard";
import {LoadingBarModule} from "ngx-loading-bar";
import { OrderPipe } from './order.pipe';
import {SidePageComponent} from "./side-bar/page/page.component";
import {AutocompleteLibModule} from "angular-ng-autocomplete";
import { EnumToArrayPipe } from './enum-to-array.pipe';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        PageComponent,
        BlockComponent,
        SideBarComponent,
        LoginComponent,
        OrderPipe,
        SidePageComponent,
        EnumToArrayPipe
    ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      {path: '', component: LoginComponent, pathMatch: 'full'},
      {path: 'home', component: HomeComponent, canActivate: [AuthGuard]}
    ]),
    LoadingBarModule,
    AutocompleteLibModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule
{
}
