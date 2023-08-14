namespace datingAppreal.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public User Sender { get; set;}
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public User Recipient { get; set; }
        public string Content { get; set; }
        public DateTime? DAteRead { get; set; } 
        public DateTime MessageSent { get; set; }   
        public bool SenderDeleted { get; set; } 
        public bool RecipientDeleted { get; set; }
    }
}
