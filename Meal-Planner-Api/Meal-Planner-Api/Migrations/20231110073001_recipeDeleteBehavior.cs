﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meal_Planner_Api.Migrations
{
    /// <inheritdoc />
    public partial class recipeDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RecipeSchedules_RecipeId",
                table: "RecipeSchedules",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeSchedules_Recipes_RecipeId",
                table: "RecipeSchedules",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
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
        }
    }
}
