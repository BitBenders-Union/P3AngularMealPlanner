namespace Meal_Planner_Api.Interfaces
{
    public interface IInstructionRepository
    {
        ICollection<Instruction> GetInstructions();
        Instruction GetInstruction(int id);
        Instruction GetInstruction(string text);
        ICollection<Instruction> GetInstructionsByRecipeID(int recipeId);
        bool InstructionExists(int id);
        bool InstructionExistsByText(string text);
        bool CreateInstruction(Instruction instruction);
        bool DeleteInstruction(Instruction instruction);
        bool UpdateInstruction(Instruction instruction);
        bool Save();

    }
}
