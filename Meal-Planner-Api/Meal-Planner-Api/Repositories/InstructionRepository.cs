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

        public bool CreateInstruction(Instruction instruction)
        {
            _context.Add(instruction);
            return Save();
        }

        public bool DeleteInstruction(Instruction instruction)
        {
            _context.Remove(instruction);
            return Save();
        }

        public Instruction GetInstruction(int id)
        {
            return _context.Instructions.FirstOrDefault(x => x.Id == id);
        }

        public Instruction GetInstruction(string text)
        {
            return _context.Instructions.FirstOrDefault(x => x.Text == text);

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
        
        public bool InstructionExistsByText(string text)
        {
            return _context.Instructions.Any(i => i.Text == text);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateInstruction(Instruction instruction)
        {
            throw new NotImplementedException();
        }
    }
}
