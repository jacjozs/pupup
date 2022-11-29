using PupUp.Models.Dogs.Enums;
using PupUp.Models.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PupUp.Models.Dogs
{
    public class Dog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DogType Type { get; set; }
        public int Age { get; set; }
        public DogGender Gender { get; set; }
        public string UserId { get; set; }
        public PupUpUser User { get; set; }
        public List<DogTrainingState> TrainingStates { get; set; }
    }
}
