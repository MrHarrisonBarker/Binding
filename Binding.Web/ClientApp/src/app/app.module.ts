import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { PageComponent } from './page/page.component';
import { BlockComponent } from './block/block.component';
import { SideBarComponent } from './side-bar/side-bar.component';
import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    PageComponent,
    BlockComponent,
    SideBarComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: LoginComponent, pathMatch: 'full' }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
