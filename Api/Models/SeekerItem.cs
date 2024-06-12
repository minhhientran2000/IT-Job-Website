namespace Api.Models
{
    public class SeekerItem
    {
        public int SeekerId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FileCV { get; set; }
        public string? Photo { get; set; }
        public decimal? GPA { get; set; }
        public string? Major { get; set; }
        public int? LocationId { get; set; }
        public string? SeekerAddress { get; set; }
    }
}
