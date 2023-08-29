import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { RecipeServiceService } from '../recipe-service.service'; // Import your service
import { Recipe } from '../Interfaces';

@Component({
  selector: 'app-forms-test',
  templateUrl: './forms-test.component.html',
  styleUrls: ['./forms-test.component.css']
})
export class FormsTestComponent implements OnInit {
  updateForm: FormGroup;
  recipe: Recipe | undefined; // Initialize ingredients as an empty array

  constructor(private formBuilder: FormBuilder, private recipeService: RecipeServiceService) {
    this.updateForm = this.formBuilder.group({});
  }

  ngOnInit(): void {
    this.recipeService.getRecipeById(1006).subscribe(recipe => {
      this.recipe = recipe;

      this.recipe.ingredients.forEach((ingredient, index) => {
        const nameControl = new FormControl(ingredient.name);
        const valueControl = new FormControl(ingredient.amounts.value);
        const unitControl = new FormControl(ingredient.amounts.unit);

        // Add controls to the form group
        this.updateForm.addControl(`name_${index}`, nameControl);
        this.updateForm.addControl(`value_${index}`, valueControl);
        this.updateForm.addControl(`unit_${index}`, unitControl);
      });
    });
  }
}
