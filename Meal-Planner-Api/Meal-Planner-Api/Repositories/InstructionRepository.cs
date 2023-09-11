using Meal_Planner_Api.Data;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Repositories
{
    public class InstructionRepository : IInstructionRepository
    {
        private readonly DataContext _context;

        public InstructionRepository(DataContext context)
        {
            _context = context;
        }

        public Instruction GetInstruction(int id)
        {
            return _context.Instructions.FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Instruction> GetInstructions()
        {
            return _context.Instructions.OrderBy(x => x.Id).ToList();
        }

        public ICollection<Instruction> GetInstructionsByRecipeID(int recipeId)
        {            

            // get all instructions 
            // include recipe
            // find instructions where recipe.id matches input parameter.

            var instructions = _context.Instructions.Include(r => r.Recipe)
                .Where(r => r.Recipe.Id == recipeId)
                .ToList();

            return instructions;
            
        }

        public bool InstructionExists(int id)
        {
            return _context.Instructions.Any(i => i.Id == id);
        }

    }
}
