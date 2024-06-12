using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    [Table("LevelOfWork")]
    public class LevelOfWork
    {
        [Key]
        public int LevId { get; set; }
        public string LevelName { get; set; }
        /*public List<JobList> Jobs { get; set; }*/
    }
}
