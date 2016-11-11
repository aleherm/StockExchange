namespace StockExchange.Models
{
    // Response format for communication between methods
    public class ResponseFormat
    {
        public bool Success { get; set; } = false; // Whether the action was successful
        public string Message { get; set; } = string.Empty; // Response message, error if failed
    }
}