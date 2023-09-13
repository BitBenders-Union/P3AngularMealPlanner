import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Observable, catchError, retry} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private apiUrl = 'https://localhost:7246/api/';

  constructor(private http:HttpClient) { }

  sendLoginData(data: any): Observable<any>{
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.post(`${this.apiUrl}/validate`, data,{headers, responseType: 'text'});
  }

}
