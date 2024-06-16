namespace RecipeAPI.Shared.DTOs
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<string>? Ingredients { get; set; }
        public List<string>? Instructions { get; set; }
        public int PrepTimeMinutes { get; set; }
        public int CookTimeMinutes { get; set; }
        public int Servings { get; set; }
        public string? Difficulty { get; set; }
        public string? Cuisine { get; set; }
        public int CaloriesPerServing { get; set; }
        public List<string>? Tags { get; set; }
        public string? Image { get; set; }
        public List<string>? MealType { get; set; }
        public bool IsFavorite { get; set; }
    }
}
