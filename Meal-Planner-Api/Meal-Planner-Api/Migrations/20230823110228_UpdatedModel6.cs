using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meal_Planner_Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModel6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Amounts_IngredientId",
                table: "Amounts");

            migrationBuilder.CreateIndex(
                name: "IX_Amounts_IngredientId",
                table: "Amounts",
                column: "IngredientId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Amounts_IngredientId",
                table: "Amounts");

            migrationBuilder.CreateIndex(
                name: "IX_Amounts_IngredientId",
                table: "Amounts",
                column: "IngredientId");
        }
    }
}
