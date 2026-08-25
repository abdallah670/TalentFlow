using TalentFlow.Domain.Enums;

namespace TalentFlow.Application.Features.Job.DTOs
{
    public class GetJobDto
    {
        public Guid Id { get; set; }

        public Guid DepartmentId { get; set; }

        public string Title { get; set; } = default!;
        public string CompanyName { get; set; } = default!;
        public string Description { get; set; } = default!;

        public EmploymentType EmploymentType { get; set; }

        public ExperienceLevel ExperienceLevel { get; set; }

        public decimal SalaryMin { get; set; }

        public decimal SalaryMax { get; set; }

        public JobStatus Status { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime? CloseDate { get; set; }
    }
}