﻿// <auto-generated />
using System;
using Meal_Planner_Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Meal_Planner_Api.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230915110736_update3")]
    partial class update3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Meal_Planner_Api.Models.Amount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Quantity")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Amounts");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.CookingTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Minutes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CookingTimes");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AmountId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnitId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AmountId");

                    b.HasIndex("UnitId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Instruction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Instructions");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.PreparationTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Minutes")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PreparationTimes");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Score")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("CookingTimeId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PreparationTimeId")
                        .HasColumnType("int");

                    b.Property<int?>("RecipeScheduleId")
                        .HasColumnType("int");

                    b.Property<int>("ServingsId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CookingTimeId");

                    b.HasIndex("PreparationTimeId");

                    b.HasIndex("RecipeScheduleId");

                    b.HasIndex("ServingsId");

                    b.HasIndex("UserId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.RecipeIngredient", b =>
                {
                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<int>("IngredientId")
                        .HasColumnType("int");

                    b.Property<int>("AmountId")
                        .HasColumnType("int");

                    b.Property<int>("UnitId")
                        .HasColumnType("int");

                    b.HasKey("RecipeId", "IngredientId");

                    b.HasIndex("AmountId");

                    b.HasIndex("IngredientId");

                    b.HasIndex("UnitId");

                    b.ToTable("RecipeIngredients");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.RecipeRating", b =>
                {
                    b.Property<int>("RecipeID")
                        .HasColumnType("int");

                    b.Property<int>("RatingID")
                        .HasColumnType("int");

                    b.HasKey("RecipeID", "RatingID");

                    b.HasIndex("RatingID");

                    b.ToTable("RecipeRatings");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.RecipeSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Column")
                        .HasColumnType("int");

                    b.Property<int>("Row")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RecipeSchedules");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Servings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Servings");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Measurement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.UserRating", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RatingId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RatingId");

                    b.HasIndex("RatingId");

                    b.ToTable("UserRating");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Ingredient", b =>
                {
                    b.HasOne("Meal_Planner_Api.Models.Amount", "Amount")
                        .WithMany()
                        .HasForeignKey("AmountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Meal_Planner_Api.Models.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Amount");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Instruction", b =>
                {
                    b.HasOne("Meal_Planner_Api.Models.Recipe", "Recipe")
                        .WithMany("Instructions")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Recipe", b =>
                {
                    b.HasOne("Meal_Planner_Api.Models.Category", "Category")
                        .WithMany("Recipes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Meal_Planner_Api.Models.CookingTime", "CookingTime")
                        .WithMany("Recipe")
                        .HasForeignKey("CookingTimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Meal_Planner_Api.Models.PreparationTime", "PreparationTime")
                        .WithMany("Recipe")
                        .HasForeignKey("PreparationTimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Meal_Planner_Api.Models.RecipeSchedule", null)
                        .WithMany("Recipes")
                        .HasForeignKey("RecipeScheduleId");

                    b.HasOne("Meal_Planner_Api.Models.Servings", "Servings")
                        .WithMany("Recipe")
                        .HasForeignKey("ServingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Meal_Planner_Api.Models.User", "User")
                        .WithMany("Recipes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("CookingTime");

                    b.Navigation("PreparationTime");

                    b.Navigation("Servings");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.RecipeIngredient", b =>
                {
                    b.HasOne("Meal_Planner_Api.Models.Amount", "Amount")
                        .WithMany("Ingredients")
                        .HasForeignKey("AmountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Meal_Planner_Api.Models.Ingredient", "Ingredient")
                        .WithMany("RecipeIngredients")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Meal_Planner_Api.Models.Recipe", "Recipe")
                        .WithMany("RecipeIngredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Meal_Planner_Api.Models.Unit", "Unit")
                        .WithMany("Ingredients")
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Amount");

                    b.Navigation("Ingredient");

                    b.Navigation("Recipe");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.RecipeRating", b =>
                {
                    b.HasOne("Meal_Planner_Api.Models.Rating", "Rating")
                        .WithMany("RecipeRating")
                        .HasForeignKey("RatingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Meal_Planner_Api.Models.Recipe", "Recipe")
                        .WithMany("RecipeRating")
                        .HasForeignKey("RecipeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rating");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.RecipeSchedule", b =>
                {
                    b.HasOne("Meal_Planner_Api.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.UserRating", b =>
                {
                    b.HasOne("Meal_Planner_Api.Models.Rating", "Rating")
                        .WithMany("UserRating")
                        .HasForeignKey("RatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Meal_Planner_Api.Models.User", "User")
                        .WithMany("UserRating")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rating");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Amount", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Category", b =>
                {
                    b.Navigation("Recipes");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.CookingTime", b =>
                {
                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Ingredient", b =>
                {
                    b.Navigation("RecipeIngredients");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.PreparationTime", b =>
                {
                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Rating", b =>
                {
                    b.Navigation("RecipeRating");

                    b.Navigation("UserRating");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Recipe", b =>
                {
                    b.Navigation("Instructions");

                    b.Navigation("RecipeIngredients");

                    b.Navigation("RecipeRating");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.RecipeSchedule", b =>
                {
                    b.Navigation("Recipes");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Servings", b =>
                {
                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.Unit", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("Meal_Planner_Api.Models.User", b =>
                {
                    b.Navigation("Recipes");

                    b.Navigation("UserRating");
                });
#pragma warning restore 612, 618
        }
    }
}
