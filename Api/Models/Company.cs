using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Table("Company")]
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyWebsiteUrl { get; set; }
        public string? CompanyEmail { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyPhoto { get; set; }
        public bool IsActive { get; set; }      
        public int UserId { get; set; }
        public User User { get; set; }
        /* public List<JobList> Jobs { get; set; }*/
    }
}
