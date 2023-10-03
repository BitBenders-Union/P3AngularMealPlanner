import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpHeaderResponse } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { Category, Recipe, RecipeDTO } from '../Interfaces';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RecipeServiceService{

  constructor(private http: HttpClient) {

  }


  url: string = 'https://localhost:7268/api';
  // url = '../assets/Recipes.json';



  getRecipes(): Observable<Recipe[]> {
    
    const headers = new HttpHeaders().set('content-type', 'application/json')

    return this.http.get<Recipe[]>(this.url, { headers }).pipe(
      catchError(error => {
        console.error('Error getting recipes:', error);
        throw error;
      }));
  }

  getRecipeById(id: number): Observable<Recipe> {

    return this.http.get<Recipe>(`${this.url}/ById/${id}`).pipe(
      catchError(error => {
        console.error('Error getting recipe by id:', error);
        throw error;
      }));

  }


  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.url}/Category`).pipe(
      catchError(error => {
        console.error('Error getting categories:', error);
        throw error;
      }));
  }

  // // temporary method that gets data from json file instead of api
  // getRecipeFromJson(id: number): Observable<Recipe | undefined> {
  //   return this.http.get<Recipe[]>(this.url).pipe(
  //     map((data) => {
  //       const recipe = data.find((x) => x.id === id);
  //       return recipe;
  //     })
  //   );
  // }


  createRecipe(recipeData: RecipeDTO): Observable<any> {
    const url = `${this.url}`; 

    // Send a POST request to the API
    return this.http.post(`${this.url}/Recipe/create`, recipeData).pipe(
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

  updateRecipe( recipeData: Recipe, recipeId: number): Observable<any> {
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
