using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LoginRegister.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Имя обязательно.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна.")]
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
