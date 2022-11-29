using PupUp.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PupUp.Helpers.Attributes
{
    public class UsernameUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var context = (PupUpDbContext)validationContext.GetService(typeof(PupUpDbContext));
            var entity = context.Users.FirstOrDefault(e => e.UserName == value.ToString());

            if (entity != null)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string username)
        {
            return $"Username {username} is already in use.";
        }
    }
}
