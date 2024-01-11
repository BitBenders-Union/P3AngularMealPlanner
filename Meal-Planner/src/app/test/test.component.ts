import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component } from '@angular/core';

interface Ingredient {
  name: string;
  amount: string;
  units: string;
}

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent {

  ingredients: Ingredient[] = []; // Initialize as an array of Ingredient objects

  addIngredient() {
    this.ingredients.push({ name: '', amount: '', units: '' });
  }

  removeIngredient(index: number) {
    this.ingredients.splice(index, 1);
  }

  moveUp(index: number) {
    if (index > 0) {
      const temp = this.ingredients[index];
      this.ingredients[index] = this.ingredients[index - 1];
      this.ingredients[index - 1] = temp;
    }
  }

  moveDown(index: number) {
    if (index < this.ingredients.length - 1) {
      const temp = this.ingredients[index];
      this.ingredients[index] = this.ingredients[index + 1];
      this.ingredients[index + 1] = temp;
    }
  }

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.ingredients, event.previousIndex, event.currentIndex);
  }

}
