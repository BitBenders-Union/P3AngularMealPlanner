import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, FormControl, ReactiveFormsModule  } from '@angular/forms';
import { Recipe, CreateRecipe } from '../Interfaces';
import { RecipeServiceService } from '../recipe-service.service';

@Component({
  selector: 'app-create-recipe',
  templateUrl: './create-recipe.component.html',
  styleUrls: ['./create-recipe.component.css']
})
export class CreateRecipeComponent {
  form: FormGroup; // the form binding to the html, this is what we use to get the values from the form
  categories: string[] = ['Breakfast', 'Lunch', 'Dinner', 'Dessert', 'Snacks']; // these control the dropdown menu for the category, 
                                                                                // we can add more categories later if we want, 
                                                                                // we can also make it not hardcoded and fetch from a pre-created database / API
  loading: boolean = false; // conrol the spinner

  // constructor initializes the form with default values, for now they are test values.
  // since ingredients and instructions are arrays, we need to initialize them as an array instead of single values
  // we also inject the createRecipeService so we can fetch from our API
  constructor(private formBuilder: FormBuilder,
    private recipeService: RecipeServiceService
    ) {
    this.form = this.formBuilder.group({
      title: 'test',
      category: 'test',
      description: 'test',
      prepTime: 123,
      cookTime: 123,
      servings: 123,
      rating: 1.5,
      ingredients: this.formBuilder.array([]),
      instructions: this.formBuilder.array([])
    });

    // this is to initialize the form with one ingredient and one instruction
    // since before we initialize the form with empty arrays, we need to add one ingredient and one instruction
    // for it to render the form properly
    this.addIngredients();
    this.addInstruction();
  }

  // this is to get the ingredients and instructions from the form
  // remember the ingredients is an array, so this is a way we can access it.
  // we also store all the ingredients here, so we can easily send it to the backend
  get ingredients(): FormArray {
    return this.form.get('ingredients') as FormArray;
  }

  // this is to add ingredients to the form
  // it pushes the data to the last index of the array
  addIngredients() {
    this.ingredients.push(this.createIngredientFormGroup());
  }

  // this is to remove the last ingredient from the form
  // we need to check if the length is greater than 1, because we don't want to have 0 ingredients
  // this also ensures the form renders properly
  removeIngredients() {
    if (this.ingredients.length > 1) {
      this.ingredients.removeAt(this.ingredients.length - 1);
    }
  }

  // this is to create the form group for the ingredients
  // we need to create a form group for each ingredient, because each ingredient is an array
  // this is all to reflect our interface and make it easier to post to the backend
  createIngredientFormGroup() {
    return new FormGroup({
      name: new FormControl(''),
      amounts: new FormControl(''),
      unit: new FormControl('')
    });
  }

  // this is to get the instructions from the form
  // works like the ingredients
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


  // Handles the submit button
  // we need to check if the form is valid, if it is, we can send it to the backend
  // we also need to format the data to match our interface

  // we use the CreateRecipe interface since this is excluding the ID's
  // the database created the ID's automatically, so we don't need to worry about that
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

      this.recipeService.createRecipe(formattedRecipe).subscribe({
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
  
  // this is to reset the form
  // since the button with type reset, deletes all inputs but doesn't take into account the number of arrays
  // we also need to reset the arrays to 1
  onReset(){
    for (let i = this.ingredients.length; i >= 1; i--) {
      this.removeIngredients();
    }

    for (let i = this.instructions.length; i >= 1; i--) {
      this.removeLastInstruction();
    }

  }

  // this is to restrict the rating to be between 0 and 5
  // if you try to type a number that is less than 0, it will automatically set it to 0
  // if you try to type a number that is greater than 5, it will automatically set it to 5
  validateRating() {
    const ratingControl = this.form.get('rating');

    if (ratingControl!.value < 0) {
      ratingControl!.setValue(0);
    } else if (ratingControl!.value > 5) {
      ratingControl!.setValue(5);
    }
  }

  // this is to restrict a given controller to be greater than 0
  aboveZero(inputElement: EventTarget, controlName: string){
    const value = parseFloat((inputElement as HTMLInputElement).value);
    if (value < 0) {
      this.form.get(controlName)?.setValue(0);
    }
  }

  // this is to restrict the ingredient amount to be greater than 0
  aboveZeroIngredient(){
    const value = parseFloat(this.ingredients.controls[0].get('amounts')?.value);

    if (value < 0) this.ingredients.controls[0].get('amounts')?.setValue(0);

  }

}