using System.ComponentModel.DataAnnotations;

namespace SampleMvcApp.Models
{
    public class SampleForm
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public int[] MyList { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        public string SSN { get; set; }
    }
}