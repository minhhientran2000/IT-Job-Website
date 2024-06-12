using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    [Table("Application")]
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }
        public DateTime CreateDate { get; set; }
        public int JobId { get; set; }  
        public int StatusId { get; set; }
        public string? IntroducingLetter { get; set; }
        public int UserId { get; set; }
        public ApplicationStatus Status {get; set; }
        public JobList Job { get; set; }
        public User User { get; set; }

    }
}
