import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Router } from '@angular/router';
import { RecipeServiceService } from '../service/recipe-service.service'; // Import the service for fetching recipes
import { RatingDTO, RatingWithRecipeId, Recipe, RecipeIdAndStarsArray, RecipeWithScore } from '../Interfaces'; // Import the Recipe interface
import { StarService } from '../service/star.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
})
export class SearchComponent implements OnInit {
  
  recipes: Recipe[] = []; // Array to hold all recipes
  filteredRecipes: RecipeWithScore[] = []; // Array to hold filtered recipes that match the search term
  scoreRecipe: RecipeWithScore[] = [];


  rating: RatingWithRecipeId ={
    recipeId: 0,
    score: 0
  }


  searchTerm: string = ''; // The search term entered by the user

  private searchInputSubject = new Subject<string>(); // Subject to manage user input for search

  // what is a subject?
  // Observable and Observer: A Subject serves as both an observable (a data source that emits values over time) and an observer (a consumer of these values).

  // Publishing and Subscribing: You can use a Subject to publish values, which can then be subscribed to by multiple observers.
  // When a new value is published, all subscribed observers are notified and receive the value.
  
  // Hot Observable: Subject is considered a "hot" observable because it starts emitting values as soon as it's created, regardless of whether there are any subscribers.
  // This is in contrast to "cold" observables that start emitting values only when subscribed to.
  
  // Behavior: Depending on the type of Subject, it can store and emit different kinds of values:
  
  // BehaviorSubject: Remembers the last emitted value and emits it immediately to new subscribers.
  // ReplaySubject: Keeps a buffer of emitted values and can replay them to new subscribers.
  // AsyncSubject: Emits only the last value when the observable completes.
  

  // Inject the Router and RecipeServiceService
  constructor(private router: Router,
              private recipeService: RecipeServiceService,
              public starService: StarService
              ){
    
    }
  
  // Navigate to the recipe detail page
  goToRecipeDetail(recipe: any) {
    // Navigate to RecipeDetailComponent with the recipe's ID as parameter
    this.router.navigate(['/recipe-detail', recipe.id]);
  }

  ngOnInit() {
    // Initialize recipes and filteredRecipes when the component loads
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
        this.filteredRecipes = [...this.scoreRecipe]
        
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {
          this.scoreRecipe.forEach(recipe => {
            this.getRating(recipe.id);
          });


      }

    
    })

    // Subscribe to changes in the search term input
    this.searchInputSubject
      .pipe(debounceTime(200), distinctUntilChanged())
      .subscribe(searchTerm => {
        // Apply filtering when the search term changes
        this.filterRecipes(searchTerm);
      });
  }

  // Filter recipes based on the search term
  filterRecipes(searchTerm: string) {
    
    this.filteredRecipes = this.scoreRecipe.filter(recipe =>
      recipe.title.toLowerCase().includes(searchTerm.toLowerCase())
    );
  }

  // Event handler for search input changes
  onSearchInputChanged(event: Event) {
    // Push the updated search term to the subject
    this.searchInputSubject.next(this.searchTerm);
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
