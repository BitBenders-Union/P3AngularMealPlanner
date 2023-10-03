
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../service/login.service';
import { Router, RouterLink } from '@angular/router';
import { UserStoreService } from '../service/user-store.service';


@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit{
  loginForm!: FormGroup;

  constructor(
    private loginService: LoginService,
    private formBuilder: FormBuilder,
    private router: Router,
    private userStore: UserStoreService
  ){
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refereshToken');

  }

  onSubmit(): void{
    if(this.loginForm.valid){
      const username = this.loginForm.value.username;
      const password = this.loginForm.value.password;

      const userData = {
        username: username,
        password: password
      };

      this.loginService.sendLoginData(username, password).subscribe({
        next: (data: any) => {
          // console.log("Success", data);
          // console.log("User ID: ", data.id);
          this.loginService.storeToken(data.accessToken);
          this.loginService.storeRefreshToken(data.refreshToken);
          // console.log(data.accessToken);
          // console.log(data.refreshToken);

          const tokenPayload = this.loginService.decodeToken();
          this.userStore.setUserInStore(tokenPayload.unique_name);
          this.userStore.setIdInStore(tokenPayload.userId);
          this.router.navigate([`/dashboard`])
        },
        error:(error) => {
          console.error("Http Error: ",error);
          this.loginForm.get('username')?.setErrors({incorrectLogin: true});
          this.loginForm.get('password')?.setErrors({incorrectLogin: true});
        }
      })
    }
  }

}
