using Microsoft.AspNetCore.Identity;

namespace OutOfOfficeSolution.Models
{
    public class User : IdentityUser<int>
    {
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; } = null!;
    }
}
