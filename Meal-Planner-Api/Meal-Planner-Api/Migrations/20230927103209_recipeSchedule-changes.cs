using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meal_Planner_Api.Migrations
{
    /// <inheritdoc />
    public partial class recipeSchedulechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_RecipeSchedules_RecipeScheduleId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_RecipeScheduleId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "RecipeScheduleId",
                table: "Recipes");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "RecipeSchedules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSchedules_RecipeId",
                table: "RecipeSchedules",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeSchedules_Recipes_RecipeId",
                table: "RecipeSchedules",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeSchedules_Recipes_RecipeId",
                table: "RecipeSchedules");

            migrationBuilder.DropIndex(
                name: "IX_RecipeSchedules_RecipeId",
                table: "RecipeSchedules");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "RecipeSchedules");

            migrationBuilder.AddColumn<int>(
                name: "RecipeScheduleId",
                table: "Recipes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_RecipeScheduleId",
                table: "Recipes",
                column: "RecipeScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_RecipeSchedules_RecipeScheduleId",
                table: "Recipes",
                column: "RecipeScheduleId",
                principalTable: "RecipeSchedules",
                principalColumn: "Id");
        }
    }
}
