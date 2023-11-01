import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RecipeServiceService } from '../service/recipe-service.service';
import { Rating, RatingDTO, Recipe, User, UserOnlyName } from '../Interfaces';
import { StarService } from '../service/star.service';
import { Router } from '@angular/router';
import { LoginService } from '../service/login.service';
import { UserStoreService } from '../service/user-store.service';


@Component({
  selector: 'app-recipe-detail',
  templateUrl: './recipe-detail.component.html',
  styleUrls: ['./recipe-detail.component.css'],
})
export class RecipeDetailComponent implements OnInit {

  // recipe can either hold a recipe or null
  // initially it is set to null
  recipe: Recipe | undefined = undefined;
  stars: (boolean | string)[] = []; // Initialize with empty stars
  rating: number = 0;
  // Inject services and routes
  constructor(
    private route: ActivatedRoute, 
    private recipeService: RecipeServiceService, 
    public starService: StarService, 
    private router: Router,
    private tokenService: LoginService,
    private userStore: UserStoreService
    ) {}

    private user: User = {
      id: 0,
      username: ''
    }

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
        const recipeId: number = Number(params.get('id'));
        if(!isNaN(recipeId)){

          this.recipeService.getRecipeById(recipeId).subscribe({
            next: (recipe: Recipe) => {
              this.recipe = recipe;
            },
            error: (error) => {
              console.error("Recipe get Error: ",error);
            }
          });
        }

        this.recipeService.GetRecipeRating(recipeId).subscribe({
          next: (rating: Rating) => {
            this.rating = rating.score;
            this.stars = this.starService.getRatingStars(this.rating);
          },
          error: (error) => {
            console.error("Recipe rating Error: ",error);
            this.rating = 0;
            this.stars = this.starService.getRatingStars(this.rating);
          }
        });
        
    });

    this.userStore.getUserFromStore().subscribe({
      next: user => {
        this.user!.username = user;
        if(this.user!.username === ''){
          this.user!.username = this.tokenService.getUsernameFromToken();
        }
      },
      error: error => console.error('There was an error!', error),
    });

    this.userStore.getIdFromStore().subscribe({
      next: id => {
        this.user!.id = id;
        if(this.user!.id === 0){
          this.user!.id = this.tokenService.getIdFromToken();
        }
      },
      error: error => console.error('There was an error!', error),
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

  

  rateRecipe(rating: number) {
    // Implement your logic to save the rating, e.g., call an API
    console.log(`User rated the recipe with ${rating + 1} stars.`);
    const Rating: RatingDTO = {
      score: rating + 1
    }

    this.recipeService.createRating(Rating, this.user!.id, this.recipe!.id).subscribe({
      next: () => {
        this.rating = Rating.score;
        this.stars = this.starService.getRatingStars(this.rating);



        // make it so the displayed rating can't be hovered over after making a rating

      },
      error: (error) => {
        console.error("Recipe rating Error: ",error);
      }

    });

  }

  
  validateRecipeUser(){;
    if(this.recipe!.user.id == this.user?.id)
      {
      return true;
    }
    else{
      return false;
    }

  }

}
