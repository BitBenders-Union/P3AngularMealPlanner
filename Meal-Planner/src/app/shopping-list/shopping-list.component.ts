import { Component, Input } from '@angular/core';
import { Ingredient } from '../Interfaces';
@Component({
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.component.html',
  styleUrls: ['./shopping-list.component.css'],
})
export class ShoppingListComponent {
  // takes in an array of ingredients as input
  @Input() ingredients: Ingredient[] = [];
}
