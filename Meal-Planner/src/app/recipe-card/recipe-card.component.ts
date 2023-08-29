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

  @Input() recipe: Recipe | null = null; // Add an @Input property

  recipes: Recipe[] = [];

  
  constructor(private recipeService: RecipeServiceService, public starService: StarService) { }
  ngOnInit(): void {
    this.getRecipes();
  }

getRecipes(): void {
  this.recipeService.getRecipes()
    .subscribe(recipes => this.recipes = recipes);
    
  
}


getRatingStars(rating: number): (boolean | string)[] {
  if (rating === null || isNaN(rating)) {
    return [];
  }

  const fullStars = Math.floor(rating);
  const halfStar = rating % 1 >= 0.5;

  const starsArray = new Array(fullStars).fill(true);
  if (halfStar) {
    starsArray.push('half');
  }

  return starsArray;
}



}
