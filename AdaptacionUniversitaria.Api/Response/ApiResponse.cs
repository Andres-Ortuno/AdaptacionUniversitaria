namespace AdaptacionUniversitaria.Api.Response
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string? Message { get; set; }

        public ApiResponse(T data, string? message = null)
        {
            Data = data;
            Message = message;
        }
    }
}
