namespace Communicator
{
    public class BufferFile
    {
        public String sender { get; set; }
        public String receiver { get; set; }
        public byte[] buffer { get; set; }
        public String extension { get; set; }
        public BufferFile(string sender, string receiver, byte[] buffer, string extension)
        {
            this.sender = sender;
            this.receiver = receiver;
            this.buffer = buffer;
            this.extension = extension;
        }
    }
}
