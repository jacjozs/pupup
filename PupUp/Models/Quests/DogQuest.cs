using PupUp.Models.Dogs;
using PupUp.Models.Quests.Enums;

namespace PupUp.Models.Quests
{
    public class DogQuest
    {
        public int QuestId { get; set; }
        public int DogId { get; set; }
        public QuestState State { get; set; }
        public Quest Quest { get; set; }
        public Dog Dog { get; set; }
    }
}
