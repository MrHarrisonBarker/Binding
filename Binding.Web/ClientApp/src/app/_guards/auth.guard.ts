import { Injectable } from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router} from '@angular/router';
import {Observable, of} from 'rxjs';
import {UserService} from "../_services/user.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor (private userService: UserService, private router: Router)
  {
  }

  canActivate (route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean>
  {
    if (this.userService.Authenticated()) {
      return of(true);
    } else {
      this.router.navigate([""]);
      return of(false);
    }
  }

}
