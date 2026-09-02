using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Departments.Queries.GetDepartments;
using TalentFlow.Application.Contracts.Infra;
using TalentFlow.Application.Responses;
using TalentFlow.Domain.Entities.RecruitmentModule;

namespace TalentFlow.Application.Features.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, BaseCommandResponse<bool>>
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;
        private readonly ICurrentTenantService currentTenantService;
        private readonly ILogger<CreateDepartmentCommandHandler> logger;

        private readonly IUnitOfWork unitOfWork;

        public CreateDepartmentCommandHandler(IDepartmentRepository departmentRepository, IMapper mapper, IMemoryCache cache, ICurrentTenantService currentTenantService, ILogger<CreateDepartmentCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
            this.cache = cache;
            this.currentTenantService = currentTenantService;
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse<bool>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Departments_{currentTenantService.TenantId}";

            try
            {
                var mapp = mapper.Map<Department>(request);
                mapp.TenantId = currentTenantService.TenantId;
                var res = await unitOfWork.Departments.AddAsync(mapp);
                await unitOfWork.CompleteAsync();
                cache.Remove(cacheKey);
                logger.LogInformation(
    "Department created for Tenant {TenantId}",
    currentTenantService.TenantId);
                return new BaseCommandResponse<bool>
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception in {Handler}", nameof(CreateDepartmentCommandHandler));
                return new BaseCommandResponse<bool>
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
