import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Observable, catchError, retry} from 'rxjs';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenModel } from '../models/token.model';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private apiUrl = 'https://localhost:7268/';
  //private apiUrl = 'http://192.168.21.22:5555/';
  
  // private apiUrl = 'http://localhost:5000/';
  private userPayload: any;
  constructor(private http:HttpClient, private router: Router) { 
    this.userPayload = this.decodeToken();
  }

  sendLoginData(username: string, password: string): Observable<any>{
    
    const body ={
      username: username,
      password: password
    };
    
    return this.http.post(`${this.apiUrl}validate`, body, {responseType: 'json'});
  }

  createLogin(data: any): Observable<any>{

    return this.http.post(`${this.apiUrl}api/User`, data, {responseType: 'json'});
  }

  //set usertoken to storage
  storeToken(tokenValue: string){
    // console.log(tokenValue);
    localStorage.setItem('accessToken', tokenValue);
  }

  storeRefreshToken(tokenValue: string){
    localStorage.setItem('refreshToken', tokenValue);
  }

  getToken(){
    return localStorage.getItem('accessToken');
  }

  getRefreshToken(){
    return localStorage.getItem('refreshToken');
  }

  //check for token
  isLoggedIn(): boolean{
    return !! localStorage.getItem('accessToken');
  }

  

  signOut(){
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    this.router.navigate(['/login']);
  }

  decodeToken(){
    const jwtHelper = new JwtHelperService();
    const token = this.getToken()!;
    // console.log(jwtHelper.decodeToken(token))
    return jwtHelper.decodeToken(token);
  }

  getUsernameFromToken(){
    if(this.userPayload)
      return this.userPayload.unique_name;
    
  }

  getIdFromToken(){
    if(this.userPayload){
      return this.userPayload.nameid;
    }
  }

  renewToken(token: TokenModel): Observable<any>{
    return this.http.post<any>(`${this.apiUrl}api/User/refresh`, token, {responseType: 'json'});
  }

  //just to test if the api is working with the token
  testApi(): Observable<any>{
    return this.http.get(`${this.apiUrl}api/User`, {responseType: 'json'});
  }
}
