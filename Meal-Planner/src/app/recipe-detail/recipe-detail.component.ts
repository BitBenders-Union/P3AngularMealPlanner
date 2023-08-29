import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RecipeServiceService } from '../recipe-service.service';
import { Recipe } from '../Interfaces';
import { StarService } from '../star.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-recipe-detail',
  templateUrl: './recipe-detail.component.html',
  styleUrls: ['./recipe-detail.component.css']
})
export class RecipeDetailComponent implements OnInit {

  // recipe can either hold a recipe or null
  // initially it is set to null
  recipe: Recipe | null = null;

  // Inject ActivatedRoute and RecipeServiceService
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
    });
  }

  updateRecipe() {
    if (this.recipe) {
      // Implement logic to open a modal or navigate to an update form
      // Pass this.recipe.id to the form to identify the recipe to update
      console.log(this.recipe.id);
      this.router.navigate(['/update', this.recipe.id]);
    }
  }

  deleteRecipe() {
    if (this.recipe) {
      if (confirm('Are you sure you want to delete this recipe?')) {
        this.recipeService.deleteRecipe(this.recipe.id).subscribe(() => {
          // Handle successful deletion (e.g., navigate back to the recipe list)
          this.router.navigate(['/search']);
        });
      }
    }
  }



  
}
