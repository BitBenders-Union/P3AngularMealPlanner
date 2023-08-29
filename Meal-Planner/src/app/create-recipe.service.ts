import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CreateRecipe } from './Interfaces';

@Injectable({
  providedIn: 'root'
})
export class CreateRecipeService {

  private url = 'https://localhost:7268/api/Recipe';

  constructor(private http: HttpClient) {}

  createRecipe(recipeData: CreateRecipe): Observable<any> {
    const url = `${this.url}`; 

    // Send a POST request to the API
    return this.http.post(url, recipeData).pipe(
      catchError(error => {
        console.error('Error creating recipe:', error);
        throw error;
      })
    );
  }

  deleteRecipe(recipeId: number): Observable<any> {
    const url = `${this.url}/${recipeId}`;
    return this.http.delete(url).pipe(
      catchError(error => {
        console.error('Error deleting recipe:', error);
        throw error;
      })
    );
  }

  updateRecipe(recipeId: number, recipeData: CreateRecipe): Observable<any> {
    const url = `${this.url}/${recipeId}`;

    // Send a PUT request to the API
    return this.http.put(url, recipeData).pipe(
      catchError(error => {
        console.error('Error updating recipe:', error);
        throw error;
      })
    );
  }

}