using Api.Models.CV;
using Microsoft.Identity.Client;

namespace Api.Models
{
    public class CvData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Target { get; set; }
        public string Summary { get; set; }
        public string GitUrl { get; set; }
        public string? Photo { get; set; }
        public List<WorkExperience> WorkExperiences { get; set; }
        public List<Education> Education { get; set; }
        public List<Skill> Skills { get; set; }
       /* public List<Reference> References { get; set; }*/
        public List<Project> Projects { get; set; }
        public List<Certificate> Certificates { get; set; }
       
    }
}
