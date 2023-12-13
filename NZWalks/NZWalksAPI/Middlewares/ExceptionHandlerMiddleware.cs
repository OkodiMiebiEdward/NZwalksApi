using System.Net;

namespace NZWalksAPI.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
           _logger = logger;
           _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                //Log this exception
               _logger.LogError(ex,$"{errorId}: {ex.Message}");

                //Return a custom response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new 
                {
                    Id = errorId,
                    ErrorMessage = "Something Went Wrong! We Are Looking Into Resolving It."
                };
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
