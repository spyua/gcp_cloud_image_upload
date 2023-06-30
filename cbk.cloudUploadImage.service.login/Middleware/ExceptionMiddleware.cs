using cbk.cloudUploadImage.service.login.Dto;

namespace cbk.cloudUploadImage.service.login.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
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
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new ApiResponse<string>
            {
                Message = exception.Message,
                // 如果需要，可以在此處添加更多詳細的異常處理邏輯
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
