import { Component, Input, OnInit } from '@angular/core';
import { CdkDrag } from '@angular/cdk/drag-drop';
import { Recipe } from '../Interfaces';
import { RecipeServiceService } from '../recipe-service.service';
import { StarService } from '../star.service';



@Component({
  selector: 'app-recipe-card',
  templateUrl: './recipe-card.component.html',
  styleUrls: ['./recipe-card.component.css']
})
export class RecipeCardComponent implements OnInit{

  @Input() recipe: Recipe | null = null; // Input decorator to pass the recipe object to the component

  recipes: Recipe[] = []; // Initialize recipes as an empty array

  
  constructor(private recipeService: RecipeServiceService, public starService: StarService) { } // Inject the services

  ngOnInit(): void {
    this.getRecipes(); // method that fetches recipes from API
  }

getRecipes(): void {
  // fetch recipes from API using a subscription, should add error handling
  this.recipeService.getRecipes()
    .subscribe(recipes => this.recipes = recipes);
    
  
}

}
