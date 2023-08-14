namespace datingAppreal.Helpers
{
    public class MessageParams : PaginationParams
    {
        public string Username { get; set; }
        public string Continer { get; set; } = "UnRead";
    }
}
