export interface Recipe {
    id: number
    title: string
    description: string
    category: Category
    preparationTime: PreparationTime
    cookingTime: CookingTime
    servings: Servings
    rating: Rating
    ingredients: Ingredient[]
    instructions: Instruction[]
    user: User
  }

export interface Category {
    id: number,
    categoryName: string
  }

  export interface PreparationTime {
    id: number,
    minutes: number
  }

  export interface CookingTime {
    id: number,
    mintues: number
  }

  export interface Servings {
    id: number,
    Quantity: number
  }

  export interface Rating {
    id: number,
    score: number
  }

  export interface Ingredient {
    id: number,
    name: string,
    amount: Amount,
    unit: Unit
  }

  export interface Amount {
    id: number,
    quantity: number
  }

  export interface Unit {
    id: number,
    measurement: string
  }

  export interface Instruction {
    id: number,
    text: string
  }

  export interface User {
    id: number,
    username: string
  }



  export interface WeekData{
    WeekDataId: number;
    userId: number;
    row: number;
    column: number;
    recipeId: number;
  }