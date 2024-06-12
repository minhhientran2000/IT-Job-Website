namespace Api.Models
{
    public class JobItem
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobLocation { get; set; }
        public string SalaryRange { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int CompanyId { get; set; }      
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public int LevId { get; set; }
        public int LocationId { get; set; }
        public string JobRequirement { get; set; }

    }
}
