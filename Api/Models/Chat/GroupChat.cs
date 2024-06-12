using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Chat
{
    [Table("GroupChat")]
    public class GroupChat
    {
        [Key]
        public int GroupId { get; set; }
        public int? SeekerId { get; set; }
        public int? CompanyId { get; set; }
        public string? GroupName { get; set; }

    }
}
