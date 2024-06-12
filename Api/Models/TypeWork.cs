using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Table("TypeOfWork")]
    public class TypeWork
    {
        [Key]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
       /* public List<JobList> Jobs { get; set; }*/
    }
}
