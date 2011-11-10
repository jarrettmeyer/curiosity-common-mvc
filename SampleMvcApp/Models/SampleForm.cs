using System.ComponentModel.DataAnnotations;

namespace SampleMvcApp.Models
{
    public class SampleForm
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}