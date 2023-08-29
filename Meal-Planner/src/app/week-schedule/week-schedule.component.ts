import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Recipe, Ingredient } from '../Interfaces';
import { WeekScheduleService } from '../week-schedule.service';
import { StarService } from '../star.service';

@Component({
  selector: 'app-week-schedule',
  templateUrl: './week-schedule.component.html',
  styleUrls: ['./week-schedule.component.css'],
})
export class WeekScheduleComponent implements OnInit {
  shoppingListIngredients: Ingredient[] = [];
  @Output() shoppingListUpdated = new EventEmitter<Ingredient[]>();

  days = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
  timeSlots = ['Breakfast', 'Lunch', 'Dinner'];

  isDragging = false;

  cellContents: Recipe[][] = Array.from({ length: this.timeSlots.length }, () =>
    Array(this.days.length).fill(null)
  );

  constructor(private weekScheduleService: WeekScheduleService, public starService: StarService) {}

  ngOnInit(): void {
    this.loadCellContents();
  }

  Drop(
    event: CdkDragDrop<Recipe[]>,
    rowIndex: number,
    colIndex: number
  ): void {
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

      this.cellContents[colIndex][rowIndex] = newRecipe;
      this.cellContents[colIndex][rowIndex].deleted = false;

      this.shoppingListUpdated.emit(newRecipe.ingredients);
      this.saveCellContents();
    }
  }
  
  getRatingStars(rating: number): number[] {
    const fullStars = Math.floor(rating);
    const halfStar = rating % 1 >= 0.5 ? 1 : 0;
  
    return new Array(fullStars + halfStar);
  }

  deleteRecipe(rowIndex: number, colIndex: number): void {
    const deletedRecipe = this.cellContents[colIndex][rowIndex];

    if (deletedRecipe) {
      deletedRecipe.ingredients.forEach((ingredient) => {
        this.shoppingListUpdated.emit([
          { ...ingredient, amounts: { ...ingredient.amounts, value: -ingredient.amounts.value } },
        ]);
      });

      deletedRecipe.deleted = true; // Set deleted property to true
      this.saveCellContents();
    }
  }

  private saveCellContents(): void {
    this.weekScheduleService.updateData(this.cellContents).subscribe();
  }

  private loadCellContents(): void {
    this.weekScheduleService.getData().subscribe((data: Recipe[][]) => {
      this.cellContents = data;
    });
  }
}
