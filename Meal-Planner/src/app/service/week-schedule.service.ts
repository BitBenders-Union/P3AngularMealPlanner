import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Recipe, Ingredient, WeekData } from '../Interfaces';

@Injectable({
  providedIn: 'root'
})
export class WeekScheduleService {

  private dataUrl = '/assets/recipes-data.json'; // Adjust the path if needed

  constructor(private http: HttpClient) { }

  getData(): Observable<any> {
    return this.http.get(this.dataUrl);
  }

  updateData(updatedData: any): Observable<any> {
    return this.http.put(this.dataUrl, updatedData);
  }


  // this should get a list of rows, columns, and recipeID's
  // based on the user id
  getWeekScheduleData(userId: number): Observable<any>{
    return this.http.get(`${this.dataUrl}/${userId}`).pipe(error=>{
      console.log(error);
      return error;
    })
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
  createScheduleEntry(weekData: WeekData): Observable<any>{
    return this.http.post(this.dataUrl + '/endpoint', weekData).pipe(error=>{
      console.log(error);
      return error;
    })   
  }

  // put / update
  updateScheduleEntry(weekData: WeekData): Observable<any>{
    return this.http.put(this.dataUrl + '/endpoint', weekData).pipe(error=>{
      console.log(error);
      return error;
    })
  }


  // delete

  deleteScheduleEntry(weekDataId: number): Observable<any>{
    return this.http.delete(`${this.dataUrl}/endpoint/${weekDataId}`).pipe(error=>{
      console.log(error);
      return error;
    })
  }


}

/* 
Idea

Create a table with mon-sun as columns and the recipeID as rows.

then fetch the data from the database and fill the table with the recipeID's


*/