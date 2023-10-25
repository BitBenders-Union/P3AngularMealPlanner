export interface Recipe {
    Id: number
    Title: string
    Description: string
    Category: Category
    PreparationTime: PreparationTime
    CookingTime: CookingTime
    Servings: Servings
    Ratings: Rating[]
    Ingredients: Ingredient[]
    Instructions: Instruction[]
    User: User
  }

export interface Category {
    Id: number,
    CategoryName: string
  }

  export interface PreparationTime {
    Id: number,
    Minutes: number
  }

  export interface CookingTime {
    Id: number,
    Minutes: number
  }

  export interface Servings {
    Id: number,
    Quantity: number
  }

  export interface Rating {
    Id: number,
    Score: number
  }

  export interface Ingredient {
    Id: number,
    Name: string,
    Amount: Amount,
    Unit: Unit
  }

  export interface Amount {
    Id: number,
    Quantity: number
  }

  export interface Unit {
    Id: number,
    Measurement: string
  }

  export interface Instruction {
    id: number,
    text: string
  }

  export interface User {
    Id: number,
    Username: string
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
    Score: number
  }

  export interface IngredientDTO {
    Name: string,
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
    Id: number; 
    Username: string;
  }