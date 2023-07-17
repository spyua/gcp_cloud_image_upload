namespace cbk.image.Infrastructure.Models
{
    public class ApiResponse
    {
        public string? Message { get; set; }

        public Task<ApiResponse> ToTask()
        {
            return Task.FromResult(this);
        }

        public static ApiResponse Ok(string? message = null)
        {
            return new ApiResponse
            {
                Message = message
            };
        }
    }

    public class ApiResponse<TData> : ApiResponse
    {
        public TData Data { get; set; } = default!;

        public new Task<ApiResponse<TData>> ToTask()
        {
            return Task.FromResult(this);
        }
    }
}
