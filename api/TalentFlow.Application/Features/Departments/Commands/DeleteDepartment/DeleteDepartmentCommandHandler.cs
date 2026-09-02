using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Departments.Commands.CreateDepartment;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Application.Features.Departments.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, BaseCommandResponse<bool>>
    {
        private readonly IMemoryCache cache;
        private readonly ICurrentTenantService currentTenantService;
        private readonly ILogger<DeleteDepartmentCommandHandler> logger;

        private readonly IUnitOfWork unitOfWork;

        public DeleteDepartmentCommandHandler( IMemoryCache cache, ICurrentTenantService currentTenantService, ILogger<DeleteDepartmentCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            this.cache = cache;
            this.currentTenantService = currentTenantService;
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse<bool>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Departments_{currentTenantService.TenantId}";

            try
            {
                var department = await unitOfWork.Departments.GetByIdAsync(request.Id);

                if (department == null)
                {
                    return new BaseCommandResponse<bool>
                    {
                        Success = false,
                        Message = "Department not found."
                    };
                }

                if (department.TenantId != currentTenantService.TenantId)
                {
                    return new BaseCommandResponse<bool>
                    {
                        Success = false,
                        Message = "Unauthorized."
                    };
                }



                await unitOfWork.Departments.DeleteAsync(department);
                await unitOfWork.CompleteAsync();
                cache.Remove(cacheKey);
                logger.LogInformation("Department {DepartmentId} deleted for Tenant {TenantId}", currentTenantService.TenantId);
                return new BaseCommandResponse<bool>
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error deleting department.");

                return new BaseCommandResponse<bool>
                {
                    Success = false,
                    Message = "Error Dleting department."
                };
            }

        }
    }
}
