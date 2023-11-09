import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../service/login.service';
import { Route, Router } from '@angular/router';
import { AbstractControl, ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css'],
})
export class UserRegisterComponent {
  registerForm: FormGroup;
  registrationError: boolean = false;
  isLoading: boolean = false;
  showUsernameError: boolean = false;
  passwordsDoNotMatch: boolean = false;

  usernameErrorMessages: string = '';
  passwordErrorMessages: string = '';
  

  constructor(
    private loginService: LoginService,
    private formBuilder: FormBuilder,
    private router: Router,    

  )
  {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required ],
      password: ['', Validators.required ],
      confirmPassword: ['', Validators.required]
    });

    this.registerForm.get('username')?.valueChanges.subscribe( () => {
      if(this.registrationError){
        this.registrationError = false;
      }
    });
  
  }

  onSubmit(): void{


    if(this.registerForm.valid && !this.passwordsDoNotMatch && !this.showUsernameError){

      this.ToggleLoadingSpinner()
      console.log("loading spinner state: ", this.isLoading)

      const userData = {
        username: this.registerForm.value.username,
        password: this.registerForm.value.password
      };

      this.loginService.createLogin(userData).subscribe({
        next: (data: any) => {
          console.log('Register success', data);
          this.router.navigate(['/login']);
        },

        error: (error) =>{
          console.error('HTTP Error', error);
          console.log('Full Error Response: ');
          this.ToggleLoadingSpinner();
          console.log("loading spinner state: ", this.isLoading)
          this.registerForm.reset();
          this.registrationError = true
        }
      });
    }

  }

checkUsernameValidity() {
  const usernameValue = this.registerForm.get('username')?.value;
  if (usernameValue.includes(' ')) {
    this.showUsernameError = true;
  } else {
    this.showUsernameError = false;
  }
}

  passwordsDoNotMatchMethod() {
    console.log("it's running")
    const password = this.registerForm.get('password')?.value;
    console.log("password: ", password)
    const confirmPassword = this.registerForm.get('confirmPassword')?.value;
    console.log("confirmPassword: ", confirmPassword)

    if (password !== confirmPassword && password.includes(' ') || password !== confirmPassword && confirmPassword.includes(' ') ){
      this.passwordErrorMessages = 'Passwords do not match and cannot contain spaces';
      
      this.passwordsDoNotMatch = true;
    }
    else if (password.includes(' ')){
      this.passwordErrorMessages = 'Password cannot contain spaces';

      this.passwordsDoNotMatch = true;

    }
    else if (password !== confirmPassword){
      this.passwordErrorMessages = 'Passwords do not match';

      this.passwordsDoNotMatch = true;

    }
    else{

      this.passwordsDoNotMatch = false;
    }

    console.log("passwordsDoNotMatch: ", this.passwordsDoNotMatch)


  }

  ToggleLoadingSpinner(){
    this.isLoading = !this.isLoading;

  }

}
