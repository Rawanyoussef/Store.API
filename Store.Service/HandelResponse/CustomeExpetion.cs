namespace Store.Service.HandelResponse
{
    public class CustomeExpetion : Response
    {
        public CustomeExpetion(int statusCode, string? message = null , string? details=null) 
            : base(statusCode, message)
        {
            Details = details;
        }
        public string? Details { get; set; }
    }
}
