# 3. Business Requirements Document (BRD)

## Business Objectives
1. Centralize recruitment operations for SMB and mid‑market companies (50–5000 employees).
2. Provide auditable and compliant process tracking to support ISO/SOC certifications.
3. Deliver actionable insights through real‑time analytics and reduce manual reporting by 80%.
4. Enable seamless onboarding of new company tenants with minimal setup (< 5 minutes).
5. Create a platform foundation that can be monetized via tiered subscriptions and add‑on AI features.

## Stakeholders
| Stakeholder | Role | Key Concerns |
|-------------|------|--------------|
| Product Owner | Prioritizes features | Market demand, competitive analysis, ROI |
| Recruiters | End‑user | Ease of use, speed, pipeline visibility |
| Hiring Managers | End‑user | Candidate quality, feedback efficiency, approval workflows |
| HR Directors | Decision maker | Compliance, reporting, integration with existing HRIS |
| IT/Security Teams | Gatekeeper | Data security, tenant isolation, authentication standards |
| Sales/Marketing | Growth driver | Demo‑ready environments, branding, feature/benefit messaging |
| Developers/Architects | Builder | Technical feasibility, maintainability, scalability |

## Scope
**In‑Scope:**
- Multi‑tenant recruitment management system with job, candidate, pipeline, interview, assessment, and offer modules.
- Configurable hiring stages and transitions (linear with future branching).
- Full audit trail and role‑based access control.
- Standard recruitment analytics KPIs (Time‑to‑Hire, Funnel, Source Effectiveness).
- Email and in‑app notifications.
- Secure file storage for resumes and documents.
- REST API and Angular back‑office frontend.
- Tenant administration (platform level) for subscription plans and feature flags.

**Out of Scope (MVP):**
- Candidate public portal.
- External job board integrations (Indeed, LinkedIn) – API hooks prepared but not built.
- AI/ML models – Architecture prepared, no runtime models.
- Billing and payment processing – Subscription plans in later phase.
- Calendar integration for interview scheduling.
- Native mobile app.

## Business Processes
### 1. Tenant Onboarding
1. Platform admin creates tenant in admin dashboard (or via API).
2. System provisions default roles (TenantAdmin, Recruiter, HiringManager, Interviewer), default pipeline template ("Standard Hiring").
3. Invitation email sent to tenant admin with login credentials.
4. Tenant admin logs in, configures company branding, custom pipeline, and adds users.

### 2. Job Requisition
1. Hiring manager or recruiter creates job, selects pipeline template.
2. System validates and stores job; status "Draft" or "Open".
3. (Future) Job is published to career site or external boards.

### 3. Candidate Processing
1. Candidate added to a job application (manual or CSV import).
2. Application enters first stage of pipeline ("Applied").
3. Recruiter moves candidate to next stage ("Screening") after review.
4. Each transition records timestamp, actor, comment; audit entry created.
5. Notifications sent to next responsible person (e.g., interviewer when moved to "Interview").

### 4. Interview/Assessment
1. Interview scheduled (manual in MVP), interviewers assigned.
2. Interviewers receive notification; after interview, they submit structured feedback.
3. For assessments, candidate receives test link (future); score recorded.
4. Hiring manager reviews aggregated feedback.

### 5. Offer Management
1. After final stage, recruiter/hiring manager generates offer with details.
2. Offer goes through approval chain (if configured); approvers get notification.
3. Approved offer sent to candidate (email); status tracked (Accepted/Declined/Expired).

### 6. Reporting
1. HR Ops selects dashboard, applies filters (department, date range).
2. System displays charts and metrics calculated from transactional data.
3. User exports report as CSV/PDF for offline analysis.

## Business Rules
| Rule ID | Description |
|---------|-------------|
| BR-01 | A candidate can be in only one active stage per job application at a time. |
| BR-02 | Stage transitions are restricted by role; e.g., only Hiring Manager can move to Offer stage. |
| BR-03 | An offer cannot be generated unless candidate has passed all required stages (configurable per pipeline). |
| BR-04 | Audit entries are immutable; no update or delete allowed. |
| BR-05 | Tenant data must never be accessible by another tenant; enforced at infrastructure level. |
| BR-06 | Soft‑delete is used for all major entities; deleted records are filtered by default. |
| BR-07 | Notification preferences per user override default templates. |

## KPIs
| KPI | Formula / Definition | Target |
|-----|----------------------|--------|
| Time to Hire | Average calendar days from application creation to offer accepted | ≤ 30 days |
| Time in Stage | Median days spent in each pipeline stage | Identify bottlenecks |
| Source Effectiveness | Percentage of hires per source channel | Optimize sourcing budget |
| Offer Acceptance Rate | Offers accepted / offers extended | ≥ 85% |
| Recruiter Workload | Open positions per recruiter, candidates processed per week | Balanced workload |
| Pipeline Conversion Rate | Candidates moved from stage A to stage B / total entered stage A | Monitor drop‑off |

## Expected Outcomes
- 25% reduction in time‑to‑hire for early adopter companies.
- 100% audit compliance for all recruitment actions.
- Tenant churn less than 5% annually due to product stickiness.
- Platform capable of handling 500+ concurrent users across multiple tenants with acceptable performance.