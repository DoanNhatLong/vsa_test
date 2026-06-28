using Microsoft.AspNetCore.Diagnostics;
using bank.Entity;
using bank.Exceptions;

namespace bank.Middleware;

public static class ExceptionMiddleware
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                var ex = feature?.Error;
                context.Response.ContentType = "application/json";

                if (ex is BusinessException bizEx)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(ApiResponse<object>.ErrorResponse(400, "Business validation failed", bizEx.Errors));
                }
                else
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(ApiResponse<object>.ErrorResponse(500, "Internal Server Error", new List<ApiError> { new("System", ex?.Message ?? "Unknown error") }));
                }
            });
        });
    }
}
