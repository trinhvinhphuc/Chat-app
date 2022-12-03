namespace Communicator
{
    public class FileMessage
    {
        public String sender { get; set; }
        public String receiver { get; set; }
        public String content { get; set; }
        public String extension { get; set; }
        public FileMessage(string sender, string receiver, string content, string extension)
        {
            this.sender = sender;
            this.receiver = receiver;
            this.content = content;
            this.extension = extension;
        }
    }
}
