using AutoMapper;
using MVC_Practice_Project.DAL.Models;
using MVC_Practice_Project.PL.DTOs;

namespace MVC_Practice_Project.PL.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>().ReverseMap();
            //.ForMember(E => E.Name, O => O.MapFrom(C => C.Name));
            CreateMap<CreateDepartmentDto, Department>().ReverseMap();
        }
    }
}
