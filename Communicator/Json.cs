namespace Communicator
{
    public class Json
    {
        public String type { get; set; }
        public String content { get; set; }
        public Json(String type, String content)
        {
            this.type = type;
            this.content = content;
        }
    }
}