using System.ComponentModel.DataAnnotations;

namespace BidOneUI.Models
{
    public class Person
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name must not exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name must not exceed 50 characters.")]
        public string LastName { get; set; }
    }
}
