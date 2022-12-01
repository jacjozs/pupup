using PupUp.Models.Dogs;

namespace PupUp.Models.Badges
{
    public class DogBadge
    {
        public int BadgeId { get; set; }
        public int DogId { get; set; }
        public Badge Badge { get; set; }
        public Dog Dog { get; set; }
    }
}
