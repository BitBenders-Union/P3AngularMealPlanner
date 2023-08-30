import { Component } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { Ingredient, Recipe } from '../Interfaces';
import { WeekScheduleService } from '../week-schedule.service'; // Import the service

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  // added animations
  animations: [
    trigger('shoppingListAnimation', [
      state('visible', style({ opacity: 1, maxHeight: '1000px' })),
      state('hidden', style({ opacity: 0, maxHeight: 0, overflow: 'hidden' })),
      transition('hidden => visible', animate('300ms ease-in-out')),
    ]),
  ],
})
export class DashboardComponent {
  shoppingListIngredients: Ingredient[] = []; // List of ingredients we send to the shoppingList componenet
  showShoppingList = true;

  constructor(private weekScheduleService: WeekScheduleService) {} // Inject the service


  // This function is called when the shopping list needs to be updated with new ingredients.
  // It takes an array of Ingredient objects as a parameter.

  onShoppingListUpdated(ingredients: Ingredient[]) {

    // Loop through the ingredients to check if they are already present in the shopping list
    ingredients.forEach(newIngredient => {
      
      // Find the index of the existing ingredient in the shopping list
      // If the ingredient is not in the shopping list, the index will be -1
      const existingIngredientIndex = this.shoppingListIngredients.findIndex(
        existingIngredient => existingIngredient.name === newIngredient.name
      );

      // If the ingredient already exists in the shopping list
      if (existingIngredientIndex !== -1) {
        
        // Add the amount of the new ingredient to the existing ingredient's amount
        this.shoppingListIngredients[existingIngredientIndex].amounts.value += newIngredient.amounts.value;
      } else {
        
        // If the ingredient is not already in the shopping list, create a new entry
        const shoppingListIngredient: Ingredient = {
          name: newIngredient.name,
          amounts: { ...newIngredient.amounts, value: newIngredient.amounts.value },
        };
        
        // Add the new ingredient to the shopping list
        this.shoppingListIngredients.push(shoppingListIngredient);
      }
    });

    // Remove ingredients with 0 amount from the shopping list
    this.shoppingListIngredients = this.shoppingListIngredients.filter(
      ingredient => ingredient.amounts.value > 0
    );
  }

  // the bookmark component emits a bool telling if the bookmark component is expanded or not
  // this function toggles the shopping list hiding it when the bookmark component is expanded
  toggleShoppingList() {
    this.showShoppingList = !this.showShoppingList;
  }
}
