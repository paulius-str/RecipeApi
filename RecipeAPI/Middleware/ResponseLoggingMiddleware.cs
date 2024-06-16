namespace RecipeAPI.Middleware
{
    public class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseLoggingMiddleware> _logger;

        public ResponseLoggingMiddleware(
            RequestDelegate next,
            ILogger<ResponseLoggingMiddleware> logger
            )
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            _logger.LogInformation($"RES: (Method: {context.Request.Method}, Path: {context.Request.Path}, Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff})");
        }
    }
}
