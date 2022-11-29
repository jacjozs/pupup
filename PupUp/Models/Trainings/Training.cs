using PupUp.Models.Dogs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PupUp.Models.Trainings
{
    public class Training
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<TrainingStep> Steps { get; set; }
        public List<DogTrainingState> TrainingStates { get; set; }
    }
}
