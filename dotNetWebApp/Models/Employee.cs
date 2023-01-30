using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace dotNetWebApp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("First Name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s-]*$",ErrorMessage="First Name must start with a Capital letter")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s-]*$", ErrorMessage = "Last Name must start with a Capital letter")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DisplayName("Address")]
        [RegularExpression(@"^[A-Z\d]+[\w\s-/]*$", ErrorMessage = "Address must start with Capital letter or number. Special characters allowed: -, /")]
        public string Address { get; set; }

        [Required]
        [DisplayName("Netto Salary")]
        [RegularExpression(@"[+]?[\d]+", ErrorMessage = "Netto Salary must be a positive number.")]
        public double NettoSalary { get; set; }

        [Required]
        [DisplayName("Position")]
        [RegularExpression(@"^[A-Z\d]+[\w\s-/']*$", ErrorMessage = "Position must start with Capital letter or number. Special characters allowed: -, ', /")]
        public string Position { get; set; }




    }
}
