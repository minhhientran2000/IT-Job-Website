using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    
        [Table("Seeker")]
        public class Seeker
        {
            [Key]
            public int SeekerId { get; set; }
            public string? Name { get; set; }
            public string? PhoneNumber { get; set; }
            public string? FileCV { get; set; }
            public string? Photo { get; set; }
            public decimal? GPA { get; set; }
            public string? Major { get; set; }
            public string? SeekerAddress { get; set; }
            public int UserId { get; set; }
            public User User { get; set; }
            public int? LocationId { get; set; }
            public Location Location { get; set; }
        

        }
    
}
