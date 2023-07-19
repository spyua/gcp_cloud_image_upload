using cbk.image.Application.Models;
using Microsoft.AspNetCore.Http;

namespace cbk.image.Web.Middleware
{
    public class ValidateImageFileTypeMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidateImageFileTypeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.HasFormContentType)
            {
                var form = await context.Request.ReadFormAsync();
                var file = form.Files["file"]; // the name should match the name of your file input

                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName).ToLower();

                    if (extension != ".png" && extension != ".jpg")
                    {
                        var response = new ApiResponse<string>
                        {
                            Message = "Invalid file type. Only .png and .jpg are supported."
                        };

                        context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
                        await context.Response.WriteAsJsonAsync(response);
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
