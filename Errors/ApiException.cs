namespace datingAppreal.Errors
{
    public class ApiException
    {
        public ApiException(int statuesCode, string message, string details)
        {
            StatuesCode = statuesCode;
            Message = message;
            Details = details;
        }

        public int StatuesCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }


    }
}
