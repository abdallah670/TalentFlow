# 12. Development Roadmap (TalentFlow)

## Overview
The development of TalentFlow is structured into **seven phases**, spanning approximately **28 weeks**. Each phase builds on the previous one, with clear tasks, dependencies, deliverables, and definitions of done. The roadmap is designed for a solo developer (or small team) while ensuring a production‑grade SaaS platform.

---

## Phase 1 — Foundation (Weeks 1–4)

### Objective
Set up the core project infrastructure, authentication, multi‑tenancy, and audit framework.

### Tasks
- Set up Clean Architecture solution structure with Docker Compose.
- Implement tenant & user management (registration, login, JWT auth, refresh tokens, RBAC).
- Configure global query filter for multi‑tenancy (`ICurrentTenantService`).
- Implement audit interceptor and `AuditLog` entity.
- Run database migrations (Tenant, User, Role, Permission, AuditLog tables).
- Build base repositories and unit of work.
- Create Angular shell: auth module (login page), route guards, interceptors.
- Set up CI/CD pipeline (GitHub Actions) for builds and tests.

### Dependencies
- None.

### Deliverables
- API can register tenants, authenticate users, and manage roles.
- All database tables are multi‑tenant ready.
- Audit logging framework captures all CUD operations.
- Frontend can log in and display a basic dashboard shell.
- Docker Compose environment runs the full stack locally.

### Definition of Done
- All unit and integration tests for auth and tenant management pass.
- Swagger documentation available for auth and tenant endpoints.
- Docker environment seeds a default tenant and admin user.
- Code coverage >80% for auth and tenant modules.

---

## Phase 2 — Recruitment Core (Weeks 5–8)

### Objective
Build job and candidate management, pipeline template builder, and stage transition engine.

### Tasks
- Job CRUD (create, update, soft delete) with pipeline template assignment.
- Candidate CRUD (manual creation, CSV import), file upload to Azure Blob (or MinIO local).
- `Application` entity and basic listing.
- `WorkflowTemplate` builder: create/update stages (JSON configuration).
- Stage transition domain logic (`MoveToStage`) with permission checking and validation.
- Angular UI:
  - Job list (paginated, filterable) and job form (with custom fields).
  - Candidate list (search, filters) and profile (resume upload, application history).
  - Basic pipeline board (read‑only columns, manual move via dropdown).

### Dependencies
- Phase 1 complete (auth, tenant context, repositories).

### Deliverables
- End‑to‑end creation of jobs, candidates, applications.
- Manual stage movement with validation (pipeline rules, permissions).
- Stage history displayed on candidate profile.

### Definition of Done
- Recruiter can move a candidate through stages as per template rules.
- Notifications (in‑app) triggered on stage change (basic SignalR hub in place).
- Integration tests for stage transition logic pass.

---

## Phase 3 — Interview & Assessment (Weeks 9–12)

### Objective
Add interview scheduling, feedback, technical assessments, and offer management.

### Tasks
- Interview scheduling (API + UI: date/time, type, interviewers).
- Interview feedback submission (scorecard form).
- Assessment builder (question bank, auto‑scoring for multiple‑choice).
- Candidate submission of assessments (manual entry, future link).
- Offer management: generation, approval workflow, status tracking.
- Email notifications via Hangfire + SendGrid (or SMTP).
- Real‑time notification UI (SignalR bell icon, unread count).
- Angular:
  - Schedule interview dialog, feedback form.
  - Assessment builder (admin) and submission view.
  - Offer creation, approval, and tracker components.

### Dependencies
- Phase 2 complete (jobs, candidates, applications, stage engine).

### Deliverables
- Full interview lifecycle (schedule → feedback → completion).
- Assessment creation, assignment, and auto‑scoring.
- Offer generation with configurable approval chain.
- Email and in‑app notifications for all events.

### Definition of Done
- Interview feedback is linked to application; hiring manager can review.
- Offer can be created, approved by multiple approvers, sent, and tracked.
- Hangfire jobs correctly send emails without blocking API.
- SignalR delivers real‑time notifications to correct users/tenants.

---

## Phase 4 — Analytics & Reporting (Weeks 13–15)

### Objective
Implement KPIs, dashboards, and audit log viewer.

### Tasks
- Create denormalized read models (or SQL views) for KPI calculations.
- Build analytics API endpoints:
  - Time‑to‑Hire, Pipeline Funnel, Source Effectiveness, Recruiter Performance.
- Angular dashboards with charts (line, bar, pie) using Chart.js/ngx‑charts.
- Export functionality (CSV for data, PNG for charts).
- Audit log viewer (server‑side pagination, filtering by entity/user/date, expandable JSON diffs).

### Dependencies
- Phases 2–3 complete (transactional data exists).

### Deliverables
- Working analytics dashboard with real data.
- Tenant admin can view, filter, and export the immutable audit log.

### Definition of Done
- KPI numbers match manual calculations from raw data.
- Dashboards support date range, department, and job filters.
- Exported CSV contains correct data.
- Audit log viewer enforces tenant isolation and permission.

---

## Phase 5 — SaaS & Advanced Features (Weeks 16–20)

### Objective
Enhance multi‑tenancy, branding, advanced pipeline features, and bulk operations.

### Tasks
- Tenant branding (logo, primary color stored in `TenantSettings`, applied via CSS variables).
- Subscription plan infrastructure (`SubscriptionPlan` table, feature flags per plan).
- Platform admin dashboard: view all tenants, health, usage metrics.
- Advanced pipeline features:
  - Parallel stages (candidate in multiple stages simultaneously).
  - Conditional stage transitions (if location=Remote → skip on‑site).
  - Automatic stage advancement (e.g., after assessment passed).
- Bulk operations: bulk stage move, CSV import with validation.
- Performance tuning and caching (Redis).

### Dependencies
- Phase 4 complete (analytics in place, audit log ready).

### Deliverables
- Tenants can customize branding; pipeline editor is flexible.
- Platform admin can monitor tenants and enforce subscription limits.
- Advanced workflows supported.

### Definition of Done
- All features documented and tested.
- Load tested with 1000 concurrent users (target performance met).
- Security hardening complete (penetration test passed).

---

## Phase 6 — AI Readiness (Weeks 21–24)

### Objective
Prepare architecture and data models for AI/ML integration without implementing models.

### Tasks
- Standardize data models for AI:
  - `ParsedResume` table (extracted text, structured skills, experience, education).
  - `CandidateSkill` and `JobSkill` tables.
  - `AIScoringResult` entity for storing matching scores.
- Implement background processing infrastructure:
  - Hangfire job that picks up a resume upload, calls an external parsing service (or mock), and populates `ParsedResume`.
- Build feature‑flagged API endpoints:
  - `POST /api/v1/ai/resume-parse` (triggers parsing).
  - `GET /api/v1/ai/candidate-ranking?jobId=` (returns sorted candidates).
- Create mock AI service that returns dummy data to validate integration points.
- Angular UI placeholders for AI‑powered features (behind feature flag).

### Dependencies
- Phase 5 complete (stable SaaS, caching, permissions).

### Deliverables
- Architecture validated for plugging in real AI services.
- Resume parsing pipeline works (mock or real).
- Candidate ranking endpoint returns data (mock).

### Definition of Done
- AI services can be swapped without changes to core domain logic.
- Demo of end‑to‑end resume upload → parsing → structured skill extraction.
- Feature flags correctly toggle AI UI elements.

---

## Phase 7 — Testing & Optimization (Weeks 25–28)

### Objective
Hardening, performance optimization, security validation, and production readiness.

### Tasks
- Load testing (k6/JMeter) with 1000 concurrent users; identify bottlenecks.
- Database index tuning and query optimization (analyze slow queries).
- Implement Redis caching for frequently accessed data (pipeline templates, permissions).
- Security penetration testing (manual and automated).
- Accessibility audit and improvements.
- Bug fixing and final documentation updates.
- Prepare deployment scripts (Azure, Kubernetes, Terraform).
- Conduct disaster recovery drills (database restore, geo‑failover).

### Dependencies
- All previous phases complete.

### Deliverables
- Production‑ready build, fully optimized and secured.
- Deployment runbooks and monitoring dashboards configured.

### Definition of Done
- Performance meets targets (95th percentile API <200ms, DB queries <50ms).
- No critical or high vulnerabilities in security scan.
- Load test reports and penetration test results documented.
- CI/CD pipeline deploys to staging with zero‑downtime blue‑green strategy.