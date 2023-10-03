import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CreateRecipeService {
  private apiUrl = 'https://localhost:7268'

  constructor(private http: HttpClient) {}

  createRecipe(recipeData: any): Observable<any> {
    console.log(recipeData);
    return this.http.post(`${this.apiUrl}/Recipe/create`, recipeData);
  }
}
