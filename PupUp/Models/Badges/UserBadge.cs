using PupUp.Models.Identity;

namespace PupUp.Models.Badges
{
    public class UserBadge
    {
        public int BadgeId { get; set; }
        public string UserId { get; set; }
        public Badge Badge { get; set; }
        public PupUpUser User { get; set; }
    }
}
