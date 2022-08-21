using Domain.Exceptions;

namespace WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private IReadOnlyDictionary<Type, int> _exceptionStatusCodes = new Dictionary<Type, int>
        {
            {typeof(BadRequestException), StatusCodes.Status400BadRequest },
            {typeof(Exception), StatusCodes.Status500InternalServerError }
        };
        public ExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger(nameof(ExceptionHandlingMiddleware));
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                _logger.LogInformation($"Sending request to {context.Request.Path}");
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = GetStatusCode(ex.GetType());
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync(ex.Message);
            }
        }
        private int GetStatusCode(Type exceptionType)
        {
            return _exceptionStatusCodes
                .FirstOrDefault(exceptionPair => exceptionPair.Key.IsAssignableFrom(exceptionType))
                .Value;
        }
    }
}
