using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Departments.Commands.CreateDepartment;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Application.Features.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, BaseCommandResponse>
    {
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;
        private readonly ICurrentTenantService currentTenantService;
        private readonly ILogger<UpdateDepartmentCommandHandler> logger;

        private readonly IUnitOfWork unitOfWork;

        public UpdateDepartmentCommandHandler(IMapper mapper, IMemoryCache cache, ICurrentTenantService currentTenantService, ILogger<UpdateDepartmentCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.cache = cache;
            this.currentTenantService = currentTenantService;
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Departments_{currentTenantService.TenantId}";

            try
            {
                var department = await unitOfWork.Departments.GetByIdAsync(request.id);

                if (department == null)
                {
                    return new BaseCommandResponse
                    {
                        Success = false,
                        Message = "Department not found."
                    };
                }

                if (department.TenantId != currentTenantService.TenantId)
                {
                    return new BaseCommandResponse
                    {
                        Success = false,
                        Message = "Unauthorized."
                    };
                }
                mapper.Map(request, department);



                await unitOfWork.Departments.UpdateAsync(department);
                await unitOfWork.CompleteAsync();
                cache.Remove(cacheKey);
                logger.LogInformation("Department updated for Tenant {TenantId}", currentTenantService.TenantId);
                return new BaseCommandResponse
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating department.");

                return new BaseCommandResponse
                {
                    Success = false,
                    Message = "Error updating department."
                };
            }

        }
    }
}
