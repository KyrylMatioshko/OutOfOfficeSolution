namespace OutOfOfficeSolution.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public ManageUserRolesViewModel()
        {
            Roles = new List<UserRolesViewModel>();
        }
        public int UserId { get; set; }
        public List<UserRolesViewModel> Roles { get; set; }
    }
}
