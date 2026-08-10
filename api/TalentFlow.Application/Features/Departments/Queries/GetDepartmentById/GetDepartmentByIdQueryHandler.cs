using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Departments.DTOs;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Application.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentQuery, BaseCommandResponse>
    {

        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public GetDepartmentByIdQueryHandler(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
        {
           try
            {

            var res = await departmentRepository.GetByIdAsync(request.Id);
                if (res == null)
                    return new BaseCommandResponse
                    {
                        Message = "Department not found",
                        Success=false,
                        
                    };
                var map = mapper.Map<DepartmentDto>(res);
                return new BaseCommandResponse
                {
                    Data = map,
                    Success=true,

                };
            }
            catch (Exception ex)
            {
                return new BaseCommandResponse
                {
                    Success = false,
                    Message = ex.Message

                };
            }

        }
    }
}
