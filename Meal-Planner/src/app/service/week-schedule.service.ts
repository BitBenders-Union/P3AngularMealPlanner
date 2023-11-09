import { Injectable } from '@angular/core';
import { HttpClient, } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { Recipe, Ingredient, RecipeScheduleDTO, RecipeDTO } from '../Interfaces';

@Injectable({
  providedIn: 'root'
})
export class WeekScheduleService {

  // private dataUrl = 'https://localhost:7268/api'; // Adjust the path if needed
  private dataUrl = 'http://192.168.21.22:5555/api'; // Adjust the path if needed
  
  // private dataUrl = 'http://localhost:5000/api'; // Adjust the path if needed

  constructor(private http: HttpClient) { }

  // getData(): Observable<any> {
  //   return this.http.get(`${this.dataUrl}/RecipeSchedule`);
  // }

  updateData(updatedData: RecipeScheduleDTO): Observable<any> {
    return this.http.patch(`${this.dataUrl}/RecipeSchedule/update`, updatedData).pipe(
      catchError(error => {
        console.log(error);
        throw error;
      }
    ));

  }


  // this should get a list of rows, columns, and recipeID's
  // based on the user id
  getWeekScheduleData(userId: number): Observable<any>{
    return this.http.get<RecipeScheduleDTO[]>(`${this.dataUrl}/RecipeSchedule/byUserId/${userId}`).pipe(
      catchError(error => {
        console.log(error);
        throw error;
      }));

  }

  // remember to change /endpoint to the correct endpoint
  // gets recipes that from an array of ids
  getScheduledRecipes(recipeIDs: number[]): Observable<any>{
    const params = {recipeIDs: recipeIDs.join(',')};
    return this.http.get(this.dataUrl + '/endpoint', {params}).pipe(error=>{
      console.log(error);
      return error;
    })
  }

  // post
  // createScheduleEntry(weekData: WeekData): Observable<any>{
  //   return this.http.post(this.dataUrl + '/endpoint', weekData).pipe(error=>{
  //     console.log(error);
  //     return error;
  //   })   
  // }

  // patch
  updateScheduleEntry(weekData: RecipeScheduleDTO): Observable<any>{
    return this.http.put(`${this.dataUrl}/RecipeSchedule/update`, weekData).pipe(error=>{
      console.log(error);
      return error;
    })
  }


  // delete

  // deleteScheduleEntry(weekDataId: number): Observable<any>{
  //   return this.http.delete(`${this.dataUrl}/endpoint/${weekDataId}`).pipe(error=>{
  //     console.log(error);
  //     return error;
  //   })
  // }


}

/* 
Idea

Create a table with mon-sun as columns and the recipeID as rows.

then fetch the data from the database and fill the table with the recipeID's


*/