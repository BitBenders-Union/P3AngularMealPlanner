import { Component, OnInit } from '@angular/core';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { Ingredient, Recipe } from '../Interfaces';
import { WeekScheduleService } from '../service/week-schedule.service'; // Import the service
import { UserStoreService } from '../service/user-store.service';
import { LoginService } from '../service/login.service';

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
export class DashboardComponent implements OnInit{
  shoppingListIngredients: Ingredient[] = []; // List of ingredients we send to the shoppingList componenet
  showShoppingList = true;

  public userName: string = "";
  public userId: number = 0;
  public testthing: any[] = [];

  constructor(private weekScheduleService: WeekScheduleService, private userStore: UserStoreService, private auth: LoginService) {} // Inject the service

  ngOnInit(){
    
      this.userStore.getUserFromStore()
      .subscribe(val =>{ 
        let userNameFromToken = this.auth.getUsernameFromToken();
        this.userName = val || userNameFromToken;

      })

  }

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
        this.shoppingListIngredients[existingIngredientIndex].amount.quantity += newIngredient.amount.quantity;
      } else {
        
        // If the ingredient is not already in the shopping list, create a new entry
        const shoppingListIngredient: Ingredient = {
          id: newIngredient.id,
          name: newIngredient.name,
          order: newIngredient.order,
          amount: { ...newIngredient.amount, quantity: newIngredient.amount.quantity, },
          unit: { ...newIngredient.unit, measurement: newIngredient.unit.measurement },
        };
        
        // Add the new ingredient to the shopping list
        this.shoppingListIngredients.push(shoppingListIngredient);
      }
    });

    // Remove ingredients with 0 amount from the shopping list
    this.shoppingListIngredients = this.shoppingListIngredients.filter(
      ingredient => ingredient.amount.quantity > 0
    );
  }

  // the bookmark component emits a bool telling if the bookmark component is expanded or not
  // this function toggles the shopping list hiding it when the bookmark component is expanded
  toggleShoppingList() {
    this.showShoppingList = !this.showShoppingList;
  }
}
