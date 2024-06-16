namespace RecipeAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger
            )
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation($"REQ: (Method: {context.Request.Method}, Path: {context.Request.Path}, Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");

            await _next(context);
        }
    }
}
