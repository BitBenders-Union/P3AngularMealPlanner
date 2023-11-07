import { Component, Input, OnInit } from '@angular/core';
import { CdkDrag } from '@angular/cdk/drag-drop';
import { Rating, RatingDTO, RatingWithRecipeId, Recipe, RecipeWithScore } from '../Interfaces';
import { RecipeServiceService } from '../service/recipe-service.service';
import { StarService } from '../service/star.service';



@Component({
  selector: 'app-recipe-card',
  templateUrl: './recipe-card.component.html',
  styleUrls: ['./recipe-card.component.css']
})
export class RecipeCardComponent implements OnInit{

  // @Input() recipe: Recipe | null = null; // Input decorator to pass the recipe object to the component

  recipes: Recipe[] = []; // Initialize recipes as an empty array
  scoreRecipe: RecipeWithScore[] = [];
  stars: any[] = [];

  rating: RatingWithRecipeId ={
    recipeId: 0,
    score: 0
  }
  
  constructor(
    private recipeService: RecipeServiceService, 
    public starService: StarService) { } // Inject the services

  ngOnInit(): void {

    this.recipeService.getRecipes().subscribe({
      next: (recipes: Recipe[]) => {
        this.recipes = recipes;

        this.recipes.forEach(recipe => {
          const recipeWithScore: RecipeWithScore = {
            id: recipe.id,
            title: recipe.title,
            score: [],
            recipe: recipe
          }
          this.scoreRecipe.push(recipeWithScore);
        });

      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
        this.scoreRecipe.forEach((recipe) => {
          this.getRating(recipe.id);

        });
      }
    });
    
  }




  getRating(recipeId: number): void {
    this.recipeService.GetRecipeRating(recipeId).subscribe({
      next: (rating: RatingDTO) => {
        this.rating.score = rating.score;
        this.rating.recipeId = recipeId;
        const recipeWithScore = this.scoreRecipe.find(recipe => recipe.id === recipeId);
        recipeWithScore!.score = this.starService.getRatingStars(this.rating.score);
      },
      error: (error) => {
        console.error(error);
        this.rating.score = 0;
        this.rating.recipeId = recipeId;
        const recipeWithScore = this.scoreRecipe.find(recipe => recipe.id === recipeId);
        recipeWithScore!.score = this.starService.getRatingStars(this.rating.score);
      },
    });    
  }



}
