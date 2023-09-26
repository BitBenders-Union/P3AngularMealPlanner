using Meal_Planner_Api.Models;

namespace Meal_Planner_Api.Interfaces
{
    public interface IInstructionRepository
    {
        ICollection<Instruction> GetInstructions();
        Instruction GetInstruction(int id);
        ICollection<Instruction> GetInstructionsByRecipeID(int recipeId);
        bool InstructionExists(int id);
        bool CreateInstruction(Instruction instruction);
        bool DeleteInstruction(Instruction instruction);
        bool UpdateInstruction(Instruction instruction);
        bool Save();

    }
}
