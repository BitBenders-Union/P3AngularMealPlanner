import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Recipe, Ingredient, RecipeScheduleDTO, User } from '../Interfaces';
import { WeekScheduleService } from '../service/week-schedule.service';
import { StarService } from '../service/star.service';
import { RecipeServiceService } from '../service/recipe-service.service';
import { UserStoreService } from '../service/user-store.service';
import { LoginService } from '../service/login.service';

@Component({
  selector: 'app-week-schedule',
  templateUrl: './week-schedule.component.html',
  styleUrls: ['./week-schedule.component.css'],
})
export class WeekScheduleComponent implements OnInit {
  shoppingListIngredients: Ingredient[] = [];
  @Output() shoppingListUpdated = new EventEmitter<Ingredient[]>(); // Event emitter for shopping list updates

  days = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']; // Weekdays
  timeSlots = ['Breakfast', 'Lunch', 'Dinner']; // Time slots for the schedule

  isDragging = false; // Flag to indicate dragging state

  public user: User = {
    id: 0,
    username: ''
  }

  // Holds the recipes/events for each time slot and day
  cellContents: Recipe[][] = Array.from({ length: this.timeSlots.length }, () =>
    Array(this.days.length).fill(null)
  );

  ratingCellContents: number[][] = Array.from({ length: this.timeSlots.length }, () =>
  Array(this.days.length).fill(null)
);


  schedule: RecipeScheduleDTO[] = [];
  savedRecipes: Recipe[] = [];

  

  constructor(private weekScheduleService: WeekScheduleService,
    public recipeService: RecipeServiceService,
    public starService: StarService,
    private userStore: UserStoreService, 
    private auth: LoginService,
    
    ) {}

  ngOnInit(): void {

    this.RetrieveUser()

  }

  // seems like the complete object in the subscribe doesn't work
  RetrieveUser(): void {

    this.userStore.getUserFromStore().subscribe({
      next: user => {
        this.user.username = user;
        if(this.user.username === ''){
          this.user.username = this.auth.getUsernameFromToken();
        }
        this.RetrieveId();
      },
      error: error => console.error('There was an error!', error)
    });

  }

  RetrieveId(): void {
    this.userStore.getIdFromStore().subscribe({
      next: id => {
        this.user.id = id;
        if(this.user.id === 0){
          this.user.id = this.auth.getIdFromToken();
        }
          this.getScheduleData(this.user.id);
      },
      error: error => console.error('There was an error!', error)
    });

  }


    // Handles the dropping of recipes into time slots
  Drop(event: CdkDragDrop<Recipe[]>, rowIndex: number, colIndex: number): void {
    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    } else {
      const recipe = event.item.data;
      console.log("recipe: ");
      console.log(recipe);
      const newRecipe: Recipe = { ...recipe, ingredients: [...recipe.ingredients] };
      console.log(newRecipe);
      if (!this.cellContents[colIndex]) {
        this.cellContents[colIndex] = [];
      }

      // Update the cellContents with the new recipe
      this.cellContents[colIndex][rowIndex] = newRecipe;
      this.recipeService.GetRecipeRating(newRecipe.id!).subscribe({
        next: (data) => {
          this.ratingCellContents[colIndex][rowIndex] = data.score;
        },
        error: (err) => console.log(err),
        complete: () => {
          // Emit ingredients to update shopping list
          this.shoppingListUpdated.emit(newRecipe.ingredients);
          // Save the updated cellContents to db
          this.saveCellContents(rowIndex, colIndex, newRecipe.id);
        }
      });

     
    }
  }
  
  // get week scheduel data from user id
  // store it in schedule
  // populate cellcontents with recipes from schedule
  // fetch recipes from database when needed
  // emit shopping list updates when needed

  initializeCells(): void {
    // Loop through schedule
    this.schedule.forEach((entry) => {
      // Check if recipeId is not null
      if (entry.recipeId !== null) {
        
        let newRecipe: Recipe;

        this.recipeService.GetRecipeRating(entry.recipeId!).subscribe({
          next: (data) => {
            this.ratingCellContents[entry.row][entry.column] = data.score;

          },
          error: (err) => console.log(err)
        });
        
        this.recipeService.getRecipeById(entry.recipeId!).subscribe({
          next: (data) => {
            newRecipe = data;
            this.cellContents[entry.row][entry.column] = newRecipe;
            this.shoppingListUpdated.emit(newRecipe.ingredients);
          },
          error: (err) => console.log(err)
        });

      }
    });

  }



// Handles the removal of a recipe from the schedule
// rowIndex: The index of the row (weekday) where the recipe is located
// colIndex: The index of the column (time slot) where the recipe is located
deleteRecipe(rowIndex: number, colIndex: number): void {
  // Get the recipe to be deleted from the cellContents
  const deletedRecipe = this.cellContents[colIndex][rowIndex];

  if (deletedRecipe) {
    // Loop through ingredients of the deleted recipe
    deletedRecipe.ingredients.forEach((ingredient) => {
      // Emit an update to the shopping list, subtracting the ingredient amounts
      // The ingredient's value is negated to indicate subtraction
      this.shoppingListUpdated.emit([
        { ...ingredient, amount: { ...ingredient.amount, quantity: -ingredient.amount.quantity } },
      ]);
    });

    // Delete the recipe from the cellContents
    this.cellContents[colIndex][rowIndex] = null as unknown as Recipe; // wtf is this
    this.ratingCellContents[colIndex][rowIndex] = null as unknown as number;

    // Save the updated cellContents to the server
    this.saveCellContents(rowIndex, colIndex, undefined);
  }
}


 // Saves the cellContents to the server
 private saveCellContents(rowIndex: number, colIndex: number, myRecipeId?: number): void {
    const updatedData: RecipeScheduleDTO = {
      row: colIndex,
      column: rowIndex,
      recipeId: myRecipeId,
      user: this.user
    };
    this.weekScheduleService.updateData(updatedData).subscribe();
  }

  // get week schedule data from user id
  // stores it in test
  getScheduleData(userID: number): void {
    
    this.weekScheduleService.getWeekScheduleData(userID)
        .subscribe({
            next: (data) => {
              this.schedule = data
              this.initializeCells()
            },
            error: (err) => console.log(err)
        });
  }

}
