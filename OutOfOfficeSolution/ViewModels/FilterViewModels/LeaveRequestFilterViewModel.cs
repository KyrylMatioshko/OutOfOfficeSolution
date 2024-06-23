namespace OutOfOfficeSolution.ViewModels.FilterViewModels
{
    public class LeaveRequestFilterViewModel
    {
        public string? Status { get; set; }
        public string? Option { get; set; }
        public string? Direction { get; set; }
        public bool IsForFiltering { get; set; } = false;
    }
}
