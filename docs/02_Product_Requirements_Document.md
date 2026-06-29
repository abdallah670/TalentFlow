# 2. Product Requirements Document (PRD)

## Product Vision
TalentFlow empowers recruiters and hiring teams with a highly customizable, collaborative, and data‑centric hiring platform. Every action is transparent, measurable, and compliant. The platform grows with the customer—from a simple job board tracker to a full enterprise ATS with AI augmentation.

## Product Goals
1. **MVP (Phase 1–3)** – Core job, candidate, and workflow management with configurable linear pipelines and role‑based access. Interview scheduling and feedback, offer management, basic analytics.
2. **V2.0 (Phase 4–5)** – Multi‑tenant administration, subscription plans, tenant branding, advanced pipeline capabilities.
3. **V2.5** – AI integrations: resume parsing, candidate‑job matching, skill extraction.
4. **V3.0** – Enterprise features: SSO, advanced permission sets, custom API integrations, marketplace.

## User Personas
- **Taylor, Recruiter** – Manages 20 open reqs. Needs to move candidates quickly through stages, schedule interviews, and see pipeline health. Values speed and clarity. Uses pipeline board daily.
- **Jordan, Hiring Manager** – Reviews shortlisted candidates, gives feedback, approves offers. Wants mobile‑friendly feedback forms and candidate comparison. Receives notifications when action needed.
- **Morgan, HR Ops** – Configures job templates, hiring workflows, user roles, and runs monthly recruitment KPI reports. Demands audit logs and compliance. Needs export capabilities.
- **Casey, System Admin** – Onboards new tenant companies, sets up subscription plans, monitors system health. Prioritizes security, tenant isolation, and performance monitoring.
- **Devon, Developer/Integrator** – Uses REST API to integrate TalentFlow with existing HRIS or career sites. Needs clear API documentation and webhooks.

## User Journeys
1. **Job Requisition to Offer** – HR Ops creates a job with a pipeline template. Recruiter posts and sources candidates. Candidates progress through screening → interview → assessment → offer. Each transition triggers notifications and audit entries.
2. **Interview & Assessment** – Interviewer receives schedule notification, conducts interview, submits scorecard. Technical assessment is sent automatically; results are attached to candidate profile. Hiring manager reviews aggregated feedback.
3. **Reporting** – HR Ops opens the time‑to‑hire dashboard, filters by department, exports a CSV report for a leadership meeting. Notices a bottleneck in "Technical Test" stage and adjusts resources.

## Functional Requirements (High‑Level)
See SRS for detailed requirements. Key areas:
- Multi‑tenant tenant management
- Job requisition and publishing with custom fields
- Configurable recruitment workflow engine
- Candidate application management (manual, CSV import, future API)
- Stage transitions with mandatory fields and role‑based permissions
- Interview scheduling with feedback collection
- Technical assessment management (creation, assignment, auto‑score)
- Offer generation and approval workflow
- Real‑time notifications (SignalR) and email notifications (Hangfire)
- Audit trail for all state changes
- RBAC with granular permissions
- Analytics dashboards with standard KPIs

## Non‑Functional Requirements
- **Performance** – API response time <200ms for 95th percentile under 1000 concurrent users.
- **Scalability** – Support 100,000+ candidates, 10,000+ jobs, with horizontal scaling of stateless services.
- **Availability** – 99.9% uptime target for web services.
- **Security** – OWASP Top 10 mitigation, encrypted data at rest and in transit, JWT with refresh tokens, rate limiting.
- **Auditability** – Immutable, tamper‑evident audit log; all CUD operations on core entities logged.
- **Multi‑tenancy** – Strict tenant isolation; no cross‑tenant data access.
- **Maintainability** – Clean Architecture, SOLID principles, unit test coverage >80% on domain/business logic.
- **Extensibility** – Plugin‑style pipeline stages, AI integration points clearly defined.

## Success Metrics
- Time‑to‑hire reduced by 15% for pilot customers.
- User adoption rate >80% within 3 months of rollout.
- Zero cross‑tenant data leakage incidents.
- Audit log completeness: 100% of defined events captured.
- System uptime meets SLA of 99.9%.

## Assumptions
- Tenants will accept a shared database with logical isolation; physical isolation can be offered later for premium tiers.
- Each tenant will have at most 500 concurrent users initially.
- File storage will be cloud blob storage (Azure Blob).
- Email delivery handled by a third‑party service (SendGrid/Mailgun).
- MVP will not include a public candidate portal; applications are created by recruiters or ingested via API.

## Constraints
- Solo developer must be able to build the core within 6‑8 months (MVP).
- Must use the specified technology stack: ASP.NET Core, Angular, SQL Server.
- Cloud costs must be manageable; no expensive managed services unless essential.
- GDPR compliance is mandatory for EU tenants (data residency, right to erasure).

## Risks
- **Workflow engine complexity** – Could become over‑engineered; MVP should support linear pipelines with parallel stages later.
- **Performance under multi‑tenancy** – Tenant ID index must be on every query; query plan monitoring required.
- **Security misconfiguration** – Multi‑tenant data filtering must be enforced at the data access layer, not left to application logic.
- **Third‑party integration delays** – Use abstract interfaces; stub external services during development.

## MVP Scope
- Tenant registration (single tenant initially, but DB schema multi‑tenant‑ready).
- Job management with custom fields.
- Candidate management (manual creation, resume upload).
- Configurable pipeline engine with linear stages.
- Stage transitions with comments and audit.
- Basic roles: Admin, Recruiter, Hiring Manager, Interviewer.
- JWT auth with RBAC.
- Simple notification emails.
- API‑only (Angular admin panel for recruiters/HR, no candidate portal).

## Future Scope
- Candidate self‑service portal.
- Calendar integration (Outlook/Google) for interview scheduling.
- Advanced analytics with trend prediction.
- AI‑based resume parsing and matching.
- SSO (OIDC/SAML).
- Marketplace for assessment integrations.