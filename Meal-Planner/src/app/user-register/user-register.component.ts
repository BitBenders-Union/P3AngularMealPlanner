import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../service/login.service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent {
  registerForm: FormGroup;
  registrationError: string | null = null;

  constructor(
    private loginService: LoginService,
    private formBuilder: FormBuilder,
    private router: Router
  )
  {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    });
  }

  onSubmit(): void{
    if(this.registerForm.valid){
      const username = this.registerForm.value.username;
      const password = this.registerForm.value.password;

      const userData = {
        username: username,
        password: password
      };

      this.loginService.createLogin(userData).subscribe({
        next: (data: any) => {
          // console.log('Register success', data);
          this.router.navigate(['/login']);
        },

        error: (error) =>{
          console.error('HTTP Error', error);
          console.log('Full Error Response: ');

          this.registrationError = error.error;
        }
      });
    }
  }


  passwordsDoNotMatch():boolean {
    const password = this.registerForm.get('password')?.value;
    const confirmPassword = this.registerForm.get('confirmPassword')?.value;

    return password !== confirmPassword;
  }
}
