using PupUp.Models.Quests.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PupUp.Models.Quests
{
    public class Quest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string RequiredQuests { get; set; }
        public string TrainingIds { get; set; }
        public bool Repetable { get; set; }
        public bool UserQuest { get; set; }
        public ActionType ActionType { get;set; } 
        public RewardType RewardType { get; set; }
        public string RewardValue { get; set; }
        public int RewardNum { get { if (int.TryParse(RewardValue, out int value)) { return value; } else { return 0; } } }
        public List<int> RewardIds => RewardValue?.Split(':').Select(int.Parse).ToList() ?? new List<int>();
        public List<int> RequiredQuestIds => RequiredQuests?.Split(':').Select(int.Parse).ToList() ?? new List<int>();
        public List<int> ListenTrainingIds => TrainingIds?.Split(':').Select(int.Parse).ToList() ?? new List<int>();
    }
}
