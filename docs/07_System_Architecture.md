# 07-System-Architecture.md

# TalentFlow System Architecture Document (Multi-Tenant SaaS)

## Document Information

| Field              | Value                     |
| ------------------ | ------------------------- |
| Project            | TalentFlow                |
| Version            | 1.0                       |
| Architecture Style | Clean Architecture + CQRS |
| System Type        | Multi-Tenant SaaS         |
| Backend            | ASP.NET Core Web API      |
| Frontend           | Angular                   |
| Database           | SQL Server                |
| Authentication     | JWT + Refresh Tokens      |

---

# 1. Architecture Overview

## Vision

TalentFlow is designed as a Multi-Tenant SaaS Applicant Tracking System (ATS) where multiple companies can use the same application while their data remains completely isolated.

Examples:

```text
TalentFlow

├── Company A
│   ├── Recruiters
│   ├── Jobs
│   └── Candidates
│
├── Company B
│   ├── Recruiters
│   ├── Jobs
│   └── Candidates
│
└── Company C
    ├── Recruiters
    ├── Jobs
    └── Candidates
```

Each company only sees its own data.

---

# 2. Architecture Principles

The system follows:

* Clean Architecture
* SOLID Principles
* CQRS Pattern
* Domain Driven Design
* Dependency Injection
* Repository Pattern
* Event-Driven Notifications
* Multi-Tenant Data Isolation

---

# 3. High-Level Architecture

```text
┌─────────────────────────────┐
│        Angular SPA          │
└──────────────┬──────────────┘
               │ HTTPS
               ▼
┌─────────────────────────────┐
│ ASP.NET Core Web API        │
│ Authentication              │
│ Authorization               │
│ Tenant Resolution           │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│ Application Layer           │
│ Commands                    │
│ Queries                     │
│ Validators                  │
│ DTOs                        │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│ Domain Layer                │
│ Entities                    │
│ Aggregates                  │
│ Business Rules              │
│ Domain Events               │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│ Infrastructure Layer        │
│ EF Core                     │
│ Repositories                │
│ Email Service               │
│ File Storage                │
│ SignalR                     │
│ Hangfire                    │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│ SQL Server                  │
└─────────────────────────────┘
```

---

# 4. Multi-Tenant Strategy

## Selected Strategy

### Shared Database + Shared Schema

All tenants share:

* Same Application
* Same Database
* Same Schema

Data isolation is achieved using:

```sql
TenantId
```

in every business table.

Example:

```sql
Jobs
(
    Id UNIQUEIDENTIFIER,
    TenantId UNIQUEIDENTIFIER,
    Title NVARCHAR(255)
)
```

---

# 5. Why This Strategy?

Advantages:

* Easy deployment
* Low hosting cost
* Easy maintenance
* Perfect for startups
* Suitable for portfolio projects

Disadvantages:

* Requires strong tenant filtering
* More complex security

---

# 6. Alternative Tenant Strategies

## Shared Database / Separate Schema

```text
Database

CompanyA.Jobs
CompanyB.Jobs
CompanyC.Jobs
```

Pros:

* Better isolation

Cons:

* More maintenance

---

## Separate Database Per Tenant

```text
CompanyA_DB
CompanyB_DB
CompanyC_DB
```

Pros:

* Maximum isolation

Cons:

* Expensive
* Operational complexity

---

# 7. Tenant Resolution

## Authentication Flow

1. User logs in
2. JWT token issued
3. Token contains:

```json
{
  "userId": "123",
  "tenantId": "456",
  "role": "Recruiter"
}
```

4. API extracts TenantId
5. All queries filtered automatically

---

# 8. Tenant Isolation

Every query must include:

```csharp
.Where(x => x.TenantId == CurrentTenantId)
```

Example:

```csharp
var jobs = await _context.Jobs
    .Where(x => x.TenantId == tenantId)
    .ToListAsync();
```

---

# 9. Clean Architecture Layers

## Presentation Layer

### Angular

Responsibilities:

* UI Rendering
* Forms
* Routing
* State Management
* Authentication

Technologies:

* Angular
* Angular Material
* NgRx

---

## API Layer

Responsibilities:

* Request Handling
* Authentication
* Authorization
* Tenant Resolution

Technologies:

* ASP.NET Core
* JWT

---

## Application Layer

Responsibilities:

* Use Cases
* Commands
* Queries
* Validation

Examples:

```text
CreateJobCommand
UpdateCandidateCommand
ScheduleInterviewCommand
```

---

## Domain Layer

Responsibilities:

* Business Logic
* Domain Rules
* Domain Events

Contains:

```text
Job
Candidate
Application
Interview
Assessment
```

No external dependencies allowed.

---

## Infrastructure Layer

Responsibilities:

* SQL Server Access
* Email Sending
* File Storage
* Background Jobs

Technologies:

* EF Core
* SQL Server
* Hangfire
* SignalR

---

# 10. CQRS Design

## Commands

Modify data.

Examples:

```text
CreateJob
UpdateJob
MoveCandidateStage
ScheduleInterview
SubmitAssessment
```

---

## Queries

Read data.

Examples:

```text
GetJobs
GetCandidates
GetPipeline
GetDashboardMetrics
```

---

# 11. Service Boundaries

## Identity Service

Handles:

* Login
* Registration
* Roles
* Permissions

---

## Recruitment Service

Handles:

* Jobs
* Candidates
* Applications

---

## Assessment Service

Handles:

* Tests
* Questions
* Results

---

## Interview Service

Handles:

* Scheduling
* Feedback
* Scorecards

---

## Notification Service

Handles:

* Emails
* In-App Notifications

---

## Reporting Service

Handles:

* Dashboards
* KPIs
* Analytics

---

# 12. Event-Driven Architecture

Domain Events:

```text
CandidateAppliedEvent
CandidateMovedStageEvent
InterviewScheduledEvent
AssessmentCompletedEvent
OfferGeneratedEvent
```

Example:

```text
Candidate Applied
        │
        ▼
CandidateAppliedEvent
        │
        ▼
Notification Handler
        │
        ▼
Send Email
```

---

# 13. File Storage Architecture

Files:

* CVs
* Certificates
* Portfolios

Storage Options:

### Development

```text
Local Storage
```

### Production

```text
Azure Blob Storage
AWS S3
```

Database stores:

```text
File Metadata Only
```

Not actual file content.

---

# 14. Background Processing

Using Hangfire.

Tasks:

* Reminder Emails
* Interview Notifications
* Assessment Deadlines
* Cleanup Jobs

---

# 15. Real-Time Features

Using SignalR.

Features:

* Live candidate movement
* Real-time notifications
* Dashboard updates

---

# 16. Security Architecture

Authentication:

```text
JWT + Refresh Tokens
```

Authorization:

```text
RBAC
```

Roles:

```text
SuperAdmin
TenantAdmin
Recruiter
HiringManager
Interviewer
Candidate
```

---

# 17. Deployment Architecture

```text
Internet
    │
    ▼
Angular SPA
    │
    ▼
ASP.NET Core API
    │
    ▼
SQL Server
```

Optional Production:

```text
Cloud Load Balancer
        │
        ▼
API Instance 1
API Instance 2
API Instance 3
        │
        ▼
SQL Server
```

---

# 18. Scalability Targets

Target Support:

* 1,000+ tenants
* 100,000+ candidates
* 10,000+ jobs
* 1,000+ concurrent users

---

# 19. Architectural Decisions

| Decision                      | Reason                     |
| ----------------------------- | -------------------------- |
| Clean Architecture            | Maintainability            |
| CQRS                          | Separation of Reads/Writes |
| Shared Database Multi-Tenancy | Cost Effective             |
| JWT Authentication            | Stateless Security         |
| SignalR                       | Real-Time Updates          |
| Hangfire                      | Background Processing      |
| SQL Server                    | Relational Data            |
| Angular                       | Enterprise Frontend        |

---

# 20. Future Evolution

Version 2:

* Subscription Billing
* Tenant Provisioning
* AI Candidate Matching
* Resume Parsing
* Candidate Ranking
* Azure AD Integration
* SSO
* Advanced Analytics

This architecture provides a production-grade foundation capable of evolving from a portfolio project into a real SaaS recruitment platform.
