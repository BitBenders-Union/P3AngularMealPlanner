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

  [
    {
      "id": 4,
      "title": "Test",
      "description": "test",
      "category": {
        "id": 4,
        "categoryName": "test"
      },
      "preparationTime": {
        "id": 3,
        "minutes": 1
      },
      "cookingTime": {
        "id": 4,
        "minutes": 2
      },
      "servings": {
        "id": 3,
        "quantity": 3
      },
      "ingredients": [
        {
          "id": 6,
          "name": "test",
          "amount": {
            "id": 5,
            "quantity": 4
          },
          "unit": {
            "id": 6,
            "measurement": "5"
          }
        },
        {
          "id": 7,
          "name": "test2",
          "amount": {
            "id": 6,
            "quantity": 5
          },
          "unit": {
            "id": 7,
            "measurement": "7"
          }
        },
        {
          "id": 8,
          "name": "wow",
          "amount": {
            "id": 4,
            "quantity": 0
          },
          "unit": {
            "id": 8,
            "measurement": "g"
          }
        },
        {
          "id": 9,
          "name": "myingr",
          "amount": {
            "id": 4,
            "quantity": 0
          },
          "unit": {
            "id": 9,
            "measurement": "luck"
          }
        }
      ],
      "instructions": [
        {
          "id": 13,
          "text": "wow"
        },
        {
          "id": 14,
          "text": "test"
        }
      ],
      "user": {
        "id": 1,
        "username": "test"
      }
    }
  ]

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