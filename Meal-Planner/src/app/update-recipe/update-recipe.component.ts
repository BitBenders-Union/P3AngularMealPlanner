import { Component, OnInit } from '@angular/core';
import { CreateAmounts, CreateIngredient, CreateInstruction, CreateRecipe, Recipe } from '../Interfaces';
import { RecipeServiceService } from '../recipe-service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, FormControl, AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-update-recipe',
  templateUrl: './update-recipe.component.html',
  styleUrls: ['./update-recipe.component.css']
})
export class UpdateRecipeComponent implements OnInit{
  recipe: CreateRecipe | undefined;
  recipeId: number | undefined;

  updateForm: FormGroup;

  categories: string[] = ['Breakfast', 'Lunch', 'Dinner', 'Dessert', 'Snacks'];

  get instructions(): FormArray {
    return this.updateForm.get('instructions') as FormArray;
  }
  
  get ingredients(): FormArray {
    return this.updateForm.get('ingredients') as FormArray;
  }
  

  

  constructor(
    private route: ActivatedRoute, 
    private recipeService: RecipeServiceService,
    private formBuilder: FormBuilder,
    private router: Router
    ) 
    { 
      this.updateForm = this.formBuilder.group({
        rating: [0],
        instructions: this.formBuilder.array([]),
        ingredients: this.formBuilder.array([])
      });
    }

  ngOnInit(): void {

    this.route.paramMap.subscribe(params => {
      this.recipeId = Number(params.get('id'));

      if(!isNaN(this.recipeId)){

        this.recipeService.getRecipeById(this.recipeId).subscribe(recipe => {
          
          this.recipe = recipe;

          this.updateForm.addControl('title', new FormControl(this.recipe.title));
          this.updateForm.addControl('category', new FormControl(this.recipe.category));
          this.updateForm.addControl('description', new FormControl(this.recipe.description));
          this.updateForm.addControl('preparationTime', new FormControl(this.recipe.preparationTime));
          this.updateForm.addControl('cookingTime', new FormControl(this.recipe.cookingTime));
          this.updateForm.addControl('servings', new FormControl(this.recipe.servings));
          this.updateForm.addControl('rating', new FormControl(this.recipe.rating));
          
          if (this.recipe.ingredients && this.recipe.ingredients.length > 0) {
            this.recipe.ingredients.forEach((ingredient, index) => {
              const nameControl = new FormControl(ingredient.name);
              const valueControl = new FormControl(ingredient.amounts.value);
              const unitControl = new FormControl(ingredient.amounts.unit);
        
              const ingredientGroup = this.formBuilder.group({
                name: nameControl,
                value: valueControl,
                unit: unitControl

              });
        
              this.ingredients.push(ingredientGroup);
            });
          }
        
          if (this.recipe.instructions && this.recipe.instructions.length > 0) {

            this.recipe.instructions.forEach((instruction, index) => {

              const textControl = new FormControl(instruction.text);
              const instructionGroup = this.formBuilder.group({
                text: textControl
              });
              this.instructions.push(instructionGroup);

            });
          }

          this.updateForm.addControl('deleted', new FormControl(this.recipe.deleted));

        });
      }
    });  
  }




  addIngredient(): void {
    this.ingredients.push(this.createIngredientGroup('', '', ''));
  }

  addInstruction(): void {
    this.instructions.push(this.createInstructionGroup(''));
  }

  removeIngredient(): void {
    if (this.ingredients.length > 0) {
      this.ingredients.removeAt(this.ingredients.length - 1);
    }
  }

  removeInstruction(): void {
    if (this.instructions.length > 0) {
      this.instructions.removeAt(this.instructions.length - 1);
    }
  }

  private createIngredientGroup(name: string, value: string, unit: string): FormGroup {
    return this.formBuilder.group({
      name: new FormControl(name),
      value: new FormControl(value),
      unit: new FormControl(unit)
    });
  }

  private createInstructionGroup(text: string): FormGroup {
    return this.formBuilder.group({
      text: new FormControl(text)
    });
  }



  updateRecipe(): void {
    if (this.updateForm.valid) {
      const formData = this.updateForm.value;

      const ingredients: CreateIngredient[] = formData.ingredients.map((ingredient: any) => {
        const amounts: CreateAmounts = {
          value: ingredient.value,
          unit: ingredient.unit
        };

        return {
          name: ingredient.name,
          amounts: amounts
        };
      });

      const instructions: CreateInstruction[] = formData.instructions.map((instruction: any) => {
        return {
          text: instruction.text
        };
      });

      const updatedRecipe: CreateRecipe = {
        title: formData.title,
        category: formData.category,
        description: formData.description,
        preparationTime: formData.preparationTime,
        cookingTime: formData.cookingTime,
        servings: formData.servings,
        rating: formData.rating,
        ingredients: ingredients,
        instructions: instructions,
        deleted: formData.deleted
      };
      console.log(updatedRecipe);


      this.recipeService.updateRecipe(updatedRecipe, this.recipeId!).subscribe(() => {
        console.log('recipe updated');
        
        // navigate back to search component
        this.router.navigate(['/search']);
      });
      
      


    }
  }

  
  // controls what the user can input for rating
  validateRating() {
    const ratingControl = this.updateForm.get('rating');

    if (ratingControl!.value < 0) {
      ratingControl!.setValue(0);
    } else if (ratingControl!.value > 5) {
      ratingControl!.setValue(5);
    }
  }

  goBack(){
    this.router.navigate(['/recipe-detail/' + this.recipeId]);
  }

}
