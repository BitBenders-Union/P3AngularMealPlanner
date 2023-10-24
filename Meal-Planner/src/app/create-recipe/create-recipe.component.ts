import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, FormControl, ReactiveFormsModule, AbstractControl  } from '@angular/forms';
import { Recipe, RecipeDTO } from '../Interfaces';
import { RecipeServiceService } from '../service/recipe-service.service';
import { LoginService } from '../service/login.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';


@Component({
  selector: 'app-create-recipe',
  templateUrl: './create-recipe.component.html',
  styleUrls: ['./create-recipe.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class CreateRecipeComponent implements OnInit {
  form: FormGroup; // the form binding to the html, this is what we use to get the values from the form

  categories: string[] = [];
  initialCategories: string[] = [];
  allCateogries: string[] = [];

  private searchInputSubject = new Subject<string>();

  loading: boolean = false; // conrol the spinner

  // constructor initializes the form with default values, for now they are test values.
  // since ingredients and instructions are arrays, we need to initialize them as an array instead of single values
  // we also inject the createRecipeService so we can fetch from our API
  constructor(private formBuilder: FormBuilder,
    private recipeService: RecipeServiceService,
    private tokenService: LoginService,
    private router: Router
    ) {
    this.form = this.formBuilder.group({
      title: null,
      category: null,
      description: null,
      prepTime: null,
      cookTime: null,
      servings: null,
      rating: null,
      ingredients: this.formBuilder.array([]),
      instructions: this.formBuilder.array([]),
    });

    // this is to initialize the form with one ingredient and one instruction
    // since before we initialize the form with empty arrays, we need to add one ingredient and one instruction
    // for it to render the form properly
    this.addIngredients(0);
    this.addInstruction(0);
  }
  ngOnInit(): void {
    this.recipeService.getCategories().subscribe({
      next: categories => {
        this.allCateogries = categories.map(category => category.categoryName);
      },
      error: error => console.error('There was an error!', error),
      complete: () => {
        
        if(this.allCateogries.length >= 5){
          this.categories = this.GetRandomElementsFromArray(this.allCateogries, 5);
          this.initialCategories = [...this.categories];
        }
        else{
          this.categories = [...this.allCateogries];
          this.initialCategories = [...this.categories];
        }

        this.searchInputSubject
        .pipe(debounceTime(200), distinctUntilChanged())
        .subscribe(searchTerm => {
          // Apply filtering when the search term changes
          this.filterCategories(searchTerm);
        });

      }
    });

  }

  filterCategories(searchTerm: string) {
    if(!this.form.get('category')?.value)
    {
      this.categories = [...this.initialCategories];
      return;
    }
    this.categories = this.allCateogries.filter(cat =>
      cat.toLowerCase().includes(searchTerm.toLowerCase())
    );
  }

  onSearchInputChanged(event: Event) {
    this.searchInputSubject.next(this.form.get('category')?.value);
  }

  //Finds 5 random categories from the array and displays them in dropdown list
  GetRandomElementsFromArray(array: string[], numberOfElements: number): string[] {
    const shuffledArray = [...array];

    // Fisher-Yates shuffle algorithm to shuffle the array
    for (let i = shuffledArray.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [shuffledArray[i], shuffledArray[j]] = [shuffledArray[j], shuffledArray[i]];
    }

    // Take the first 'count' elements
    return shuffledArray.slice(0, numberOfElements);
  }

  // this is to get the ingredients and instructions from the form
  // remember the ingredients is an array, so this is a way we can access it.
  // we also store all the ingredients here, so we can easily send it to the backend
  get ingredients(): FormArray {
    return this.form.get('ingredients') as FormArray;
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

  addInstruction(index: number) {
    this.instructions.controls.splice(index + 1, 0, this.createInstructionFormGroup())
  }

  removeInstruction(index: number) {
    if (this.instructions.length > 1) {
      this.instructions.controls.splice(index, 1);
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

      const formattedRecipe: RecipeDTO = {
        Title: this.form.get('title')?.value,
        Description: this.form.get('description')?.value ,
        Category: {
          CategoryName: this.form.get('category')?.value
        },
        PreparationTimes: { 
          Minutes: this.form.get('prepTime')?.value
        },
        CookingTimes: { 
          Minutes: this.form.get('cookTime')?.value
         },
        Servings: { 
          Quantity: this.form.get('servings')?.value
        },
        Ratings: [{
          Score: this.form.get('rating')?.value
        }],
        Ingredients: this.ingredients.controls.map(control => ({
          Name: control.get('name')?.value,
          Amount: {
            Quantity: control.get('amounts')?.value,
          },
          Unit: {
            Measurement: control.get('unit')?.value
          }
        })),
        Instructions: this.instructions.controls.map(control => ({
          Text: control.get('text')?.value
        })),
        User: {
          Id: this.tokenService.getIdFromToken(),
          Username: this.tokenService.getUsernameFromToken()
        }
      };


      console.log(formattedRecipe);
      this.recipeService.createRecipe(formattedRecipe).subscribe({
        next: response => {
          this.form.reset();
          this.router.navigate([`/recipe-detail/${response}`])
        },
        error: error => console.error('There was an error!', error)
        });
    } 
    else {
      console.log('Form is invalid');
    }

    this.loading = false
    this.markAllAsTouched(this.form)
    this.markFormArrayControlsAsTouched(this.form.get('ingredients') as FormArray);
    this.markFormArrayControlsAsTouched(this.form.get('instructions') as FormArray);

  }
  
  markAllAsTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      if (control instanceof FormGroup) {
        this.markAllAsTouched(control);
      } else {
        control.markAsTouched();
      }
    });
  }

  markFormArrayControlsAsTouched(formArray: FormArray) {
    formArray.controls.forEach((control: AbstractControl) => {
      if (control instanceof FormGroup) {
        Object.values(control.controls).forEach(ctrl => {
          ctrl.markAsTouched();
        });
      }
    });
  }
  
  

  // this is to reset the form
  // since the button with type reset, deletes all inputs but doesn't take into account the number of arrays
  // we also need to reset the arrays to 1
  onReset(){
    for (let i = this.ingredients.length; i >= 1; i--) {
      this.removeIngredient(i);
    }

    for (let i = this.instructions.length; i >= 1; i--) {
      this.removeInstruction(i);
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

  removeIngredient(index: number) {
    if(this.ingredients.length > 1){
      this.ingredients.controls.splice(index, 1);
    }
  }

  
  // this is to add ingredients to the form
  // it pushes the data to the last index of the array
  addIngredients(index: number) {
    this.ingredients.controls.splice(index + 1, 0, this.createIngredientFormGroup())
  }

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.ingredients.controls, event.previousIndex, event.currentIndex);
  }


  dropInstruction(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.instructions.controls, event.previousIndex, event.currentIndex);
  }
}
