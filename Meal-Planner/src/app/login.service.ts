import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Observable, catchError, retry} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private apiUrl = 'https://localhost:7246/api/';

  constructor(private http:HttpClient) { }

  sendLoginData(username: string, password: string): Observable<any>{
    const body = {
      username: username,
      password: password
    }

    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post(`${this.apiUrl}User/validate`, body,{headers, responseType: 'text'});
  }

}
