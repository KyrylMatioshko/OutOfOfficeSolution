namespace OutOfOfficeSolution.ViewModels
{
    public class UserRolesViewModel
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
