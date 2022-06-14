namespace ShephardTech.Financials.Application.Contracts
{
    public class ApiResponse<T>
    {
        public ApiResponse() { }

        public ApiResponse(T response)
        {
            Data = response;
        }

        public T Data { get; set; }

        public int StatusCode { get; set; } = 200;

        public bool hasError { get; set; } = false;

        public string Message { get; set; } = string.Empty;
    }
}
