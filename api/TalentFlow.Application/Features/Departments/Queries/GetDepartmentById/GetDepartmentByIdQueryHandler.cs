using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Departments.DTOs;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Application.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentQuery, BaseCommandResponse<DepartmentDto>>
    {
        private readonly ILogger<GetDepartmentByIdQueryHandler> logger;

        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public GetDepartmentByIdQueryHandler(IDepartmentRepository departmentRepository, IMapper mapper, ILogger<GetDepartmentByIdQueryHandler> logger)
        {
            this.logger = logger;
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<BaseCommandResponse<DepartmentDto>> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {Handler}", nameof(GetDepartmentByIdQueryHandler));
           try
            {

            var res = await departmentRepository.GetByIdAsync(request.Id);
                if (res == null)
                    return new BaseCommandResponse<DepartmentDto>
                    {
                        Message = "Department not found",
                        Success=false,
                        
                    };
                var map = mapper.Map<DepartmentDto>(res);
                return new BaseCommandResponse<DepartmentDto>
                {
                    Data = map,
                    Success=true,

                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception in {Handler}", nameof(GetDepartmentByIdQueryHandler));
                return new BaseCommandResponse<DepartmentDto>
                {
                    Success = false,
                    Message = ex.Message

                };
            }

        }
    }
}
