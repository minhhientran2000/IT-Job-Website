using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    [Table("Job")]
    public class JobList
    {
        [Key]
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobLocation { get; set; }
        public string SalaryRange { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }       
        public int CategoryId { get; set; }       
        public Category Category { get; set; }
        public int TypeId { get; set; }
        public TypeWork Type { get; set; }
        public int LevId { get; set; }
        public LevelOfWork? Lev { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }

        public string JobRequirement { get; set; }
    }
}
