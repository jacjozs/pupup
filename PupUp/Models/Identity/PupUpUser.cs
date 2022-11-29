using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PupUp.Helpers.Attributes;
using PupUp.Models.Dogs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PupUp.Models.Identity
{
    public class PupUpUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [UsernameUnique]
        public override string UserName { get; set; }
        [Required]
        [NotMapped]
        public string Password { get; set; }
        [Required]
        [EmailUserUnique]
        public override string Email { get; set; }
        public string ProfilImageUrl { get; set; }
        public Points Points { get; set; }
        public List<Dog> Dogs { get; set; } = new List<Dog>();
    }
}
