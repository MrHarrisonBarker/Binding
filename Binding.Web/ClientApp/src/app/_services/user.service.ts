import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {User} from "../_models/User";

@Injectable({
  providedIn: 'root'
})
export class UserService
{
  private readonly baseUrl: string;
  public User: User;

  constructor (private http: HttpClient, @Inject('BASE_URL') baseUrl: string)
  {
    this.baseUrl = baseUrl;
  }

  public Authenticate (email: string, password: string)
  {

  }

}
