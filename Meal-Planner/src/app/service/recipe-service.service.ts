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
    
    // const headers = new HttpHeaders().set('content-type', 'application/json')

    return this.http.get<Recipe[]>(`${this.url}/Recipe`).pipe(
      catchError(error => {
        if(error.status === 404){
          console.log("404 error ", error);
        }
        // console.error('Error getting recipes:', error);
        throw error;
      }));
  }



  getRecipeById(id: number): Observable<Recipe> {

    return this.http.get<Recipe>(`${this.url}/Recipe/ById/${id}`).pipe(
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


  createRecipe(recipeData: RecipeDTO): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    // Send a POST request to the API
    return this.http.post(`${this.url}/Recipe/create`, recipeData, {headers}).pipe(
      catchError(error => {
        console.error('Error creating recipe:', error);
        throw error;
      })
    );
  }

  deleteRecipe(recipeId: number): Observable<any> {
    return this.http.delete(`${this.url}/Recipe/delete/${recipeId}`);
  }

  //------------------- changing -------------------------------
  updateRecipe( recipeData: RecipeDTO, recipeId: number): Observable<any> {
    // Send a PUT request to the API
    return this.http.put(`${this.url}/Recipe/update/${recipeId}`, recipeData);
  }
}
