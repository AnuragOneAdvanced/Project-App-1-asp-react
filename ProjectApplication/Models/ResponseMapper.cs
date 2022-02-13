namespace ProjectApplication.Models
{
    public class ResponseMapper
    {
        public int StatusCode { get; set; }
        public object? Value { get; set; }

        public string? Message { get; set; }
    }
}
