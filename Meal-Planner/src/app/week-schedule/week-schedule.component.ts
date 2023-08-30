import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Recipe, Ingredient } from '../Interfaces';
import { WeekScheduleService } from '../week-schedule.service';
import { StarService } from '../star.service';
import { RecipeServiceService } from '../recipe-service.service';

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

  constructor(
    private weekScheduleService: WeekScheduleService, // WeekScheduleService for data handling
    private recipeService: RecipeServiceService, // RecipeService for fetching recipes
    public starService: StarService // StarService for rating stars
  ) {}


  ngOnInit(): void {
    this.loadCellContents(); // Load cell contents when the component initializes
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
      this.cellContents[colIndex][rowIndex].deleted = false;

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
        { ...ingredient, amounts: { ...ingredient.amounts, value: -ingredient.amounts.value } },
      ]);
    });

    // Mark the recipe as deleted by setting the 'deleted' property to true
    deletedRecipe.deleted = true;

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
}
