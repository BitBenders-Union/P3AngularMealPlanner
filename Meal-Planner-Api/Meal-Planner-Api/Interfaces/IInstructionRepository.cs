using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IInstructionRepository
    {
        ICollection<Instruction> GetInstructions();
        Instruction GetInstruction(int id);
        Instruction GetInstructionByRecipeID(int recipeId);
        bool InstructionExists(int id);
        bool InstructionExistsByRecipeID(int recipeId);

    }
}
