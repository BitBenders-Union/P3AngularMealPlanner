import { Component, Input } from '@angular/core';
import { Ingredient } from '../Interfaces';
@Component({
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.component.html',
  styleUrls: ['./shopping-list.component.css'],
})
export class ShoppingListComponent {
  @Input() ingredients: Ingredient[] = [];
}
