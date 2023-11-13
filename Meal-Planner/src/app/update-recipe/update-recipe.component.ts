import { Component, OnInit } from '@angular/core';
import { Amount, Unit, Ingredient, Recipe, Instruction, RecipeDTO, User } from '../Interfaces';
import { RecipeServiceService } from '../service/recipe-service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, FormControl, AbstractControl, Validators } from '@angular/forms';
import { LoginService } from '../service/login.service';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { UserStoreService } from '../service/user-store.service';

@Component({
  selector: 'app-update-recipe',
  templateUrl: './update-recipe.component.html',
  styleUrls: ['./update-recipe.component.css']
})
export class UpdateRecipeComponent implements OnInit{
  updateForm: FormGroup;

  recipe: Recipe | undefined;
  recipeId: number | undefined;


  categories: string[] = [];
  initialCategories: string[] = [];
  allCateogries: string[] = [];

  loading: boolean = false;

  private user: User = {
    id: 0,
    username: ''
  }

  private searchInputSubject = new Subject<string>();


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
    private router: Router,
    private tokenService: LoginService,
    private userStore: UserStoreService
    ) 
    { 
      this.updateForm = this.formBuilder.group({
        title: ['', Validators.required], // Use custom validator
        category: ['', Validators.required],
        description: ['', Validators.required],
        prepTime: ['', Validators.required],
        cookTime: ['', Validators.required],
        servings: ['', Validators.required],
        ingredients: this.formBuilder.array([], Validators.required),
        instructions: this.formBuilder.array([], Validators.required),
      });
    }

  ngOnInit(): void {

    this.route.paramMap.subscribe(params => {
      this.recipeId = Number(params.get('id'));

      if(!isNaN(this.recipeId)){

        this.recipeService.getRecipeById(this.recipeId).subscribe(recipe => {
          
          this.recipe = recipe;
          this.updateForm.patchValue({
            title: this.recipe!.title,
            description: this.recipe!.description,
            category: this.recipe!.category.categoryName,
            prepTime: this.recipe!.preparationTime.minutes,
            cookTime: this.recipe!.cookingTime.minutes,
            servings: this.recipe!.servings.quantity,
            ingredients: this.recipe!.ingredients,
            instructions: this.recipe!.instructions,
          });

          
          if (this.recipe!.ingredients && this.recipe!.ingredients.length > 0) {
            this.recipe!.ingredients.forEach((ingredient) => {
              const nameControl = new FormControl(ingredient.name);
              const amountsControl = new FormControl(ingredient.amount.quantity);
              const unitControl = new FormControl(ingredient.unit.measurement);
        
              const ingredientGroup = this.formBuilder.group({
                name: nameControl,
                order: ingredient.order,
                amounts: amountsControl,
                unit: unitControl

              });
        
              this.ingredients.push(ingredientGroup);
            });
          }
        
          if (this.recipe!.instructions && this.recipe!.instructions.length > 0) {

            this.recipe!.instructions.forEach((instruction) => {

              const textControl = new FormControl(instruction.text);
              const instructionGroup = this.formBuilder.group({
                text: textControl
              });
              this.instructions.push(instructionGroup);

            });
          }


        });
      }
    });

    this.recipeService.getCategories().subscribe({
      next: categories => {
        this.allCateogries = categories.map(category => category.categoryName);
      },
      error: error => console.error('There was an error!', error),
      complete: () => {
        this.categories = this.GetRandomElementsFromArray(this.allCateogries, 5);
        this.initialCategories = [...this.categories];

        this.searchInputSubject
        .pipe(debounceTime(200), distinctUntilChanged())
        .subscribe(searchTerm => {
          // Apply filtering when the search term changes
          this.filterCategories(searchTerm);
        });

      }
    });


    this.userStore.getUserFromStore().subscribe({
      next: user => {
        this.user.username = user || this.tokenService.getUsernameFromToken();

      },
      error: error => console.error('There was an error!', error)

    });

    this.userStore.getIdFromStore().subscribe({
      next: id => {
        this.user.id = id || this.tokenService.getIdFromToken();

      },
      error: error => console.error('There was an error!', error)
    });



  }

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

  filterCategories(searchTerm: string) {
    if(!this.updateForm.get('category')?.value)
    {
      this.categories = [...this.initialCategories];
      return;
    }
    this.categories = this.allCateogries.filter(cat =>
      cat.toLowerCase().includes(searchTerm.toLowerCase())
    );
  }

  onSearchInputChanged(event: Event) {
    this.searchInputSubject.next(this.updateForm.get('category')?.value);
  }



  updateRecipe(): void {
    this.loading = true;
    if (this.updateForm.valid) {
      const recipeDTO: RecipeDTO = {
        Title: this.updateForm.get('title')!.value,
        Description: this.updateForm.get('description')!.value,
        Category: {
          CategoryName: this.updateForm.get('category')!.value
        },
        PreparationTime: {
          Minutes: this.updateForm.get('prepTime')!.value
        },
        CookingTime:{
          Minutes: this.updateForm.get('cookTime')!.value        
        },          
        Servings:{ 
          Quantity: this.updateForm.get('servings')!.value
        },
        Ingredients: this.ingredients.controls.map(control => ({
          Name: control.get('name')?.value,
          Order: control.get('order')?.value,
          Amount: {
            Quantity: control.get('amounts')?.value
          },
          Unit: {
            Measurement: control.get('unit')?.value
          }
        })),
        Instructions: this.instructions.controls.map(control => ({
          Text: control.get('text')?.value
        })),
        User: {
          id: this.user.id,
          username: this.user.username
        }
      };

      this.recipeService.updateRecipe(recipeDTO, this.recipeId!).subscribe({
        next:() => {
          this.router.navigate(['/recipe-detail/' + this.recipeId]);
        },
        error:(error) => {
          console.error("Update recipe error: ", error);
          this.loading = false;
        }
      });
    }
  }
  

  goBack(){
    this.router.navigate(['/recipe-detail/' + this.recipeId]);
  }



  createIngredientFormGroup(index: number) {
    return new FormGroup({
      name: new FormControl(''),
      amounts: new FormControl(''),
      unit: new FormControl(''),
      order: new FormControl(index)
    });
  }

  createInstructionFormGroup() {
    return new FormGroup({
      text: new FormControl('')
    });
  }

  addIngredients(index: number) {
    this.ingredients.controls.splice(index + 1, 0, this.createIngredientFormGroup(index));
  }

  addInstructions(index: number) {
    this.instructions.controls.splice(index + 1, 0, this.createInstructionFormGroup())
  }
  
  removeIngredients(index: number) {
    if(this.ingredients.length > 1){
      this.ingredients.controls.splice(index, 1);
    }
  }

  removeInstructions(index: number) {
    if (this.instructions.length > 1) {
      this.instructions.controls.splice(index, 1);
    }
  }



  dropIngredient(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.ingredients.controls, event.previousIndex, event.currentIndex);
    for (let i = 0; i < this.ingredients.controls.length; i++) {
      this.ingredients.controls[i].get('order')!.setValue(i);
  }
  }

  dropInstruction(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.instructions.controls, event.previousIndex, event.currentIndex);
  }


}
