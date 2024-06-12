using Org.BouncyCastle.Crypto.Tls;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models.Chat
{
    [Table("Message")]
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public string? Content { get; set; }
        public string? Time { get; set; }
        public int? GroupId { get; set; }
        public int? UserType { get; set; }
        public string? DataType { get; set; }
        public string? FileName { get; set; }
        public string? FileLink { get; set; }

        /*public Seeker? Seeker { get; set; }
        public Company? Company { get; set; }*/
    }
}
