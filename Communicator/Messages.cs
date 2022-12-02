using System.Text.Json.Serialization;

namespace Communicator
{
    public class Messages
    {
        public String sender { get; set; }
        public String receiver { get; set; }
        public String message { get; set; }
        public Messages(String sender, String recerver, String message)
        {
            this.sender = sender;
            this.receiver = recerver;
            this.message = message;
        }
        [JsonConstructor]
        public Messages() { }
    }
}
