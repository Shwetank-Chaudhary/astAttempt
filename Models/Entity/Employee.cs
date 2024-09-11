using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace astAttempt.Models.Entity
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmpId { get; set; }

        public required string FirstName { get; set; }

        public string? LastName { get; set; }

        
        public City? City { get; set; }
        public required string CityId { get; set; }
        [ForeignKey("CityId")]


        public int PhoneNumber { get; set; }

        public required string Password { get; set; }

        public string Role { get; set; } = "Employee";

        public required string EmpEmail { get; set; }

        //public string UserId { get; set; }

        //[ForeignKey("UserId")]
        //AspNetUserManager<Employee> _userManager { get; set; }
    }
}
