import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Recipe, Ingredient, WeekData } from '../Interfaces';
import { WeekScheduleService } from '../service/week-schedule.service';
import { StarService } from '../service/star.service';
import { RecipeServiceService } from '../service/recipe-service.service';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
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

  // Holds the recipes/events for each time slot and day
  cellContents: Recipe[][] = Array.from({ length: this.timeSlots.length }, () =>
    Array(this.days.length).fill(null)
  );

  userID: number = 0;

  test: WeekData[] = [];
  savedRecipes: Recipe[] = [];


  constructor(private weekScheduleService: WeekScheduleService, public starService: StarService,
    private router:Router, private route: ActivatedRoute, private userStore: UserStoreService, private auth: LoginService ) {}

  ngOnInit(): void {
    // get user id from url
    this.userStore.getUserFromStore()
      .subscribe(val =>{
        let userIdFromToken = this.auth.getIdFromToken();
        this.userID = val || userIdFromToken;
        // console.log(this.userName);
      })
    this.loadCellContents(); // Load cell contents when the component initializes

    // get week schedule data from user id
    // remember to change to actual user ID instead of 1
    // this.getScheduleData(this.userID);
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
      const newRecipe: Recipe = { ...recipe, ingredients: [...recipe.ingredients] };

      if (!this.cellContents[colIndex]) {
        this.cellContents[colIndex] = [];
      }

      // Update the cellContents with the new recipe
      this.cellContents[colIndex][rowIndex] = newRecipe;
      // this.cellContents[colIndex][rowIndex].deleted = false;

      // Emit ingredients to update shopping list
      this.shoppingListUpdated.emit(newRecipe.ingredients);

      // Save the updated cellContents
      this.saveCellContents();
    }
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

    // Mark the recipe as deleted by setting the 'deleted' property to true
    // deletedRecipe.deleted = true;

    // Save the updated cellContents to the server
    this.saveCellContents();
  }
}


 // Saves the cellContents to the server
 private saveCellContents(): void {
  this.weekScheduleService.updateData(this.cellContents).subscribe();
  }

  // Loads cellContents from the server
  private loadCellContents(): void {
    this.weekScheduleService.getData().subscribe((data: Recipe[][]) => {
      this.cellContents = data;
    });
  }


  // get week schedule data from user id
  // stores it in test
  // getScheduleData(userID: number): void {
  //   // console.log("dumb cunt id: ", userID)
  //   this.weekScheduleService.getWeekScheduleData(userID)
  //       .subscribe({
  //           next: (data) => {
  //             this.test = data
  //             const recipeIDs = data.map((item: WeekData) => item.recipeId);
  //             this.getRecipes(recipeIDs);
  //           },
  //           error: (err) => console.log(err),
  //       })
  // }

  // // gets recipes from an array of recipe ids
  // // is called in getScheduleData
  // // stores it in savedRecipes
  // getRecipes(recipIds: number[]): void{
  //   this.weekScheduleService.getScheduledRecipes(recipIds)
  //       .subscribe({
  //           next: (data) => {
  //             // console.log(data)
  //             this.savedRecipes = data;
  //           },
  //           error: (err) => console.log(err),
  //       })
  // }




}
