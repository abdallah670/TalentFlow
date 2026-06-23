# TalentFlow — Business Requirements Document (BRD)

> **Document Version:** 1.0  
> **Date:** June 2026  
> **Owner:** Business Analysis  
> **Status:** Approved

---

## 1. Business Objectives

| ID | Objective | Measurement |
|---|---|---|
| BO-01 | Reduce average time-to-hire for SMBs by 35%+ | Analytics: avg. days from job post to offer acceptance |
| BO-02 | Eliminate manual, error-prone spreadsheet-based hiring | Adoption metric: 0 spreadsheet dependencies post-onboarding |
| BO-03 | Improve candidate evaluation quality through structured, AI-assisted scoring | Average interviewer rating variance reduced by 40% |
| BO-04 | Provide actionable hiring analytics to decision-makers | Weekly dashboard report adoption by hiring managers |
| BO-05 | Automate repetitive recruiter tasks (scheduling, notifications, follow-ups) | Recruiter admin time reduced to < 6 hrs/week |
| BO-06 | Build a scalable SaaS product viable for monetization in Phase 4 | Multi-tenant architecture operational; subscription plans defined |

---

## 2. Stakeholders

| Stakeholder | Role | Interest Level | Influence Level |
|---|---|---|---|
| **Startup Founders / SMB Owners** | Primary Buyer | High | High |
| **HR Specialists** | Primary User | High | Medium |
| **Recruiters** | Primary User | High | Medium |
| **Hiring Managers** | Key User | High | High |
| **Candidates** | End Consumer | Medium | Low |
| **Technical Interviewers** | Secondary User | Medium | Low |
| **System Administrator** | Platform Operator | Medium | Medium |
| **Development Team** | Builder | High | High |
| **Investors / Future SaaS Buyers** | Future Stakeholder | Low (now) | High (Phase 4) |

### Stakeholder Communication Plan

| Stakeholder | Communication Channel | Frequency |
|---|---|---|
| Development Team | Sprint planning, daily standups, retrospectives | Daily |
| Primary Users (HR/Recruiter) | User testing sessions, feedback surveys | Bi-weekly |
| Business Owner | Status reports, demo sessions | Weekly |
| Candidates | Email notifications, application portal | As-needed |

---

## 3. Scope

### In-Scope (MVP through Phase 4)

#### Phase 1 — MVP
- User authentication with RBAC (Admin, HR, Recruiter, Hiring Manager, Interviewer)
- Job posting management (create, edit, publish, archive)
- Candidate profiles and resume upload
- Application tracking with multi-stage pipeline Kanban board
- Interview scheduling and basic feedback collection
- KPI dashboard with key hiring metrics

#### Phase 2 — Professional
- Technical assessment module (MCQ, open-ended, basic coding)
- Real-time notifications (SignalR) and email notifications (Hangfire)
- Audit logging and admin panel
- Advanced reporting and export (PDF/CSV)
- File storage integration (Azure Blob Storage)

#### Phase 3 — AI Features
- AI-powered resume parsing and skill extraction
- Candidate-to-job match scoring
- AI candidate ranking per job
- AI interview question generation
- AI interview feedback summarization
- AI-assisted job description generation

#### Phase 4 — SaaS
- Multi-tenant architecture with complete data isolation
- Company workspace management
- Subscription plan management (Free, Professional, Enterprise)
- Tenant-specific settings and configuration
- Usage analytics and billing hooks

---

## 4. Out of Scope

The following are **explicitly excluded** from all phases (unless formally re-scoped):

| Item | Rationale |
|---|---|
| Native mobile applications (iOS/Android) | Web-responsive design covers mobile use cases; native apps are a V3+ consideration |
| Video interview hosting | Integration with Zoom/Teams is preferred; building proprietary video infrastructure is out of scope |
| Payroll or HRIS integration | Beyond hiring — requires a separate module and compliance layer |
| Background check integrations (e.g., Sterling, Checkr) | Third-party integrations are post-SaaS phase |
| Custom ML model training | Use OpenAI API / pre-built NLP; custom training requires MLOps infrastructure |
| Job board external posting (Indeed, LinkedIn) | API costs and complexity deferred to V2 |
| Candidate self-service portal with full account | Candidates apply via link; full portal is a V2 feature |
| Multi-language / localization | English-first; i18n framework will be scaffolded for future use |

---

## 5. Business Rules

### BR-01: Role Hierarchy & Permissions
```
Admin > HR Specialist > Recruiter > Hiring Manager > Interviewer
```
- Admins have unrestricted access to all system data
- HR Specialists can create jobs and manage the full pipeline
- Recruiters can manage applications but cannot create jobs
- Hiring Managers can view all candidates for their jobs and submit final decisions
- Interviewers can only see their assigned interviews and submit feedback

### BR-02: Application Lifecycle
- A candidate may apply to multiple jobs simultaneously
- A candidate cannot apply to the same job twice while an application is active
- Applications must progress forward through pipeline stages (no backward movement without explicit "revert" permission)
- An application automatically closes when a hire is made for the same job (other applications move to "Rejected")

### BR-03: Job Posting Rules
- Only HR Specialists and Admins may publish a job posting
- A job cannot be published without: title, description, department, required skills, and at least one pipeline stage
- A published job becomes "Closed" automatically after its expiry date (configurable)
- Archived jobs are read-only and cannot receive new applications

### BR-04: Assessment Rules
- Assessments are timed; once started, the timer cannot be paused
- Assessment results are visible to recruiters and hiring managers only (not to candidates)
- Auto-graded MCQ results are final; manual review is required for coding and open-ended questions

### BR-05: Interview Rules
- An interview must have at least one assigned interviewer
- Interview feedback must be submitted within 48 hours of the scheduled interview time
- A candidate cannot be moved to the "Offer" stage without at least one completed interview feedback record

### BR-06: Data Retention
- Candidate data is retained for a maximum of 2 years post-application (GDPR compliance)
- Audit logs are retained for 1 year
- Deleted records are soft-deleted (IsDeleted flag) and purged after the retention period

### BR-07: Audit Rules
- All create, update, delete, and stage-change operations are recorded in the audit log
- Audit records are immutable (no edit, no delete by any user including Admin)
- Audit logs capture: entity type, entity ID, actor user ID, action, timestamp, IP address, before/after state (JSON diff)

---

## 6. Business Constraints

| Constraint | Detail |
|---|---|
| **Regulatory** | Candidate data handling must comply with GDPR (right to access, right to erasure) |
| **Data Residency** | All data must be stored within the EU for EU-based tenants (Phase 4) |
| **Accessibility** | UI must meet WCAG 2.1 Level AA for inclusivity compliance |
| **Integration** | Phase 1-3 uses no external HRIS/ERP integration; API-first design enables future integrations |
| **Vendor Lock-in** | Architecture must allow swapping AI providers (OpenAI → Azure OpenAI → local LLM) without core refactoring |

---

## 7. Expected Business Outcomes

### Short-Term (0–3 months)
- Functional MVP with core recruitment workflow operational
- At least 3 internal demo sessions with simulated user scenarios
- Portfolio-ready codebase demonstrating enterprise-grade architecture

### Medium-Term (3–6 months)
- Full feature set through Phase 3 (AI features operational)
- Automated test coverage ≥ 80% on domain/application layers
- CI/CD pipeline configured (GitHub Actions)

### Long-Term (6–12 months)
- SaaS multi-tenant architecture operational (Phase 4)
- First external beta users onboarded
- Subscription billing integration (Stripe) functional
- Platform capable of serving 10+ simultaneous tenants

---

## 8. Return on Investment (ROI) Projection

### For Portfolio
| Benefit | Estimated Value |
|---|---|
| Demonstrates senior-level architecture | Higher employability / $20K+ salary premium |
| Demonstrates AI integration capability | Preferred profile for AI-adjacent roles |
| Demonstrates SaaS design | Opens consulting/freelance opportunities |

### For Potential SaaS Product
| Plan | Price/Month | Target Segment | Projected Users (Year 1) |
|---|---|---|---|
| Free | $0 | Individual Recruiters | 500 |
| Professional | $49 | SMBs (≤ 50 employees) | 150 |
| Business | $149 | SMBs (51–500 employees) | 50 |
| **Annual MRR Target** | | | **~$15,000/month** |
