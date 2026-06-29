# 08-Database-Design.md

# TalentFlow Database Design Document

## Document Information

| Field        | Value                          |
| ------------ | ------------------------------ |
| Project      | TalentFlow                     |
| Version      | 1.0                            |
| Database     | SQL Server                     |
| Architecture | Multi-Tenant SaaS              |
| Strategy     | Shared Database, Shared Schema |

---

# 1. Database Design Goals

The database must support:

* Multi-Tenant SaaS Architecture
* Recruitment Workflows
* Candidate Management
* Interview Management
* Technical Assessments
* Analytics & Reporting
* Audit Logging
* Future AI Features

---

# 2. Multi-Tenant Strategy

## Tenant Isolation

Every business entity contains:

```sql
TenantId UNIQUEIDENTIFIER
```

Example:

```sql
Jobs
(
    Id UNIQUEIDENTIFIER,
    TenantId UNIQUEIDENTIFIER,
    Title NVARCHAR(255)
)
```

All queries are filtered by:

```sql
TenantId
```

This ensures:

* Company A cannot see Company B data.
* Shared database remains secure.
* Low infrastructure cost.

---

# 3. Database Modules

```text
Identity Module

Tenant Module

Recruitment Module

Workflow Module

Interview Module

Assessment Module

Notification Module

Audit Module

Reporting Module
```

---

# 4. Tenant Module

## Tenants

Represents companies using TalentFlow.

### Columns

| Column           | Type                |
| ---------------- | ------------------- |
| Id               | UNIQUEIDENTIFIER PK |
| Name             | NVARCHAR(200)       |
| Slug             | NVARCHAR(100)       |
| SubscriptionPlan | NVARCHAR(50)        |
| IsActive         | BIT                 |
| CreatedAt        | DATETIME2           |
| UpdatedAt        | DATETIME2           |

---

## TenantSettings

Stores company preferences.

### Columns

| Column         | Type                |
| -------------- | ------------------- |
| Id             | UNIQUEIDENTIFIER PK |
| TenantId       | FK                  |
| CompanyLogoUrl | NVARCHAR(500)       |
| PrimaryColor   | NVARCHAR(20)        |
| TimeZone       | NVARCHAR(100)       |
| DateFormat     | NVARCHAR(50)        |

---

# 5. Identity Module

## Users

### Columns

| Column       | Type                |
| ------------ | ------------------- |
| Id           | UNIQUEIDENTIFIER PK |
| TenantId     | FK                  |
| FirstName    | NVARCHAR(100)       |
| LastName     | NVARCHAR(100)       |
| Email        | NVARCHAR(255)       |
| PasswordHash | NVARCHAR(MAX)       |
| IsActive     | BIT                 |
| LastLoginAt  | DATETIME2           |
| CreatedAt    | DATETIME2           |

---

## Roles

### Columns

| Column      | Type          |
| ----------- | ------------- |
| Id          | INT PK        |
| Name        | NVARCHAR(50)  |
| Description | NVARCHAR(255) |

Examples:

```text
TenantAdmin
Recruiter
HiringManager
Interviewer
Candidate
```

---

## Permissions

### Columns

| Column      | Type          |
| ----------- | ------------- |
| Id          | INT PK        |
| Name        | NVARCHAR(100) |
| Description | NVARCHAR(255) |

Examples:

```text
CreateJob
DeleteJob
ManageUsers
ViewReports
```

---

## UserRoles

| UserId | FK |
| ------ | -- |
| RoleId | FK |

---

## RolePermissions

| RoleId       | FK |
| ------------ | -- |
| PermissionId | FK |

---

# 6. Recruitment Module

## Departments

### Columns

| Column   | Type                |
| -------- | ------------------- |
| Id       | UNIQUEIDENTIFIER PK |
| TenantId | FK                  |
| Name     | NVARCHAR(100)       |

Examples:

```text
Engineering
HR
Marketing
Sales
```

---

## Skills

### Columns

| Column | Type                |
| ------ | ------------------- |
| Id     | UNIQUEIDENTIFIER PK |
| Name   | NVARCHAR(100)       |

Examples:

```text
C#
Angular
SQL Server
Docker
Azure
```

---

## Jobs

### Columns

| Column          | Type                |
| --------------- | ------------------- |
| Id              | UNIQUEIDENTIFIER PK |
| TenantId        | FK                  |
| DepartmentId    | FK                  |
| Title           | NVARCHAR(255)       |
| Description     | NVARCHAR(MAX)       |
| EmploymentType  | NVARCHAR(50)        |
| ExperienceLevel | NVARCHAR(50)        |
| SalaryMin       | DECIMAL             |
| SalaryMax       | DECIMAL             |
| Status          | NVARCHAR(50)        |
| OpenDate        | DATETIME2           |
| CloseDate       | DATETIME2           |
| CreatedByUserId | FK                  |
| CreatedAt       | DATETIME2           |

---

## JobSkills

### Columns

| Column        | Type    |
| ------------- | ------- |
| JobId         | FK      |
| SkillId       | FK      |
| RequiredLevel | TINYINT |
| IsMandatory   | BIT     |

---

## Candidates

### Columns

| Column            | Type                |
| ----------------- | ------------------- |
| Id                | UNIQUEIDENTIFIER PK |
| TenantId          | FK                  |
| FirstName         | NVARCHAR(100)       |
| LastName          | NVARCHAR(100)       |
| Email             | NVARCHAR(255)       |
| PhoneNumber       | NVARCHAR(50)        |
| LinkedInUrl       | NVARCHAR(500)       |
| PortfolioUrl      | NVARCHAR(500)       |
| CurrentPosition   | NVARCHAR(255)       |
| YearsOfExperience | INT                 |
| Source            | NVARCHAR(100)       |
| CreatedAt         | DATETIME2           |

---

## CandidateSkills

### Columns

| Column      | Type    |
| ----------- | ------- |
| CandidateId | FK      |
| SkillId     | FK      |
| SkillLevel  | TINYINT |

---

## CandidateExperiences

### Columns

| Column      | Type                |
| ----------- | ------------------- |
| Id          | UNIQUEIDENTIFIER PK |
| CandidateId | FK                  |
| CompanyName | NVARCHAR(255)       |
| JobTitle    | NVARCHAR(255)       |
| StartDate   | DATE                |
| EndDate     | DATE                |
| Description | NVARCHAR(MAX)       |

---

## CandidateEducations

### Columns

| Column      | Type                |
| ----------- | ------------------- |
| Id          | UNIQUEIDENTIFIER PK |
| CandidateId | FK                  |
| Institution | NVARCHAR(255)       |
| Degree      | NVARCHAR(255)       |
| StartDate   | DATE                |
| EndDate     | DATE                |

---

## CandidateDocuments

### Columns

| Column      | Type                |
| ----------- | ------------------- |
| Id          | UNIQUEIDENTIFIER PK |
| CandidateId | FK                  |
| FileName    | NVARCHAR(255)       |
| FileUrl     | NVARCHAR(1000)      |
| FileType    | NVARCHAR(50)        |
| UploadedAt  | DATETIME2           |

---

## Applications

### Columns

| Column         | Type                |
| -------------- | ------------------- |
| Id             | UNIQUEIDENTIFIER PK |
| TenantId       | FK                  |
| CandidateId    | FK                  |
| JobId          | FK                  |
| CurrentStageId | FK                  |
| Status         | NVARCHAR(50)        |
| AppliedAt      | DATETIME2           |

---

# 7. Workflow Module

## Pipelines

### Columns

| Column   | Type                |
| -------- | ------------------- |
| Id       | UNIQUEIDENTIFIER PK |
| TenantId | FK                  |
| Name     | NVARCHAR(100)       |

---

## PipelineStages

### Columns

| Column     | Type                |
| ---------- | ------------------- |
| Id         | UNIQUEIDENTIFIER PK |
| PipelineId | FK                  |
| Name       | NVARCHAR(100)       |
| StageOrder | INT                 |

Examples:

```text
Applied
Screening
Assessment
Interview
Offer
Hired
```

---

## ApplicationStageHistory

### Columns

| Column          | Type                |
| --------------- | ------------------- |
| Id              | UNIQUEIDENTIFIER PK |
| ApplicationId   | FK                  |
| FromStageId     | FK                  |
| ToStageId       | FK                  |
| ChangedByUserId | FK                  |
| Notes           | NVARCHAR(MAX)       |
| ChangedAt       | DATETIME2           |

---

## CandidateActivities

### Columns

| Column          | Type                |
| --------------- | ------------------- |
| Id              | UNIQUEIDENTIFIER PK |
| CandidateId     | FK                  |
| ActivityType    | NVARCHAR(100)       |
| Description     | NVARCHAR(MAX)       |
| CreatedByUserId | FK                  |
| CreatedAt       | DATETIME2           |

---

# 8. Interview Module

## Interviews

### Columns

| Column          | Type                |
| --------------- | ------------------- |
| Id              | UNIQUEIDENTIFIER PK |
| ApplicationId   | FK                  |
| InterviewType   | NVARCHAR(50)        |
| ScheduledAt     | DATETIME2           |
| DurationMinutes | INT                 |
| Status          | NVARCHAR(50)        |

---

## InterviewParticipants

### Columns

| Column      | Type         |
| ----------- | ------------ |
| InterviewId | FK           |
| UserId      | FK           |
| Role        | NVARCHAR(50) |

---

## InterviewCriteria

### Columns

| Column   | Type                |
| -------- | ------------------- |
| Id       | UNIQUEIDENTIFIER PK |
| TenantId | FK                  |
| Name     | NVARCHAR(100)       |
| MaxScore | INT                 |

Examples:

```text
Communication
Leadership
Problem Solving
Technical Skills
```

---

## InterviewFeedback

### Columns

| Column            | Type                |
| ----------------- | ------------------- |
| Id                | UNIQUEIDENTIFIER PK |
| InterviewId       | FK                  |
| SubmittedByUserId | FK                  |
| Recommendation    | NVARCHAR(50)        |
| Summary           | NVARCHAR(MAX)       |
| SubmittedAt       | DATETIME2           |

---

## InterviewScores

### Columns

| Column     | Type                |
| ---------- | ------------------- |
| Id         | UNIQUEIDENTIFIER PK |
| FeedbackId | FK                  |
| CriteriaId | FK                  |
| Score      | INT                 |

---

# 9. Assessment Module

## Assessments

### Columns

| Column          | Type                |
| --------------- | ------------------- |
| Id              | UNIQUEIDENTIFIER PK |
| TenantId        | FK                  |
| JobId           | FK                  |
| Title           | NVARCHAR(255)       |
| PassingScore    | INT                 |
| DurationMinutes | INT                 |

---

## Questions

### Columns

| Column       | Type                |
| ------------ | ------------------- |
| Id           | UNIQUEIDENTIFIER PK |
| AssessmentId | FK                  |
| QuestionType | NVARCHAR(50)        |
| Content      | NVARCHAR(MAX)       |
| Points       | INT                 |

---

## QuestionOptions

### Columns

| Column     | Type                |
| ---------- | ------------------- |
| Id         | UNIQUEIDENTIFIER PK |
| QuestionId | FK                  |
| OptionText | NVARCHAR(MAX)       |
| IsCorrect  | BIT                 |

---

## AssessmentAssignments

### Columns

| Column        | Type                |
| ------------- | ------------------- |
| Id            | UNIQUEIDENTIFIER PK |
| AssessmentId  | FK                  |
| ApplicationId | FK                  |
| AssignedAt    | DATETIME2           |
| DueDate       | DATETIME2           |

---

## CandidateAnswers

### Columns

| Column       | Type                |
| ------------ | ------------------- |
| Id           | UNIQUEIDENTIFIER PK |
| AssignmentId | FK                  |
| QuestionId   | FK                  |
| AnswerText   | NVARCHAR(MAX)       |

---

## AssessmentResults

### Columns

| Column       | Type                |
| ------------ | ------------------- |
| Id           | UNIQUEIDENTIFIER PK |
| AssignmentId | FK                  |
| Score        | INT                 |
| Passed       | BIT                 |
| CompletedAt  | DATETIME2           |

---

# 10. Notification Module

## Notifications

### Columns

| Column    | Type                |
| --------- | ------------------- |
| Id        | UNIQUEIDENTIFIER PK |
| TenantId  | FK                  |
| UserId    | FK                  |
| Title     | NVARCHAR(255)       |
| Message   | NVARCHAR(MAX)       |
| IsRead    | BIT                 |
| CreatedAt | DATETIME2           |

---

## NotificationTemplates

### Columns

| Column   | Type                |
| -------- | ------------------- |
| Id       | UNIQUEIDENTIFIER PK |
| TenantId | FK                  |
| Name     | NVARCHAR(100)       |
| Subject  | NVARCHAR(255)       |
| Body     | NVARCHAR(MAX)       |

---

# 11. Audit Module

## AuditLogs

### Columns

| Column     | Type          |
| ---------- | ------------- |
| Id         | BIGINT PK     |
| TenantId   | FK            |
| UserId     | FK            |
| EntityName | NVARCHAR(100) |
| EntityId   | NVARCHAR(100) |
| Action     | NVARCHAR(50)  |
| OldValues  | NVARCHAR(MAX) |
| NewValues  | NVARCHAR(MAX) |
| CreatedAt  | DATETIME2     |

---

# 12. Soft Delete Strategy

All major entities contain:

```sql
IsDeleted BIT
DeletedAt DATETIME2 NULL
DeletedByUserId UNIQUEIDENTIFIER NULL
```

Soft delete applies to:

* Jobs
* Candidates
* Applications
* Assessments
* Interviews

---

# 13. Indexing Strategy

## Unique Indexes

```sql
Users(Email, TenantId)

Tenants(Slug)

Roles(Name)

Permissions(Name)
```

---

## Search Indexes

```sql
Candidates(Email)

Candidates(LastName)

Jobs(Status)

Jobs(DepartmentId)

Applications(JobId)

Applications(CandidateId)
```

---

## Reporting Indexes

```sql
Applications(Status)

Applications(AppliedAt)

ApplicationStageHistory(ChangedAt)

Interviews(ScheduledAt)
```

---

# 14. ERD Overview

```text
Tenant
 │
 ├── Users
 ├── Departments
 ├── Jobs
 ├── Candidates
 ├── Pipelines
 ├── Assessments
 ├── Notifications
 │
 ├── Jobs
 │     └── Applications
 │              ├── Interviews
 │              ├── AssessmentAssignments
 │              └── ApplicationStageHistory
 │
 ├── Candidates
 │     ├── CandidateSkills
 │     ├── CandidateExperiences
 │     ├── CandidateEducations
 │     └── CandidateDocuments
 │
 └── AuditLogs
```

---

# 15. Future Database Enhancements

### AI Features

Additional tables:

```text
ResumeParses
CandidateEmbeddings
JobEmbeddings
AIRecommendations
SkillExtractions
```

### SaaS Features

Additional tables:

```text
Subscriptions
Invoices
Payments
TenantUsage
FeatureFlags
```

This database design supports a production-grade multi-tenant ATS platform while remaining realistic to implement with ASP.NET Core, Angular, and SQL Server.
