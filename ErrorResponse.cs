namespace MyWebsite
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {
        }
        
        public ErrorResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        
        public string ErrorMessage { get; set; }
    }
}