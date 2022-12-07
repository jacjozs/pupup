using System.ComponentModel.DataAnnotations;

namespace PupUp.Models.Trainings.Enums
{
    public enum TrainingState
    {
        None,
        [Display(Name = "Not Learned")]
        NotLearned,
        [Display(Name = "In Progress")]
        InProgress,
        [Display(Name = "Learned")]
        Learned,
        [Display(Name = "Skill")]
        Skill
    }
    public static class TrainingStateExtensions
    {
        public static string GetColor(this TrainingState state)
        {
            switch (state)
            {
                case TrainingState.NotLearned:
                    return "#c63535";
                case TrainingState.InProgress:
                    return "#FBBB21";
                case TrainingState.Learned:
                    return "#d28335";
                case TrainingState.Skill:
                    return "#7CC5AD";
                default:
                    return "white";
            }
        }
        public static int GetPercent(this TrainingState state)
        {
            switch (state)
            {
                case TrainingState.NotLearned:
                    return 0;
                case TrainingState.InProgress:
                    return 33;
                case TrainingState.Learned:
                    return 67;
                case TrainingState.Skill:
                    return 100;
                default:
                    return 100;
            }
        }
    }
}
