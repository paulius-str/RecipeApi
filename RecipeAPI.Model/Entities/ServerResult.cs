using System.Text.Json;

namespace RecipeAPI.Model.Entities
{
    public class ServerResult
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
