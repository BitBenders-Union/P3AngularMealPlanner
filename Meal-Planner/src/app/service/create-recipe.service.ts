import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CreateRecipeService {
  // private apiUrl = 'https://localhost:7268'
  private apiUrl = 'http://192.168.21.22:5555'
  // private apiUrl = 'http://localhost:5000'

  constructor(private http: HttpClient) {}

  createRecipe(recipeData: any): Observable<any> {
    console.log(recipeData);
    return this.http.post(`${this.apiUrl}/Recipe/create`, recipeData);
  }


  // we aren't using this at all
}
