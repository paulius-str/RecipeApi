namespace RecipeAPI.Service.Contract
{
    public interface IDateTimeService
    {
        DateTime Now();
        DateTime UtcNow();
    }
}