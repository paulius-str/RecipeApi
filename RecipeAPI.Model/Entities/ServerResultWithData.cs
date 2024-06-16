namespace RecipeAPI.Model.Entities
{
    public class ServerResultWithData<T> : ServerResult
    {
        public T Data { get; set; }
    }
}
