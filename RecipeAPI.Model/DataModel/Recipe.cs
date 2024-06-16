﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeAPI.Model.DataModel
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<string>? Ingredients { get; set; }
        public List<string>? Instructions { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int Servings { get; set; }
        public string Difficulty { get; set; }
        public string Cuisine { get; set; }
        public int CaloriesPerServing { get; set; }
        public List<string>? Tags { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public List<string> MealType { get; set; }
        [NotMapped]
        public bool IsFavorite { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
    }
}
