import { Component, Input, OnInit } from '@angular/core';
import { CdkDrag } from '@angular/cdk/drag-drop';
import { Rating, RatingDTO, Recipe } from '../Interfaces';
import { RecipeServiceService } from '../service/recipe-service.service';
import { StarService } from '../service/star.service';



@Component({
  selector: 'app-recipe-card',
  templateUrl: './recipe-card.component.html',
  styleUrls: ['./recipe-card.component.css']
})
export class RecipeCardComponent implements OnInit{

  @Input() recipe: Recipe | null = null; // Input decorator to pass the recipe object to the component

  recipes: Recipe[] = []; // Initialize recipes as an empty array
  stars: any[][] = [];
  rating: number = 0;
  
  constructor(private recipeService: RecipeServiceService, public starService: StarService) { } // Inject the services

  ngOnInit(): void {

    

    this.recipeService.getRecipes().subscribe({
      next: (recipes: Recipe[]) => {
        this.recipes = recipes;
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
        this.recipes.forEach( (recipe) => {
          this.getRating(recipe.id);
          console.log("recipes complete");
          console.log(this.stars[0])
        });

      }
    
    });
    
  }



  getRating(recipeId: number): void {
    this.recipeService.GetRecipeRating(recipeId).subscribe({
      next: (rating: RatingDTO) => {
        this.rating = rating.score;
        console.log(rating);
        console.log(this.rating)
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
        this.stars.push(this.starService.getRatingStars(this.rating));
        console.log("stars complete");
        console.log(this.stars)
      }});    
  }



}
