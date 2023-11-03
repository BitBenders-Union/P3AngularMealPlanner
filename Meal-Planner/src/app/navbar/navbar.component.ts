import { Component, OnInit } from '@angular/core';
import { LoginService } from '../service/login.service';
import { UserStoreService } from '../service/user-store.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit{

constructor(
  private loginService: LoginService,
  private userStore: UserStoreService,
  private auth: LoginService
  ) { }

public userName: string = "";


  ngOnInit(): void {
    this.userStore.getUserFromStore()
      .subscribe(val =>{ 
        let userNameFromToken = this.auth.getUsernameFromToken();
        this.userName = val || userNameFromToken;

      })

  }


  logout(){
    this.loginService.signOut();
  }
}
