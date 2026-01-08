using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Dto
{
    public class RegistrationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[0-9]+.*)(?=.*[a-zA-Z]+.*)[0-9a-zA-Z]{6,}$", ErrorMessage = "Password must contain at least one letter, at least one number, and be longer than six charaters.")]
        public string Password { get; set; }

        [Required]
        [MinLength(5,ErrorMessage ="Display length will be minimum of 5 character")]
        public string DisplayName {  get; set; }

    }
}
