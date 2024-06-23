using AutoMapper;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.ViewModels;

namespace OutOfOfficeSolution.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            AllowNullCollections = true;
            AddGlobalIgnore("Item");

            CreateMap<Employee, EmployeeViewModel>().ReverseMap();

            CreateMap<Project, ProjectViewModel>().ReverseMap();

            CreateMap<LeaveRequest, LeaveRequestViewModel>().ReverseMap();
            CreateMap<ApprovalRequest, ApprovalRequestViewModel>().ReverseMap();
        }
    }
}
