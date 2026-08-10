using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

using TalentFlow.Application.Contracts.Persistence;
using TalentFlow.Application.Features.Departments.DTOs;
using TalentFlow.Application.Interfaces;
using TalentFlow.Application.Responses;

namespace TalentFlow.Application.Features.Departments.Queries.GetDepartments
{
    public class GetDepartmentsQueryHandler : IRequestHandler<GetDepartmentsQuery, BaseCommandResponse>
    {

        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;
        private readonly ICurrentTenantService currentTenantService ;
        private readonly ILogger<GetDepartmentsQueryHandler> logger;


        public GetDepartmentsQueryHandler(IDepartmentRepository departmentRepository, IMapper mapper, IMemoryCache cache, ICurrentTenantService currentTenantService, ILogger<GetDepartmentsQueryHandler> logger)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
            this.cache = cache;
            this.currentTenantService = currentTenantService;
            this.logger = logger;
        }

        public async Task<BaseCommandResponse> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"Departments_{currentTenantService.TenantId}";
            try
            {
                if (cache.TryGetValue(cacheKey, out List<DepartmentDto> departments))
                {
                    logger.LogInformation("Departments loaded from cache.");
                    return new BaseCommandResponse
                    {
                        Success = true,
                        Data = departments
                    };
                }

                var res = await departmentRepository.GetAllAsync();
                if (!res.Any())
                {

                    return new BaseCommandResponse
                    {
                        Success = false,
                        Message = "No departments found."
                    };
                }
                logger.LogInformation("Departments loaded from database.");
                var map = mapper.Map<List<DepartmentDto>>(res);
                var r = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                cache.Set(cacheKey, map,r);
                return new BaseCommandResponse
                {
                    Data = map,
                    Success = true,

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
