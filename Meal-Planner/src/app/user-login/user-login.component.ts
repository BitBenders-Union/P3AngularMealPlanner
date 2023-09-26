import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../login.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent {
  loginForm!: FormGroup;

  constructor(
    private loginService: LoginService,
    private formBuilder: FormBuilder,
    private router: Router
  ){
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
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
          console.log("Success", data);
          console.log("User ID: ", data.id);
          this.loginService.storeToken(data.token);
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
