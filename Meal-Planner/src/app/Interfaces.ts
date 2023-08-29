export interface Recipe {
    id: number;
    title: string;
    category: string;
    description: string;
    preparationTime: number;
    cookingTime: number;
    servings: number;
    rating: number;
    ingredients: Ingredient[];
    instructions: Instruction[];
    deleted: boolean;
  }


  
  export interface Ingredient {
    name: string;
    amounts: Amounts;
  }
  
  export interface Amounts {
    value: number;
    unit: string;
  };

  export interface Instruction{
    text: string;
    recipeId: number;
  }



  export interface CreateRecipe{
    title: string;
    category: string;
    description: string;
    preparationTime: number;
    cookingTime: number;
    servings: number;
    rating: number;
    ingredients: CreateIngredient[];
    instructions: CreateInstruction[];
    deleted: boolean;
  }


  export interface CreateIngredient {
    name: string;
    amounts: Amounts;
  }
  
  export interface CreateAmounts {
    value: number;
    unit: string;
  };

  export interface CreateInstruction{
    text: string;
  }
  