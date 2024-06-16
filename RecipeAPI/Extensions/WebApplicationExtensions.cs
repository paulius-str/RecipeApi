using Microsoft.AspNetCore.Diagnostics;
using RecipeAPI.Middleware;
using RecipeAPI.Model.Entities;
using RecipeAPI.Model.Exceptions;

namespace RecipeAPI.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            ResourceNotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        await context.Response.WriteAsync(new ServerResult
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }

        public static void AddLoggingMiddleware(this WebApplication app)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ResponseLoggingMiddleware>();
        }
    }
}
