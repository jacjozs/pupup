using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PupUp.Models.Trainings
{
    public class TrainingStep : IComparable<TrainingStep>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Index { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }
        public int TrainingId { get; set; }
        public Training Training { get; set; }

        public int CompareTo(TrainingStep other) => Index.CompareTo(other.Index);
    }
}
