# 4. Software Requirements Specification (SRS)

## Introduction
This document specifies the software requirements for TalentFlow. It covers both functional and non‑functional requirements, system constraints, and interface definitions.

## System Requirements
### Operating Environment
- **Backend**: ASP.NET Core 8+ on Linux Docker containers.
- **Frontend**: Angular 17+ SPA served via Nginx.
- **Database**: SQL Server 2022 (or Azure SQL Database).
- **Background Processing**: Hangfire (in‑process or separate worker).
- **File Storage**: Azure Blob Storage (MinIO for local development).
- **Caching**: Redis (for distributed caching in production).
- **Search**: Future Elasticsearch for full‑text search.

### Hardware Sizing (Production Estimate)
- API servers: 2–4 vCPUs, 4–8 GB RAM, auto‑scaling to 10 instances.
- SQL Database: 100 DTU or equivalent (Azure SQL Standard S3), elastic pool for multi‑tenant.
- Blob Storage: Standard hot tier.

## Functional Requirements

### FR-01 Tenant Management
- FR-01-01: The system shall allow a platform admin to create a new tenant with a unique subdomain, company name, and admin email.
- FR-01-02: Upon creation, default roles (TenantAdmin, Recruiter, HiringManager, Interviewer) and a default pipeline template shall be provisioned.
- FR-01-03: Tenant admin can update company branding (logo, primary color), timezone, and notification defaults.
- FR-01-04: Tenant admin can manage users within the tenant: create, deactivate, assign roles.
- FR-01-05: The system shall enforce tenant isolation: all API requests must include a valid `tenant_id` claim in the JWT, and all data queries shall automatically filter by the current tenant’s ID.

### FR-02 Job Management
- FR-02-01: Authorized users can create, update, close, and soft‑delete job requisitions.
- FR-02-02: A job record includes: title, description, department, location, employment type (Full‑time, Part‑time, Contract, etc.), salary range (optional), custom fields (key‑value JSON), and status (Draft, Open, Closed, Cancelled).
- FR-02-03: Each job must be associated with a `WorkflowTemplate` upon creation. The template defines the stages the candidates will go through.

### FR-03 Candidate Management
- FR-03-01: Recruiters can create candidates manually, import via CSV, or (future) receive via API.
- FR-03-02: Candidate profile: firstName, lastName, email, phone, resume file (uploaded to blob), skills (JSON array), source (e.g., Referral, LinkedIn), notes.
- FR-03-03: A candidate can be linked to multiple jobs via separate applications.
- FR-03-04: GDPR compliance: candidate can be anonymized (PII fields cleared) or permanently deleted (cascade soft‑delete to applications, interviews, etc.). An audit trail of the deletion request is kept.

### FR-04 Recruitment Workflow Engine
- FR-04-01: Tenant admins can create `WorkflowTemplate` entities with an ordered list of stages. Each stage has a `name` (string), `order`, `allowedNextStages` (array of stage names), and `requiredPermission` (the permission needed to move a candidate into/out of this stage).
- FR-04-02: Templates can be edited, but existing job instances retain their current pipeline definition (snapshot at job creation).
- FR-04-03: When a candidate is moved from one stage to another, a `StageTransition` record is created with: applicationId, fromStage, toStage, transitionedByUserId, transitionedAt, and optional comment.
- FR-04-04: The system shall validate that the transition is allowed by the pipeline definition and that the user has the required permission. If a stage requires mandatory feedback (e.g., "Interview Score" required to exit "Technical Interview"), the system shall prompt the user to complete it before allowing the move.

### FR-05 Interview & Assessment
- FR-05-01: Users can schedule interviews with date/time, location (or virtual link), interview type, and assigned interviewers.
- FR-05-02: After the interview, assigned interviewers can submit `InterviewFeedback` containing a numeric score (1‑5), strengths, weaknesses, and overall recommendation (Hire/No‑Hire).
- FR-05-03: Admins can create `Assessment` entities with a title, description, passing score, and a set of questions (multiple‑choice/text, stored as JSON).
- FR-05-04: An assessment can be assigned to a candidate; their `AssessmentSubmission` records their answers and auto‑calculated score (for multiple‑choice). Manual scoring is also supported.

### FR-06 Offer Management
- FR-06-01: Authorized users can generate an `Offer` for a candidate’s application. Offer includes salary, start date, benefits description, expiration date, and status.
- FR-06-02: An approval workflow can be configured: offer must be approved by one or more approvers (e.g., HR, Department Head). `OfferApproval` records track each approver’s decision.
- FR-06-03: Once approved, the offer can be sent to the candidate (email). Status transitions: Draft → PendingApproval → Approved → Sent → Accepted/Declined/Expired.
- FR-06-04: If candidate accepts, the application status changes to "Hired".

### FR-07 Notifications
- FR-07-01: Real‑time in‑app notifications using SignalR for events: stage changed, interview assigned, offer approval requested.
- FR-07-02: Email notifications for the same events via Hangfire background jobs. Email templates stored as HTML in the database per tenant.
- FR-07-03: Users can manage notification preferences: enable/disable specific types (e.g., "Don't email me for stage changes").

### FR-08 Audit Logging
- FR-08-01: All Create, Update, Delete operations on core entities (Job, Candidate, Application, StageTransition, Offer, User) shall generate immutable `AuditLog` records.
- FR-08-02: Each audit record includes: TenantId, EntityType, EntityId, Action (Created/Updated/Deleted), ActorUserId, Timestamp, OldValues (JSON), NewValues (JSON).
- FR-08-03: Audit logs are queryable by tenant admins through a dedicated UI with filters (entity type, user, date range). No user (including admins) can modify or delete audit entries.

### FR-09 Reporting & Analytics
- FR-09-01: Predefined dashboards: Time‑to‑Hire trend, Pipeline Funnel, Source Effectiveness, Recruiter Performance, Offer Acceptance Rate.
- FR-09-02: All dashboards support filters: date range, department, job, recruiter.
- FR-09-03: Users can export dashboard data as CSV and chart images as PNG.
- FR-09-04: A scheduled report feature (future) to email weekly summaries to HR directors.

## Non‑Functional Requirements
### NFR-01 Performance
- API: 95th percentile response time < 200ms for reads, < 500ms for writes under load of 500 concurrent users.
- Database: Query execution time < 50ms for common queries (pipeline board, candidate list).
- File uploads: Resume upload (10MB max) completed within 5 seconds.

### NFR-02 Scalability
- The API layer shall be stateless, enabling horizontal scaling by adding more container instances.
- Database shall support elastic pool for tenants, or read replicas for reporting queries.
- SignalR shall use a backplane (Azure SignalR Service) to enable scale‑out.
- Hangfire shall use SQL Server as storage, with option to move to Redis for higher throughput.

### NFR-03 Security (see Security Design Document)
- All endpoints require authentication except auth endpoints.
- Data isolation per tenant; no cross‑tenant access possible.
- Input validation on all endpoints; file upload restrictions.
- Rate limiting (100 requests/min per user, burst 20).

### NFR-04 Availability
- Target 99.9% uptime (43.8 min downtime per month).
- Database: Azure SQL with active geo‑replication for DR.
- Backend: Deployed across multiple availability zones.

### NFR-05 Maintainability
- Code coverage >80% for domain and application layers.
- All public APIs documented with Swagger/OpenAPI.
- Architecture follows Clean Architecture principles; modules are loosely coupled.

### NFR-06 Compliance
- GDPR: Data residency option, right to erasure via anonymization or deletion.
- Audit trail maintained for 7 years.
- Password complexity enforced; passphrases encouraged.

## Security Requirements (SRS subset)
- Communication over HTTPS only.
- Passwords hashed with BCrypt (work factor 12).
- JWT signed with RSA asymmetric keys (HS256 acceptable for dev).
- Refresh tokens stored hashed (SHA256) in database; rotation on use.
- All file uploads validated for content type and maximum size; stored in private blob container.
- Anti‑XSS: CSP headers, Angular sanitization.
- SQL injection prevention via EF Core parameterized queries.
- Cross‑tenant access prevention: Global query filter + `ICurrentTenantService` immutable after middleware.

## Data Requirements
- **Master Data**: Tenants, Users, Roles, Permissions.
- **Transactional Data**: Jobs, Candidates, Applications, StageTransitions, Interviews, Assessments, Offers.
- **Reference Data**: Pipeline templates, assessment question banks.
- **Analytical Data**: Denormalized aggregations (materialized views or separate tables) for fast dashboard queries.
- **File Storage**: Resumes, offer letters, assessment attachments. Metadata in DB, files in Azure Blob.

## Integration Requirements
- Email: SMTP or SendGrid/Mailgun API (abstraction layer).
- Blob Storage: Azure Blob SDK (or MinIO for dev).
- Future: HRIS webhooks, calendar APIs (Microsoft Graph, Google Calendar), job board posting APIs (Indeed, LinkedIn).

## External Interfaces
- RESTful API (see API Design Document).
- SignalR Hub: `/hubs/notifications` – protected with JWT, tenant scoped.
- Hangfire Dashboard: Internal admin only, path `/hangfire` restricted to platform admins.

## Reporting Requirements
The system shall provide a reporting module with at least the following views:
- Time‑to‑Hire over time (line chart).
- Pipeline Funnel (bar chart with conversion percentages).
- Source Effectiveness (pie chart).
- Recruiter Activity (bar chart: candidates processed per recruiter).
- Offer Acceptance Rate (gauge or trend).