import {Component, OnInit} from '@angular/core';
import {UserService} from "../_services/user.service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit
{

  constructor (private userService: UserService)
  {
  }

  ngOnInit ()
  {
    this.userService.Authenticate("harrison@thebarkers.me.uk","Password").subscribe();
  }

}
