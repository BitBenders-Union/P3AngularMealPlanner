import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, FormControl, ReactiveFormsModule  } from '@angular/forms';
import { CreateRecipeService } from '../create-recipe.service';
import { Recipe, CreateRecipe } from '../Interfaces';

@Component({
  selector: 'app-create-recipe',
  templateUrl: './create-recipe.component.html',
  styleUrls: ['./create-recipe.component.css']
})
export class CreateRecipeComponent {
  form: FormGroup;
  categories: string[] = ['Breakfast', 'Lunch', 'Dinner', 'Dessert', 'Snacks'];
  loading: boolean = false;

  constructor(private formBuilder: FormBuilder,
    private createRecipeService: CreateRecipeService 
    ) {
    this.form = this.formBuilder.group({
      title: 'test',
      category: 'test',
      description: 'test',
      prepTime: '123',
      cookTime: '123',
      servings: '123',
      rating: '1.5',
      ingredients: this.formBuilder.array([]),
      instructions: this.formBuilder.array([])
    });

    this.addIngredients();
    this.addInstruction();
  }

  get ingredients(): FormArray {
    return this.form.get('ingredients') as FormArray;
  }

  addIngredients() {
    this.ingredients.push(this.createIngredientFormGroup());
  }

  removeIngredients() {
    if (this.ingredients.length > 1) {
      this.ingredients.removeAt(this.ingredients.length - 1);
    }
  }

  createIngredientFormGroup() {
    return new FormGroup({
      name: new FormControl(''),
      amounts: new FormControl(''),
      unit: new FormControl('')
    });
  }

  get instructions(): FormArray {
    return this.form.get('instructions') as FormArray;
  }

  addInstruction() {
    this.instructions.push(this.createInstructionFormGroup());
  }

  removeLastInstruction() {
    if (this.instructions.length > 1) {
      this.instructions.removeAt(this.instructions.length - 1);
    }
  }

  createInstructionFormGroup() {
    return new FormGroup({
      text: new FormControl('')
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.loading = true;

      const formattedRecipe: CreateRecipe = {
        title: this.form.get('title')?.value,
        category: this.form.get('category')?.value ,
        description: this.form.get('description')?.value ,
        preparationTime: this.form.get('prepTime')?.value,
        cookingTime: this.form.get('cookTime')?.value,
        servings: this.form.get('servings')?.value,
        rating: this.form.get('rating')?.value,
        ingredients: this.ingredients.controls.map(control => ({
          name: control.get('name')?.value,
          amounts: {
            value: control.get('amounts')?.value,
            unit: control.get('unit')?.value
          }
        })),
        instructions: this.instructions.controls.map(control => ({
          text: control.get('text')?.value
        })),
        deleted: false
      };

      this.createRecipeService.createRecipe(formattedRecipe).subscribe({
        next: response => {
          console.log('Recipe created successfully', response);
          this.form.reset();
        },
        error: error => console.error('There was an error!', error),
        complete: () => {
          this.loading = false
          this.onReset();
          }
        });
    } else {
      console.log('Form is invalid');
    }

  }
  
  onReset(){
    for (let i = this.ingredients.length; i >= 1; i--) {
      this.removeIngredients();
    }

    for (let i = this.instructions.length; i >= 1; i--) {
      this.removeLastInstruction();
    }

  }

  validateRating() {
    const ratingControl = this.form.get('rating');

    if (ratingControl!.value < 0) {
      ratingControl!.setValue(0);
    } else if (ratingControl!.value > 5) {
      ratingControl!.setValue(5);
    }
  }

  aboveZero(inputElement: EventTarget, controlName: string){
    const value = parseFloat((inputElement as HTMLInputElement).value);
    if (value < 0) {
      this.form.get(controlName)?.setValue(0);
    }
  }


  aboveZeroIngredient(){
    const value = parseFloat(this.ingredients.controls[0].get('amounts')?.value);

    if (value < 0) this.ingredients.controls[0].get('amounts')?.setValue(0);

  }

}