import {Injectable} from "@angular/core";
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from "rxjs";
import {UserService} from "./_services/user.service";

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor
{
  constructor(private authService: UserService)
  {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
  {
    if (this.authService.accessToken != null) {
      console.log("using bearer token");
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${this.authService.accessToken}`
        }
      });
    }
    return next.handle(req);
  }

}
