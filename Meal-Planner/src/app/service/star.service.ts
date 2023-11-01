import { Injectable } from '@angular/core';
import { RatingWithRecipeId, RecipeIdAndStarsArray } from '../Interfaces';

@Injectable({
  providedIn: 'root'
})
export class StarService {

  // since we are rendering the stars multiple times throughout the website, we created a service to handle the logic
  // so we don't have to repeat ourselves
  getRatingStars(rating: number): (boolean | string)[] {
    if (rating === null || isNaN(rating)) {
      return new Array(5).fill('empty');
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
      return starsArray;
  }



  getRatingStarsAndRecipeId(rating: RatingWithRecipeId): RecipeIdAndStarsArray{

    if (rating === null || isNaN(rating.score)) {
      const returnObject: RecipeIdAndStarsArray = {
        recipeId: rating.recipeId,
        starsArray: new Array(5).fill('empty')
      }
      return returnObject
    }
  

    const fullStars = Math.floor(rating.score);
    const halfStar = rating.score % 1 >= 0.5;
  
    const starsArray = new Array(5).fill('empty'); // Initialize with 'empty' values
  
    for (let i = 0; i < fullStars; i++) {
      starsArray[i] = true; // Fill with 'true' for full stars
    }
  
    if (halfStar) {
      starsArray[fullStars] = 'half'; // Set the half star
    }

    const returnObject: RecipeIdAndStarsArray = {
      recipeId: rating.recipeId,
      starsArray: starsArray
    }
      return returnObject;
  }


}
