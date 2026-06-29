using System;
using TalentFlow.Domain.Common;

namespace TalentFlow.Domain.Entities.RecruitmentModule;

public class CandidateDocument : BaseEntity
{
    public Guid CandidateId { get; set; }
    public string FileName { get; set; } = default!;
    public string FileUrl { get; set; } = default!;
    public string FileType { get; set; } = default!;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
