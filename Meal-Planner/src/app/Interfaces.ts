export interface Recipe {
    id: number
    title: string
    description: string
    category: Category
    preparationTime: PreparationTime
    cookingTime: CookingTime
    servings: Servings
    ingredients: Ingredient[]
    instructions: Instruction[]
    user: User
  }

  export interface RecipeWithScore {
    id: number
    title: string
    score: (boolean | string)[]
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
    minutes: number
  }

  export interface Servings {
    id: number,
    quantity: number
  }

  export interface Rating {
    id: number,
    score: number
  }

  export interface Ingredient {
    id: number,
    name: string,
    order: number,
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


  export interface RecipeDTO {
    Title: string
    Description: string
    Category: CategoryDTO
    PreparationTime: PreparationTimeDTO
    CookingTime: CookingTimeDTO
    Servings: ServingsDTO
    Ingredients: IngredientDTO[]
    Instructions: InstructionDTO[]
    User: User
  }

export interface CategoryDTO {
    CategoryName: string
  }

  export interface PreparationTimeDTO {
    Minutes: number
  }

  export interface CookingTimeDTO {
    Minutes: number
  }

  export interface ServingsDTO {
    Quantity: number
  }

  export interface RatingDTO {
    score: number
  }

  export interface IngredientDTO {
    Name: string,
    Order: number,
    Amount: AmountDTO,
    Unit: UnitDTO
  }

  export interface AmountDTO {
    Quantity: number
  }

  export interface UnitDTO {
    Measurement: string
  }

  export interface InstructionDTO {
    Text: string
  }




  export interface RecipeScheduleDTO{
    row: number;
    column: number;
    recipeId?: number;
    user: UserOnlyName;
  }

  export interface UserOnlyName{
    id: number; 
    username: string;
  }

  export interface RatingWithRecipeId{
    recipeId: number;
    score: number;
  }

  export interface RecipeIdAndStarsArray{
    recipeId: number;
    starsArray: (boolean | string)[];
  }