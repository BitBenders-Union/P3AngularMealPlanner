import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpHeaderResponse } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { CreateRecipe, Recipe } from './Interfaces';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RecipeServiceService{

  constructor(private http: HttpClient) {

  }


  // url: string = 'https://localhost:7268/api/Recipe';
  url = './assets/Recipes.json';



  getRecipes(): Observable<Recipe[]> {
    
    const headers = new HttpHeaders().set('content-type', 'application/json')

    return this.http.get<Recipe[]>(this.url, { headers }).pipe(
      catchError(error => {
        console.error('Error getting recipes:', error);
        throw error;
      }));
  }

  getRecipeById(id: number): Observable<Recipe> {

    return this.http.get<Recipe>(`${this.url}/${id}`).pipe(
      catchError(error => {
        console.error('Error getting recipe by id:', error);
        throw error;
      }));

  }

  // temporary method that gets data from json file instead of api
  getRecipeFromJson(id: number): Observable<Recipe | undefined> {
    return this.http.get<Recipe[]>(this.url).pipe(
      map((data) => {
        const recipe = data.find((x) => x.id === id);
        return recipe;
      })
    );
  }


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

  updateRecipe( recipeData: CreateRecipe, recipeId: number): Observable<any> {
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
