using RecipeAPI.Service.Contract;

namespace RecipeAPI.Service
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now() => DateTime.Now;
        public DateTime UtcNow() => DateTime.UtcNow;
    }
}
