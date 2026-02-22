using BankSystemApi.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemApi.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(BadRequestException ex)
            {
                context.Response.StatusCode = 400;

                var problemDetails = new ProblemDetails
                {
                    Status = 400,
                    Title = "Bad Request",
                    Detail = ex.Message
                };

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch(NotFoundException ex)
            {
                context.Response.StatusCode = 404;

                var problemDetails = new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = ex.Message
                };

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch(UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = 401;

                var problemDetails = new ProblemDetails
                {
                    Status = 401,
                    Title = "Unauthorized",
                    Detail = ex.Message
                };

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = 500;

                var problemDetails = new ProblemDetails
                {
                    Status = 500,
                    Title = "Server Error",
                    Detail = ex.Message
                };

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
