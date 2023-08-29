import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Router } from '@angular/router';
import { RecipeServiceService } from '../recipe-service.service';
import { Recipe } from '../Interfaces';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
})
export class SearchComponent implements OnInit {
  recipes: Recipe[] = [];
  filteredRecipes: Recipe[] = [];

  searchTerm: string = '';

  private searchInputSubject = new Subject<string>();

  constructor(private router: Router, private recipeSerivce: RecipeServiceService) {
    
  }

  getRecipes(): void {
    this.recipeSerivce.getRecipes().subscribe(recipes => {
      this.recipes = recipes;
      this.filteredRecipes = [...this.recipes];
    });

  }

  goToRecipeDetail(recipe: Recipe) {
    // Navigate to RecipeDetailComponent with the recipe's title as parameter
    this.router.navigate(['/recipe-detail', recipe.id]);
  }

  ngOnInit() {
    this.getRecipes();

    this.searchInputSubject
      .pipe(debounceTime(200), distinctUntilChanged())
      .subscribe(searchTerm => {
        this.filterRecipes(searchTerm);
      });
  }

  filterRecipes(searchTerm: string) {
    this.filteredRecipes = this.recipes.filter(recipe =>
      recipe.title.toLowerCase().includes(searchTerm.toLowerCase())
    );
  }

  onSearchInputChanged(event: Event) {
    this.searchInputSubject.next(this.searchTerm);
  }

}
