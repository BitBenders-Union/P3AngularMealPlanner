import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RecipeServiceService } from '../service/recipe-service.service';
import { Rating, Recipe } from '../Interfaces';
import { StarService } from '../service/star.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-recipe-detail',
  templateUrl: './recipe-detail.component.html',
  styleUrls: ['./recipe-detail.component.css'],
})
export class RecipeDetailComponent implements OnInit {

  // recipe can either hold a recipe or null
  // initially it is set to null
  recipe: Recipe | undefined = undefined;
  stars: (boolean | string)[] = new Array(5).fill('empty'); // Initialize with empty stars
  rating: number = 0;
  // Inject services and routes
  constructor(private route: ActivatedRoute, private recipeService: RecipeServiceService, public starService: StarService, private router: Router) {}


  ngOnInit(): void {
    
    // Get the id from the route
    // cast to number
    // check if it is a number
    // if it is a number, call the service to fetch recipe by id
    // set this.recipe to the recipe returned from the service
    // html then renders this.recipe

    // do nothing if the id is not a number
    // html will display error message 'no recipe found'



      this.route.paramMap.subscribe(params => {
        const recipeId = Number(params.get('id'));
        if(!isNaN(recipeId)){
            this.recipeService.getRecipeById(recipeId!).subscribe(recipe =>{
              this.recipe = recipe;
            });
        }

        this.recipeService.GetRecipeRating(recipeId).subscribe({
          next: (rating: Rating) => {
            this.rating = rating.score;
            console.log(this.rating);
          },
          error: (error) => {
            console.error("Recipe rating Error: ",error);
          },
          complete: () => {
            this.stars = this.starService.getRatingStars(this.rating);
            console.log(this.stars);
          }
        });
        
    });

    
  }

  // routes to the update page using the recipe id
  updateRecipe() {
    if (this.recipe) {

      this.router.navigate(['/update', this.recipe.id]);
    }
  }


  // deletes the recipe
  // navigates back to the search page
  deleteRecipe() {
    if (this.recipe) {
      if (confirm('Are you sure you want to delete this recipe?')) {
        this.recipeService.deleteRecipe(this.recipe.id).subscribe({
          next: () => {
            // Handle successful deletion (e.g., navigate back to the recipe list)
            this.router.navigate(['/search']);
          },
          error: (error) => {
            console.error("Recipe delete Error: ",error);
          }
        })
      }
    }
  }

  hoverStar(index: number) {
    this.stars = this.starService.getRatingStars(index + 1);
  }

  resetStars() {
    // Reset the stars when mouse leaves the container
    this.stars = new Array(5).fill('empty');
  }

  rateRecipe(rating: number) {
    // Implement your logic to save the rating, e.g., call an API
    console.log(`User rated the recipe with ${rating + 1} stars.`);
  }

  
}
