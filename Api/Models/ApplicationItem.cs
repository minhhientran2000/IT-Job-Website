namespace Api.Models
{
    public class ApplicationItem
    {
        public int ApplicationId { get; set; }
        public DateTime CreateDate { get; set; }
        public int JobId { get; set; }
        public int StatusId { get; set; }
        public string? IntroducingLetter { get; set; }
        public int UserId { get; set; }
    }
}
