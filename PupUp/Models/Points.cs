using PupUp.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace PupUp.Models
{
    public class Points
    {
        [Key]
        public string UserId { get; set; }
        public PupUpUser User { get; set; }
        public long Exp { get; set; }
        public long Coin { get; set; }
    }
}
