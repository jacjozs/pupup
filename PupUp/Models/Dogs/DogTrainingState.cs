using PupUp.Models.Trainings;
using PupUp.Models.Trainings.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace PupUp.Models.Dogs
{
    public class DogTrainingState
    {
        public int DogId { get; set; }
        public int TrainingId { get; set; }
        public Dog Dog { get; set; }
        public TrainingState State { get; set; }
        [Display(Name = "Update Time")]
        [DisplayFormat(DataFormatString = "{0:yyyy:MM:dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdateTime { get; set; }
        public Training Training { get; set; }
    }
}
