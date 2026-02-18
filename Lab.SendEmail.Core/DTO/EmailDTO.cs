namespace Lab.SendEmail.Core.DTO
{
    public class EmailDTO
    {
        public List<string> TOs{ get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
