using PupUp.Models.Identity;
using PupUp.Models.Quests.Enums;

namespace PupUp.Models.Quests
{
    public class UserQuest
    {
        public int QuestId { get; set; }
        public string UserId { get; set; }
        public QuestState State { get; set; }
        public Quest Quest { get; set; }
        public PupUpUser User { get; set; }
    }
}
