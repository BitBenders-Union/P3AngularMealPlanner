import { Component, OnInit } from '@angular/core';
import { Amount, Unit, Ingredient, Recipe, Instruction, RecipeDTO } from '../Interfaces';
import { RecipeServiceService } from '../service/recipe-service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, FormControl, AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-update-recipe',
  templateUrl: './update-recipe.component.html',
  styleUrls: ['./update-recipe.component.css']
})
export class UpdateRecipeComponent implements OnInit{
  updateForm: FormGroup;

  recipe: Recipe | undefined;
  recipeId: number | undefined;


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
        title: '',
        description: '',
        category: this.formBuilder.group({
          categoryName: ['']
        }),
        preparationTimes: this.formBuilder.group({
          minutes: [0]
        }),
        cookingTimes: this.formBuilder.group({
          minutes: [0]
        }),
        servings: this.formBuilder.group({
          quantity: [0]
        }),
        ratings: [[]],
        ingredients: [[]],
        instructions: [[]],
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
            category: {
              categoryName: this.recipe!.category.categoryName
            },
            preparationTimes: {
              minutes: this.recipe!.preparationTime.minutes
            },
            cookingTimes: {
              minutes: this.recipe!.cookingTime.mintues
            },
            servings: {
              quantity: this.recipe!.servings.Quantity
            },
            ratings: this.recipe!.rating,
            ingredients: this.recipe!.ingredients,
            instructions: this.recipe!.instructions
          });

          
          if (this.recipe!.ingredients && this.recipe!.ingredients.length > 0) {
            this.recipe!.ingredients.forEach((ingredient) => {
              const nameControl = new FormControl(ingredient.name);
              const valueControl = new FormControl(ingredient.amount.quantity);
              const unitControl = new FormControl(ingredient.unit.measurement);
        
              const ingredientGroup = this.formBuilder.group({
                name: nameControl,
                value: valueControl,
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

          // this.updateForm.addControl('deleted', new FormControl(this.recipe!.deleted));

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

      const ingredients: Ingredient[] = formData.ingredients.map((ingredient: any) => {
        const amounts: Amount = {
          id: 0,
          quantity: ingredient.value,
        };
        const unit: Unit ={
          id: 0,
          measurement: ingredient.unit
        }

        return {
          name: ingredient.name,
          amounts: amounts,
          unit: unit
        };
      });

      const instructions: Instruction[] = formData.instructions.map((instruction: any) => {
        return {
          text: instruction.text
        };
      });

      const updatedRecipe: RecipeDTO = {
        Title: formData.title,
        Category: formData.category,
        Description: formData.description,
        PreparationTimes: formData.preparationTime,
        CookingTimes: formData.cookingTime,
        Servings: formData.servings,
        Ratings: [formData.rating],
        Ingredients: formData.ingredients,
        Instructions: formData.instructions,
        User: this.recipe!.user,
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
