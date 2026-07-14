# TalentFlow — Task Breakdown & Assignment

> **Last Updated:** 2026-07-14  
> **Project:** TalentFlow — AI-Powered Hiring & Candidate Assessment Platform  
> **Team:** 2 Persons

---

## Current Project State (What Exists vs What's Missing)

### Already Built ✅

**Backend:**
- ✅ All **Domain Entities** created across 8 modules (Identity, Tenant, Recruitment, Workflow, Interview, Assessment, Notification, Audit)
- ✅ **AppDbContext** fully configured with all DbSets, soft-delete filter, audit timestamps
- ✅ **Database Migration** exists (`Init_Entities` — June 29, 2026) — tables created
- ✅ **Program.cs** fully configured (Auth/JWT, Swagger, Hangfire, Rate Limiting, CORS, Serilog, API Versioning, OAuth providers)
- ✅ **DI Registration** set up in Infrastructure, Application, and Persistence layers
- ✅ **Middleware** (Exception, RequestLogging, HangfireAuthorization)
- ✅ **Filters** (ApiExceptionFilter)
- ✅ **Application layer** contracts, models, responses, exceptions, constants
- ✅ **appsettings.json** with all configuration sections

**Frontend:**
- ✅ **Clean Architecture** scaffolding (core/data/domain/presentation layers)
- ✅ **Auth Service** with login, register, email verification, forgot/reset password, refresh token, JWT cookie storage
- ✅ **Auth Guard** + **Admin Guard** implemented
- ✅ **Token Interceptor** with auto-refresh on 401
- ✅ **Environment config** (API base URL)
- ✅ **Angular project** fully set up (standalone components)

### Not Built / Missing ❌

**Backend:**
- ❌ **Controllers** — NONE exist (no Controllers folder at all)
- ❌ **Offer Entity** — MISSING from Domain entirely (OfferModule doesn't exist, no Offer/OfferApproval entities)
- ❌ **CQRS Handlers** — NONE implemented (only MediatR registered in DI)
- ❌ **AutoMapper Profiles** — NONE created
- ❌ **FluentValidation Validators** — NONE created
- ❌ **Service implementations** — NONE (IEmailService, IBlobStorageService, ICurrentTenantService, etc.)
- ❌ **SignalR Hub** — Not implemented
- ❌ **Multi-tenant query filter** — Not yet applied (ICurrentTenantService not implemented)

**Frontend:**
- ❌ **Routes** — Empty (`app.routes.ts` has empty array)
- ❌ **Feature modules** — NONE built (no dashboard, jobs, candidates, applications, interviews, assessments, offers, analytics, admin, notifications)
- ❌ **NgRx State** — Not set up (auth currently uses signals + cookies)
- ❌ **UI Components** — NONE built (no login page, no layout, no shared components)
- ❌ **Admin Layout** — Only folder exists, no implementation
- ❌ **Domain entities/repositories/usecases** — Empty folders
- ❌ **Data models** — Only `auth.model.ts` exists

---

## Assignment Strategy

| Person | Role | Scope |
|--------|------|-------|
| **Person 1** | Full Stack Lead (Frontend Heavy) | **All Frontend (Angular)** — UI, components, state management, routing + **Backend tasks that are UI-adjacent**: Auth, Users, Roles, Tenant, Dashboard, Notifications, Search, File Upload, Offer Domain Entity |
| **Person 2** | Backend Specialist | **All remaining Backend**: Core CRUD Controllers (Jobs, Candidates, Applications, Pipelines, Interviews, Assessments), CQRS Handlers, Domain Logic, Pipeline Engine, Email Service, Audit, AI, Testing, DevOps |

---

## Phase 1 — Foundation (Weeks 1–4)

### Person 1 — Frontend + Auth/User/Tenant Backend + Offer Entity

| # | Task | Layer | Details |
|---|------|-------|---------|
| 1.1 | **Fix Auth Guard** | Frontend | Current guard blocks all non-admin users. Fix to allow authenticated users with any role, add proper `RoleGuard` for admin-only routes. |
| 1.2 | **Login Page UI** | Frontend | Build `LoginComponent` with email/password form, social login buttons (Google/Facebook), validation, loading state, error display. |
| 1.3 | **Registration Page UI** | Frontend | Build `RegisterComponent` with form fields (firstName, lastName, email, userName, password), validation, success/error feedback. |
| 1.4 | **Email Verification UI** | Frontend | Build `VerifyEmailComponent` and `ResendVerificationComponent` with token handling from query params. |
| 1.5 | **Forgot/Reset Password UI** | Frontend | Build `ForgotPasswordComponent` and `ResetPasswordComponent` forms. |
| 1.6 | **Main Layout Shell** | Frontend | Build `MainLayoutComponent` with sidebar navigation, top header (user menu, notification bell placeholder, tenant branding placeholder), responsive collapse. |
| 1.7 | **App Routing** | Frontend | Set up lazy-loaded routes for all feature modules with guards. Configure path structure per Frontend Design doc. |
| 1.8 | **Auth State (NgRx)** | Frontend | Set up `AuthState` store module: actions (login, logout, refresh, register), effects, selectors for user, tenant, tokens, isAuthenticated. Migrate from signal-only approach. |
| 1.9 | **Auth Controller** | Backend | Implement `AuthController`: `POST /api/v1/auth/login`, `POST /api/v1/auth/register`, `POST /api/v1/auth/refresh-token`, `POST /api/v1/auth/logout`, `POST /api/v1/auth/change-password`, `POST /api/v1/auth/verify-email`, `POST /api/v1/auth/forgot-password`, `POST /api/v1/auth/reset-password`. |
| 1.10 | **Tenant Controller** | Backend | Implement `TenantController`: `GET /api/v1/tenants/current`, `PUT /api/v1/tenants/settings`. |
| 1.11 | **Users Controller** | Backend | Implement `UsersController`: `GET /api/v1/users`, `GET /api/v1/users/{id}`, `POST /api/v1/users`, `PUT /api/v1/users/{id}`, `PATCH /api/v1/users/{id}/disable`. |
| 1.12 | **Roles Controller** | Backend | Implement `RolesController`: `GET /api/v1/roles`, `POST /api/v1/roles`, `POST /api/v1/roles/{id}/permissions`. |
| 1.13 | **Offer Domain Entity** | Backend | **Create missing `OfferModule`** with entities: `Offer`, `OfferApproval`. Add to DbContext. Create migration. |
| 1.14 | **Admin — User Management UI** | Frontend | Build `UserManagementComponent` (user list table, create/edit dialog, role assignment). |
| 1.15 | **Admin — Role Management UI** | Frontend | Build `RoleManagementComponent` (role list, create/edit, permission assignment checkboxes). |
| 1.16 | **Admin — Tenant Settings UI** | Frontend | Build `TenantSettingsComponent` (branding: logo upload, color picker, timezone, notification defaults). |
| 1.17 | **Global Error Handler** | Frontend | Enhance existing `GlobalErrorHandler` to handle HTTP errors, show toast notifications, log errors. |

### Person 2 — Backend Infrastructure

| # | Task | Layer | Details |
|---|------|-------|---------|
| 1.18 | **Multi-Tenancy Implementation** | Backend | Implement `ICurrentTenantService` interface + implementation. Apply global query filter in `AppDbContext` to auto-filter by `TenantId` on all queries. |
| 1.19 | **Audit Service** | Backend | Implement `IAuditService` to generate `AuditLog` records on all CUD operations (EntityType, EntityId, Action, OldValues, NewValues, ActorUserId). Wire into `SaveChangesAsync` override. |
| 1.20 | **Generic Repository + Unit of Work** | Backend | Implement `IRepository<T>` with `IUnitOfWork` pattern. Include soft-delete support, tenant filtering, pagination helper. |
| 1.21 | **CQRS Handlers Skeleton** | Backend | Create base classes: `BaseCommand<T>`, `BaseQuery<T>`, `BaseHandler`. Set up FluentValidation pipeline behavior with MediatR. Create AutoMapper profiles base. |
| 1.22 | **Identity Configuration** | Backend | Configure ASP.NET Identity (Password rules, lockout, email confirmation requirements). Seed default roles (TenantAdmin, Recruiter, HiringManager, Interviewer) and admin user. |
| 1.23 | **Offer Migration** | Backend | Create EF migration for new `Offer` + `OfferApproval` entities (after Person 1 creates them). Update `AppDbContextModelSnapshot`. |
| 1.24 | **CI/CD Pipeline** | DevOps | Set up GitHub Actions: build, test (xUnit + Jest), SonarQube analysis, Docker image build/push. |
| 1.25 | **Docker Compose** | DevOps | Create `docker-compose.yml` with API, SQL Server, Redis, MinIO containers. Create `Dockerfile` for API. |

---

## Phase 2 — Recruitment Core (Weeks 5–8)

### Person 1 — Frontend (All UI)

| # | Task | Layer | Details |
|---|------|-------|---------|
| 2.1 | **Department List/Manage UI** | Frontend | Build `DepartmentListComponent` — simple CRUD table with inline edit. |
| 2.2 | **Skill Management UI** | Frontend | Build `SkillListComponent` — tag-style list with add/remove. |
| 2.3 | **Job List Page** | Frontend | Build `JobListComponent` — paginated data table (Title, Department, Status, Candidate Count, Created Date). Filters: status, department, search. |
| 2.4 | **Job Form (Create/Edit)** | Frontend | Build `JobFormComponent` — reactive form with dynamic custom fields (FormArray), pipeline template selector, validation (title required, salary validation). |
| 2.5 | **Job Detail Page** | Frontend | Build `JobDetailComponent` — job info display, assigned pipeline visualization, mini pipeline funnel chart, quick actions (publish, close). |
| 2.6 | **Candidate List Page** | Frontend | Build `CandidateListComponent` — search by name/email, filter by source/skills, paginated table with quick actions (view, delete, import). |
| 2.7 | **Candidate Profile Page** | Frontend | Build `CandidateProfileComponent` — personal info card, resume download link, skills/experience/education sections, application history timeline, current stage per application. |
| 2.8 | **Candidate Import (CSV)** | Frontend | Build `CandidateImportComponent` — file upload with drag-drop, CSV column mapping preview, validation errors display. |
| 2.9 | **Pipeline Board** | Frontend | Build `PipelineBoardComponent` — Kanban-style columns per pipeline stage. Each column shows candidate cards (name, avatar placeholder, days in stage). Manual stage move via dropdown. |
| 2.10 | **Application Detail Page** | Frontend | Build `ApplicationDetailComponent` — candidate info summary, stage history timeline, action buttons (move stage, schedule interview, generate offer). |
| 2.11 | **Stage Transition Dialog** | Frontend | Build `StageTransitionDialog` — modal showing current stage, allowed next stages dropdown, mandatory fields (if required), comment box, validation. |
| 2.12 | **Shared Components Library** | Frontend | Build `DataTableComponent` (generic: columns config, pagination, sorting), `StatusBadgeComponent` (color-coded), `FileUploadComponent` (drag-drop, progress, validation), `ConfirmDialogComponent`, `EmptyStateComponent`. |
| 2.13 | **Permission Directive** | Frontend | Build `*appIfPermission` structural directive to show/hide UI elements based on user permissions from JWT claims. |
| 2.14 | **NgRx Job State** | Frontend | Set up `JobState` store: actions (loadJobs, createJob, updateJob, deleteJob, publishJob, closeJob), effects, selectors with filter/sort/pagination support. |
| 2.15 | **NgRx Candidate State** | Frontend | Set up `CandidateState` store: actions, effects, selectors with search/filter/pagination. |
| 2.16 | **NgRx Application State** | Frontend | Set up `ApplicationState` store: pipeline data grouped by stage, current application, stage history, actions for move-stage/reject/hire. |

### Person 2 — Backend (CRUD Controllers + CQRS + Domain Logic)

| # | Task | Layer | Details |
|---|------|-------|---------|
| 2.17 | **Departments Controller + CQRS** | Backend | Implement full CRUD: `GET /api/v1/departments`, `POST /api/v1/departments`, `PUT /api/v1/departments/{id}`, `DELETE /api/v1/departments/{id}`. Commands, queries, validators, mappings. |
| 2.18 | **Skills Controller + CQRS** | Backend | Implement: `GET /api/v1/skills`, `POST /api/v1/skills`. Query/command handlers + validators. |
| 2.19 | **Jobs Controller + CQRS** | Backend | Implement: `GET /api/v1/jobs` (filtering: status, departmentId, search; sorting; pagination), `GET /api/v1/jobs/{id}`, `POST /api/v1/jobs`, `PUT /api/v1/jobs/{id}`, `PATCH /api/v1/jobs/{id}/publish`, `PATCH /api/v1/jobs/{id}/close`. Handlers: `CreateJobCommand`, `UpdateJobCommand`, `GetJobsQuery`, `GetJobByIdQuery`, `PublishJobCommand`, `CloseJobCommand`. |
| 2.20 | **Candidates Controller + CQRS** | Backend | Implement: `GET /api/v1/candidates` (search: name/email, filter: skills, experience; pagination), `GET /api/v1/candidates/{id}`, `POST /api/v1/candidates`, `PUT /api/v1/candidates/{id}`, `GET /api/v1/candidates/{id}/activities`. Handlers + validators. |
| 2.21 | **Applications Controller + CQRS** | Backend | Implement: `POST /api/v1/applications` (apply candidate to job), `GET /api/v1/applications`, `GET /api/v1/applications/{id}`, `PATCH /api/v1/applications/{id}/move-stage`, `PATCH /api/v1/applications/{id}/reject`, `PATCH /api/v1/applications/{id}/hire`. |
| 2.22 | **Pipeline Controller + CQRS** | Backend | Implement: `GET /api/v1/pipelines`, `POST /api/v1/pipelines`, `POST /api/v1/pipelines/{id}/stages`, `PATCH /api/v1/pipelines/{id}/stages/reorder`. |
| 2.23 | **Stage Transition Domain Engine** | Backend | Implement core domain service `IMoveStageService`: validate transition is allowed by pipeline definition, check user permissions, enforce mandatory feedback requirements, create `ApplicationStageHistory` record. |
| 2.24 | **CSV Import Endpoint** | Backend | Implement `POST /api/v1/candidates/import` — parse CSV, validate rows (email uniqueness, required fields), bulk insert candidates with transaction rollback on error. |
| 2.25 | **AutoMapper Profiles** | Backend | Create mapping profiles for all entities ↔ DTOs: Department, Skill, Job, Candidate, Application, Pipeline, PipelineStage. |
| 2.26 | **FluentValidation Validators** | Backend | Create validators for all commands: CreateJob, UpdateJob, CreateCandidate, UpdateCandidate, ApplyCandidate, MoveStage, CreatePipeline, AddStage, etc. |

---

## Phase 3 — Interview & Assessment (Weeks 9–12)

### Person 1 — Frontend + SignalR Backend + Notification Backend

| # | Task | Layer | Details |
|---|------|-------|---------|
| 3.1 | **Schedule Interview Dialog** | Frontend | Build `ScheduleInterviewDialog` — date/time picker, duration, location/meeting URL, interview type dropdown, interviewer multi-select with search. |
| 3.2 | **Interview Feedback Form** | Frontend | Build `InterviewFeedbackFormComponent` — numeric score (1-5), strengths textarea, weaknesses textarea, recommendation (Hire/No-Hire/ Hold). Validation. |
| 3.3 | **Assessment Builder** | Frontend | Build `AssessmentBuilderComponent` — title, description, passing score, dynamic question list (add/edit/remove questions: multiple-choice options or text answer). |
| 3.4 | **Assessment Assignment UI** | Frontend | Build `AssessmentAssignmentComponent` — assign assessment to candidate, view assignment status, due date. |
| 3.5 | **Assessment Submission View** | Frontend | Build `AssessmentSubmissionComponent` — display candidate answers next to questions, show auto-calculated score for multiple-choice, manual score input for text answers. |
| 3.6 | **Offer Creation Form** | Frontend | Build `OfferCreateComponent` — form with salary (min/max), start date, expiration date, benefits description, offer letter text editor. |
| 3.7 | **Offer Approval Component** | Frontend | Build `OfferApprovalComponent` — pending approvals list, approve/reject buttons with comment modal. |
| 3.8 | **Offer Tracker Component** | Frontend | Build `OfferTrackerComponent` — visual status timeline (Draft → PendingApproval → Approved → Sent → Accepted/Declined/Expired), with dates and actor info at each step. |
| 3.9 | **Notification Bell (SignalR)** | Frontend | Build `NotificationBellComponent` — bell icon in header with unread count badge. Subscribe to SignalR hub, update count in real-time. |
| 3.10 | **Notification List Page** | Frontend | Build `NotificationListComponent` — paginated list with type icons, message, timestamp. Mark as read individually or "Mark all as read". |
| 3.11 | **SignalR Notification Hub (Backend)** | Backend | Implement `NotificationHub` extending `Hub`. JWT authentication. Group users by tenant. Methods: `JoinGroup(tenantId)`, `SendNotification`. |
| 3.12 | **Notifications Controller (Backend)** | Backend | Implement: `GET /api/v1/notifications` (paginated), `PATCH /api/v1/notifications/{id}/read`, `PATCH /api/v1/notifications/read-all`. |
| 3.13 | **NgRx Interview State** | Frontend | Set up `InterviewState`: actions, effects (load, schedule, submitFeedback), selectors. |
| 3.14 | **NgRx Assessment State** | Frontend | Set up `AssessmentState`: actions, effects (load, create, assign, submitAnswers, getResults), selectors. |
| 3.15 | **NgRx Offer State** | Frontend | Set up `OfferState`: actions, effects (load, create, approve, reject, send, accept, decline), selectors. |
| 3.16 | **NgRx Notification State** | Frontend | Set up `NotificationState`: actions, effects (load, markRead, markAllRead, newNotification from SignalR), selectors for unread count. |

### Person 2 — Backend (Interview/Assessment/Offer Logic + Email)

| # | Task | Layer | Details |
|---|------|-------|---------|
| 3.17 | **Interviews Controller + CQRS** | Backend | Implement: `POST /api/v1/interviews` (schedule), `GET /api/v1/interviews`, `GET /api/v1/interviews/{id}`, `POST /api/v1/interviews/{id}/feedback`. Handlers: `ScheduleInterviewCommand`, `GetInterviewsQuery`, `GetInterviewByIdQuery`, `SubmitFeedbackCommand`. |
| 3.18 | **Assessments Controller + CQRS** | Backend | Implement: `POST /api/v1/assessments`, `GET /api/v1/assessments`, `POST /api/v1/assessments/{id}/questions`, `POST /api/v1/assessment-assignments`, `POST /api/v1/assessment-submissions`, `GET /api/v1/assessment-results/{id}`. Auto-score multiple-choice, manual score for text. |
| 3.19 | **Offers Controller + CQRS** | Backend | Implement: `POST /api/v1/offers`, `GET /api/v1/offers`, `GET /api/v1/offers/{id}`, `PATCH /api/v1/offers/{id}/submit-for-approval`, `PATCH /api/v1/offers/{id}/approve`, `PATCH /api/v1/offers/{id}/reject`, `PATCH /api/v1/offers/{id}/send`, `PATCH /api/v1/offers/{id}/accept`, `PATCH /api/v1/offers/{id}/decline`. |
| 3.20 | **Offer Approval Workflow** | Backend | Implement configurable approval chain: multiple approvers, `OfferApproval` records, status transition validation (only valid transitions allowed). |
| 3.21 | **Email Service** | Backend | Implement `IEmailService` with SendGrid/SMTP. Create Hangfire background jobs for async email sending on events: stage change notification, interview assignment, offer approval request, offer sent. |
| 3.22 | **Email Templates** | Backend | Store HTML templates per tenant in DB. Implement template rendering with placeholders (`{{candidateName}}`, `{{jobTitle}}`, etc.). |
| 3.23 | **AutoMapper Profiles (Phase 3)** | Backend | Create mappings for Interview, Assessment, Offer entities ↔ DTOs. |
| 3.24 | **Validators (Phase 3)** | Backend | Create FluentValidation validators: ScheduleInterview, SubmitFeedback, CreateAssessment, AddQuestion, CreateOffer, ApproveOffer, etc. |

---

## Phase 4 — Analytics & Reporting (Weeks 13–15)

### Person 1 — Frontend Dashboards + Backend Analytics Endpoints

| # | Task | Layer | Details |
|---|------|-------|---------|
| 4.1 | **Dashboard Summary Page** | Frontend | Build `DashboardComponent` — summary cards (Open Jobs, Active Candidates, Scheduled Interviews, Offers Sent) with icons and trend indicators. Quick action buttons. |
| 4.2 | **Time-to-Hire Chart** | Frontend | Build `TimeToHireChartComponent` — line chart (Chart.js/ngx-charts) with date range picker and department filter. |
| 4.3 | **Pipeline Funnel Chart** | Frontend | Build `FunnelChartComponent` — horizontal bar chart showing candidate count per stage with conversion percentages between stages. |
| 4.4 | **Source Effectiveness Chart** | Frontend | Build `SourceEffectivenessChartComponent` — pie/donut chart showing candidate distribution by source (LinkedIn, Referral, Job Board, etc.). |
| 4.5 | **Recruiter Performance Chart** | Frontend | Build `RecruiterPerformanceComponent` — bar chart: candidates processed per recruiter with time period filter. |
| 4.6 | **CSV/PNG Export** | Frontend | Add export buttons to all charts: download chart as PNG, download underlying data as CSV. |
| 4.7 | **Dashboard Controller (Backend)** | Backend | Implement `DashboardController`: `GET /api/v1/dashboard/summary`, `GET /api/v1/dashboard/funnel`, `GET /api/v1/dashboard/analytics` (time-to-hire, source effectiveness, recruiter performance). |
| 4.8 | **Analytics SQL Views** | Backend | Create denormalized SQL views for KPI calculations: `vw_TimeToHire`, `vw_PipelineFunnel`, `vw_SourceEffectiveness`, `vw_RecruiterPerformance`. |
| 4.9 | **NgRx Analytics State** | Frontend | Set up `AnalyticsState`: actions, effects (loadSummary, loadFunnel, loadAnalytics), selectors for all chart data. |

### Person 2 — Backend Audit + Frontend Admin UI

| # | Task | Layer | Details |
|---|------|-------|---------|
| 4.10 | **Audit Logs Controller** | Backend | Implement `AuditLogsController`: `GET /api/v1/audit-logs` with server-side pagination, filters (entity type, action, userId, date range from/to). |
| 4.11 | **Audit Log Viewer UI** | Frontend | Build `AuditLogViewerComponent` — paginated table with filter controls, expandable rows showing old/new values JSON diff. |

---

## Phase 5 — SaaS & Advanced Features (Weeks 16–20)

### Person 1 — Frontend

| # | Task | Layer | Details |
|---|------|-------|---------|
| 5.1 | **Tenant Branding (Dynamic CSS)** | Frontend | On login, fetch tenant settings (primary color, logo URL). Set CSS variables via `document.documentElement.style.setProperty`. Apply logo in header. |
| 5.2 | **Pipeline Template Builder UI** | Frontend | Build `PipelineTemplateBuilderComponent` — ordered list of stages with add/edit/delete/reorder (drag-and-drop or up/down buttons). Each stage: name, allowed next stages, required permission, mandatory feedback flag. |
| 5.3 | **Advanced Pipeline UI** | Frontend | Visual indicators for: parallel stages (candidate in multiple stages), conditional transitions (show condition label), automatic advancement (badge). |
| 5.4 | **Bulk Operations UI** | Frontend | Add checkbox selection to candidate list, bulk actions toolbar: "Move to Stage" dropdown, "Bulk Reject" with confirmation. |
| 5.5 | **Platform Admin Dashboard** | Frontend | Build platform-level views: all tenants table (name, subdomain, status, user count, created date), health metrics (API uptime, DB status), usage stats. |

### Person 2 — Backend

| # | Task | Layer | Details |
|---|------|-------|---------|
| 5.6 | **Subscription Plan Infrastructure** | Backend | Create `SubscriptionPlan` table, `TenantSubscription` with feature flags. Middleware to check plan limits on API requests. |
| 5.7 | **Advanced Pipeline Logic** | Backend | Implement parallel stages (candidate can be in multiple stages of same pipeline), conditional transitions (e.g., if location=Remote → skip On-site stage), automatic advancement (e.g., assessment passed → next stage). |
| 5.8 | **Bulk Operations Endpoints** | Backend | Implement `PATCH /api/v1/applications/bulk/move-stage`, `POST /api/v1/candidates/bulk-import` (enhanced from Phase 2). |
| 5.9 | **Redis Caching** | Backend | Implement `IDistributedCache` wrapper for pipeline templates, permissions, frequently accessed reference data. Cache invalidation on updates. |
| 5.10 | **Performance Tuning** | Backend | Database index optimization, query profiling with EF Core logs, N+1 query elimination. |

---

## Phase 6 — AI Readiness (Weeks 21–24)

### Person 1 — Frontend + Backend (UI-Adjacent)

| # | Task | Layer | Details |
|---|------|-------|---------|
| 6.1 | **Search Controller (Backend)** | Backend | Implement `SearchController`: `GET /api/v1/search?query=` — global search across Jobs (title), Candidates (name, email, skills), Interviews (candidate name), Assessments (title). |
| 6.2 | **File Upload Endpoint (Backend)** | Backend | Implement `POST /api/v1/candidates/{id}/resume` — multipart file upload. Validate content type (PDF, DOCX), max size (10MB). Store in Azure Blob/MinIO. Save metadata in `CandidateDocument`. |
| 6.3 | **AI Feature Placeholders (UI)** | Frontend | Add feature-flagged UI elements behind `FeatureFlagService`: "AI Match Score" badge on candidate cards, "Rank by AI" button on pipeline board, resume parsing status indicator on candidate profile. |
| 6.4 | **Search UI** | Frontend | Build global search component in header: search input with debounce, results dropdown grouped by entity type (Jobs, Candidates, Interviews, Assessments). |

### Person 2 — Backend

| # | Task | Layer | Details |
|---|------|-------|---------|
| 6.5 | **AI Data Models** | Backend | Create entities: `ParsedResume` (extracted text, structured skills, experience summary, education), `CandidateSkill` (skill name, proficiency, years), `JobSkill` (required skill, weight), `AIScoringResult` (candidateId, jobId, score, breakdown JSON). Add to DbContext + migration. |
| 6.6 | **Resume Parsing Pipeline** | Backend | Implement Hangfire background job: pick up new `CandidateDocument` → call external parsing API (or mock) → extract structured data → populate `ParsedResume` and `CandidateSkill`. |
| 6.7 | **Mock AI Service** | Backend | Create mock implementation of `IAIService` returning dummy data: resume parse results, candidate ranking scores, job matching scores. Allows frontend dev without real AI API. |
| 6.8 | **AI API Endpoints** | Backend | Implement: `POST /api/v1/ai/parse-resume` (triggers parsing job), `GET /api/v1/ai/candidate-ranking?jobId=` (returns candidates sorted by match score), `GET /api/v1/ai/match-job?candidateId=` (returns jobs ranked by fit). |

---

## Phase 7 — Testing & Optimization (Weeks 25–28)

### Person 1 — Frontend

| # | Task | Layer | Details |
|---|------|-------|---------|
| 7.1 | **Frontend Unit Tests** | Frontend | Write Jasmine/Karma tests for all components (rendering, user interaction, state changes), services (API calls, error handling), NgRx effects (success/failure paths), reducers (state transitions). Target >80% coverage. |
| 7.2 | **Accessibility Audit** | Frontend | Run axe DevTools / Lighthouse audit. Fix: color contrast ratios, missing ARIA labels, keyboard navigation gaps, focus management in modals. |
| 7.3 | **Responsive Design Polish** | Frontend | Test on mobile/tablet viewports. Fix: sidebar collapse behavior, table-to-card layout switch, pipeline board accordion on small screens, touch-friendly controls. |

### Person 2 — Backend

| # | Task | Layer | Details |
|---|------|-------|---------|
| 7.4 | **Backend Unit Tests** | Backend | Write xUnit tests for: Domain entities (validation, value objects), CQRS handlers (command/query logic), FluentValidation validators, service implementations. Target >80% coverage for Domain and Application layers. |
| 7.5 | **Integration Tests** | Backend | Write integration tests with test database (EF Core InMemory or SQL Server container). Cover: full API request → response flow for each controller, business rule enforcement (stage transition validation, offer approval workflow), auth/authorization, tenant isolation. |
| 7.6 | **Load Testing** | Backend | Write k6 scripts simulating real user flows. Test with 500/1000 concurrent users. Identify bottlenecks in API endpoints and database queries. Target: 95th percentile <200ms reads, <500ms writes. |
| 7.7 | **Security Testing** | Backend | Run OWASP ZAP scan. Fix: CSRF vulnerabilities, XSS in inputs, CORS hardening, rate limit tuning, JWT token security (short expiry, refresh rotation). |
| 7.8 | **Database Optimization** | Backend | Analyze slow queries from app logs. Add indexes for: TenantId + foreign keys, status fields used in filters, search fields (name, email). Run `EXPLAIN` on complex queries. |
| 7.9 | **Deployment Scripts** | DevOps | Create Azure ARM / Terraform templates. Configure: App Service, SQL Database, Blob Storage, Redis Cache, SignalR Service. Set up staging + production slots with blue-green deployment. |
| 7.10 | **Disaster Recovery** | DevOps | Document and test: database point-in-time restore, geo-replication failover, backup retention policy (7 years for audit logs). Create runbook. |

---

## Summary: Work Distribution

| Phase | Person 1 Tasks | Person 2 Tasks |
|-------|:--------------:|:--------------:|
| **Phase 1 — Foundation** | 17 | 8 |
| **Phase 2 — Recruitment Core** | 16 | 10 |
| **Phase 3 — Interview & Assessment** | 16 | 8 |
| **Phase 4 — Analytics & Reporting** | 9 | 2 |
| **Phase 5 — SaaS & Advanced** | 5 | 5 |
| **Phase 6 — AI Readiness** | 4 | 4 |
| **Phase 7 — Testing & Optimization** | 3 | 7 |
| **TOTAL** | **70 tasks** | **44 tasks** |

---

## Dependencies & Handoff Points

| Handoff | What | Responsible | When |
|---------|------|-------------|------|
| **H1** | API Base URL + Auth/Tenant/User/Roles endpoints | Person 1 | End of Phase 1 |
| **H2** | Job/Candidate/Application/Pipeline API endpoints | Person 2 | End of Phase 2 |
| **H3** | Interview/Assessment/Offer API endpoints | Person 2 | End of Phase 3 |
| **H4** | Dashboard/Analytics/Notification API endpoints | Person 1 | End of Phase 4 |
| **H5** | Advanced pipeline + bulk operation APIs | Person 2 | End of Phase 5 |
| **H6** | AI + Search + File Upload APIs | Person 1 + 2 | End of Phase 6 |

> **Note:** Person 1 can develop Frontend UI independently using Angular `HttpInterceptor` with a mock backend (`angular-in-memory-web-api` or similar) while waiting for real API endpoints from Person 2. This decouples frontend and backend development.
>
> **Critical Ordering:** Person 1 must build the **Offer Domain Entities** in Phase 1 because Phase 3's Offer module depends on them, and Person 2 needs them for the Offer Controller + CQRS.