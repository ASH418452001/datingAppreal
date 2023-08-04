using datingAppreal.Errors;
using System.Net;
using System.Text.Json;

namespace datingAppreal.MiddleWare
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly Logger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate Next , Logger<ExceptionMiddleware> logger 
            ,IHostEnvironment env)
        {
            next = Next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, ex.Message, "Internal erver Error");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
          
        }
    }
} 
