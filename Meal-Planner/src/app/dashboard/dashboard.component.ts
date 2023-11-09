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
        console.log("dashboard user")
      })

  }



  // This function is called when the shopping list needs to be updated with new ingredients.
  // It takes an array of Ingredient objects as a parameter.

  onShoppingListUpdated(ingredients: Ingredient[]) {

    // to add each ingredient we first need to check if they already exist in the shopping list
    // so we first need to make a foreach on the ingredients array
    ingredients.forEach((ingredient) => {

      // check if exist
      const existingIngredient = this.shoppingListIngredients.findIndex(x =>
        x.name === ingredient.name && x.unit.measurement === ingredient.unit.measurement
      );
      
      // if ingredient doesn't exist in the shopping list
      if(existingIngredient == -1)
      {
        // add it to the shopping list
        this.shoppingListIngredients.push(ingredient);

      }
      else{
        // if it does exist in the shopping list
        // we need to add the quantity of the ingredient to the existing ingredient
        this.shoppingListIngredients[existingIngredient].amount.quantity += ingredient.amount.quantity;

        // after ingredient is updated, we need to check if it is 0. if it is 0, we need to remove it from the shopping list
        if(this.shoppingListIngredients[existingIngredient].amount.quantity == 0)
        {
          // remove the ingredient from the shopping list
          this.shoppingListIngredients.splice(existingIngredient, 1);

        }

      }
    });


  }
  

 

  // the bookmark component emits a bool telling if the bookmark component is expanded or not
  // this function toggles the shopping list hiding it when the bookmark component is expanded
  toggleShoppingList() {
    this.showShoppingList = !this.showShoppingList;
  }
}
