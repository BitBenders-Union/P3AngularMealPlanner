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
  recipe: Recipe | undefined = undefined;

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
            

          // change this to getRecipeById when we are using the api
            this.recipeService.getRecipeById(recipeId!).subscribe(recipe =>{
              this.recipe = recipe;
            });
        }   
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
        this.recipeService.deleteRecipe(this.recipe.id).subscribe(() => {
          // Handle successful deletion (e.g., navigate back to the recipe list)
          this.router.navigate(['/search']);
        });
      }
    }
  }



  
}
