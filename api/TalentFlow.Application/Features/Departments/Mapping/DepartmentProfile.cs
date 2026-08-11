using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Features.Departments.Commands.CreateDepartment;
using TalentFlow.Application.Features.Departments.Commands.UpdateDepartment;
using TalentFlow.Application.Features.Departments.DTOs;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Application.Features.Departments.Mapping
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>();

            CreateMap<CreateDepartmentCommand, Department>();

            CreateMap<UpdateDepartmentCommand, Department>();
        }
    }
}
