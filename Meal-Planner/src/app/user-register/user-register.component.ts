import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LoginService } from '../service/login.service';
import { Route, Router } from '@angular/router';
import { AbstractControl, ValidationErrors } from '@angular/forms';

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

  constructor(
    private loginService: LoginService,
    private formBuilder: FormBuilder,
    private router: Router,    

  )
  {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required, ],
      password: ['', Validators.required, this.noWhitespaceValidator],
      confirmPassword: ['', Validators.required]
    });

    this.registerForm.get('username')?.valueChanges.subscribe( () => {
      if(this.registrationError){
        this.registrationError = false;
      }
    });
  
  }

  onSubmit(): void{
    
    if(this.registerForm.valid){
      this.ToggleLoadingSpinner()

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

  passwordsDoNotMatch():boolean {
    const password = this.registerForm.get('password')?.value;
    const confirmPassword = this.registerForm.get('confirmPassword')?.value;

    if (password !== confirmPassword){
      return true;
    }

    if (/\s/.test(password)){
      return true;
    }

    return false
  }



  ToggleLoadingSpinner(){
    this.isLoading = !this.isLoading;

  }

  noWhitespaceValidator(control: AbstractControl): ValidationErrors | null {
    const isWhitespace = (control.value || '').trim().length === 0;
    return isWhitespace ? { whitespace: true } : null;
  }

  



}
