import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StarService {

  // since we are rendering the stars multiple times throughout the website, we created a service to handle the logic
  // so we don't have to repeat ourselves
  getRatingStars(rating: number): (boolean | string)[] {
    if (rating === null || isNaN(rating)) {
      return [];
    }
  

    const fullStars = Math.floor(rating);
    const halfStar = rating % 1 >= 0.5;
  
    const starsArray = new Array(5).fill('empty'); // Initialize with 'empty' values
  
    for (let i = 0; i < fullStars; i++) {
      starsArray[i] = true; // Fill with 'true' for full stars
    }
  
    if (halfStar) {
      starsArray[fullStars] = 'half'; // Set the half star
    }
    console.log(starsArray);
      return starsArray;
  }


}
