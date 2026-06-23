# TalentFlow — Product Requirements Document (PRD)

> **Document Version:** 1.0  
> **Date:** June 2026  
> **Owner:** Product Management  
> **Status:** Approved for Development

---

## 1. Product Goals

### Primary Goals
1. Provide SMBs with a centralized, structured recruitment workflow
2. Replace disconnected tools (spreadsheets, email, manual scheduling) with a single platform
3. Reduce average time-to-hire by ≥ 35% within 3 months of adoption
4. Enable data-driven hiring decisions through analytics and AI-powered scoring
5. Scale to a multi-tenant SaaS model to monetize the platform

### Secondary Goals
1. Improve candidate experience through transparent communication
2. Enable fair, consistent evaluation through structured assessments
3. Support compliance requirements (audit logs, data retention, RBAC)
4. Provide a showcase of senior-level software engineering maturity for portfolio purposes

---

## 2. Success Metrics (KPIs)

| Metric | Baseline | Target (90 Days) | Measurement Method |
|---|---|---|---|
| Time-to-Hire | 42 days (industry avg) | ≤ 25 days | Analytics dashboard |
| Recruiter Admin Time | ~14 hrs/week | ≤ 6 hrs/week | User surveys |
| Candidate Drop-off Rate | 60% | ≤ 25% | Application funnel analytics |
| Interview No-Show Rate | ~30% | ≤ 10% | Interview scheduling module |
| AI Match Accuracy | — | ≥ 80% precision | A/B testing vs. manual ranking |
| System Uptime | — | ≥ 99.5% | Infrastructure monitoring |
| API Response Time (P95) | — | ≤ 300ms | APM tooling |
| User Satisfaction (NPS) | — | ≥ 40 | In-app feedback |

---

## 3. User Personas

### Persona 1: Sarah — HR Specialist
- **Age:** 32 | **Company Size:** 50–200 employees
- **Tech Proficiency:** Medium
- **Goals:** Post jobs quickly, track all candidates in one place, generate reports for leadership
- **Frustrations:** Losing candidate data in email threads, spending hours on status updates
- **Key Features Needed:** Job posting, pipeline board, automated email notifications, reporting

### Persona 2: James — Technical Recruiter
- **Age:** 28 | **Company Size:** 200–500 employees
- **Tech Proficiency:** High
- **Goals:** Source top technical talent, rank candidates efficiently, schedule technical assessments
- **Frustrations:** No visibility into technical assessment results, inefficient back-and-forth with hiring managers
- **Key Features Needed:** AI match scoring, technical assessment module, interview feedback

### Persona 3: Maria — Hiring Manager (Engineering Lead)
- **Age:** 38 | **Company Size:** 100–300 employees
- **Tech Proficiency:** High
- **Goals:** Make confident final hiring decisions backed by data, give structured feedback
- **Frustrations:** Reviewing unstructured CVs, no aggregated view of candidate evaluations
- **Key Features Needed:** Candidate scorecards, interview feedback summaries, AI-generated insights

### Persona 4: Rami — Startup Founder
- **Age:** 35 | **Company Size:** 5–30 employees
- **Tech Proficiency:** Medium-High
- **Goals:** Hire 3–5 key people with minimal admin overhead
- **Frustrations:** No time for complex HR tools, needs a simple but structured process
- **Key Features Needed:** Quick job posting, simple pipeline, candidate comparison

### Persona 5: Alex — Job Candidate
- **Age:** 26 | **Background:** Software Engineer
- **Goals:** Apply easily, get timely feedback, understand next steps
- **Frustrations:** No status updates, opaque hiring process
- **Key Features Needed:** Application status tracking, email notifications

---

## 4. Functional Requirements

### FR-01: Authentication & Authorization
| ID | Requirement | Priority |
|---|---|---|
| FR-01-1 | Users shall register with email, password, and role | Must Have |
| FR-01-2 | System shall use JWT access tokens (15-min expiry) with refresh tokens (7-day expiry) | Must Have |
| FR-01-3 | System shall enforce Role-Based Access Control (Admin, HR, Recruiter, Hiring Manager, Interviewer) | Must Have |
| FR-01-4 | Users shall reset passwords via email-verified token | Must Have |
| FR-01-5 | System shall log all authentication events (login, logout, failed attempts) | Should Have |
| FR-01-6 | System shall support Google OAuth (Phase 2) | Should Have |

### FR-02: Job Management
| ID | Requirement | Priority |
|---|---|---|
| FR-02-1 | Authorized users shall create, edit, publish, and archive job listings | Must Have |
| FR-02-2 | Jobs shall have: title, description, department, location, type, salary range, required skills, status | Must Have |
| FR-02-3 | Jobs shall support multiple statuses: Draft, Active, Closed, Archived | Must Have |
| FR-02-4 | AI shall generate job descriptions from title and skill inputs | Should Have |
| FR-02-5 | System shall track views and application counts per job | Should Have |
| FR-02-6 | Jobs shall support custom stages for the hiring pipeline | Must Have |

### FR-03: Candidate Management
| ID | Requirement | Priority |
|---|---|---|
| FR-03-1 | System shall maintain a searchable candidate database | Must Have |
| FR-03-2 | Candidate profiles shall include: name, contact info, skills, experience, education, resume | Must Have |
| FR-03-3 | System shall support resume upload (PDF, DOC, DOCX) | Must Have |
| FR-03-4 | AI shall parse resumes and extract skills, experience, education automatically | Should Have |
| FR-03-5 | Candidates shall be taggable with custom labels | Should Have |
| FR-03-6 | System shall calculate and display an AI match score against a job posting | Should Have |

### FR-04: Application Tracking
| ID | Requirement | Priority |
|---|---|---|
| FR-04-1 | Candidates may apply to one or more open jobs | Must Have |
| FR-04-2 | Applications shall track stage: Applied → Screened → Interview → Offer → Hired / Rejected | Must Have |
| FR-04-3 | System shall display a Kanban-style pipeline board per job | Must Have |
| FR-04-4 | Recruiters shall move candidates between stages via drag-and-drop | Must Have |
| FR-04-5 | System shall log every stage transition with timestamp and actor | Must Have |
| FR-04-6 | System shall send automated email/notification on stage change | Should Have |

### FR-05: Interview Management
| ID | Requirement | Priority |
|---|---|---|
| FR-05-1 | Interviewers shall be assigned to specific candidates and application stages | Must Have |
| FR-05-2 | System shall schedule interviews with date, time, duration, format (Video/Phone/On-site), and location | Must Have |
| FR-05-3 | System shall send interview invitations to interviewers and candidates | Must Have |
| FR-05-4 | Interviewers shall submit structured feedback and ratings (1–5 scale per criterion) | Must Have |
| FR-05-5 | System shall aggregate multi-interviewer ratings into a composite score | Should Have |
| FR-05-6 | AI shall summarize interviewer feedback into a concise recommendation | Should Have |
| FR-05-7 | System shall generate AI-suggested interview questions based on job description and candidate profile | Should Have |

### FR-06: Technical Assessments
| ID | Requirement | Priority |
|---|---|---|
| FR-06-1 | Admins/HR shall create assessment templates with questions (MCQ, Coding, Open-ended) | Must Have |
| FR-06-2 | Assessments shall be assignable to candidates at specific pipeline stages | Must Have |
| FR-06-3 | System shall enforce time limits on assessments | Must Have |
| FR-06-4 | Coding assessments shall support basic syntax-highlighted editors | Should Have |
| FR-06-5 | System shall auto-grade MCQ answers and display scores | Must Have |
| FR-06-6 | System shall notify recruiter when an assessment is completed | Should Have |

### FR-07: Notifications
| ID | Requirement | Priority |
|---|---|---|
| FR-07-1 | System shall send in-app real-time notifications via SignalR | Must Have |
| FR-07-2 | System shall send email notifications for key events (interview scheduled, application received, stage change) | Must Have |
| FR-07-3 | Users shall configure their notification preferences | Should Have |
| FR-07-4 | Notifications shall be queued via Hangfire for reliability | Should Have |

### FR-08: Analytics Dashboard
| ID | Requirement | Priority |
|---|---|---|
| FR-08-1 | Dashboard shall display: total jobs, total candidates, open applications, interviews scheduled | Must Have |
| FR-08-2 | Dashboard shall show pipeline conversion funnel | Must Have |
| FR-08-3 | Dashboard shall display time-to-hire per job and overall | Should Have |
| FR-08-4 | Dashboard shall show source-of-hire metrics | Should Have |
| FR-08-5 | Reports shall be exportable to PDF/CSV | Should Have |

### FR-09: Admin Panel
| ID | Requirement | Priority |
|---|---|---|
| FR-09-1 | Admins shall manage users: create, activate, deactivate, assign roles | Must Have |
| FR-09-2 | Admins shall view full audit logs (actor, action, resource, timestamp, IP) | Must Have |
| FR-09-3 | Admins shall configure system settings (email templates, pipeline stages, departments) | Should Have |
| FR-09-4 | Admins shall manage permission sets and role definitions | Should Have |

---

## 5. Non-Functional Requirements

### Performance
| ID | Requirement |
|---|---|
| NFR-P-01 | API response time ≤ 200ms for 95th percentile under normal load |
| NFR-P-02 | Application shall support ≥ 500 concurrent users without degradation |
| NFR-P-03 | File uploads (resume) shall complete within 5 seconds for files up to 10MB |
| NFR-P-04 | Dashboard queries shall render within 2 seconds |

### Security
| ID | Requirement |
|---|---|
| NFR-S-01 | All API endpoints shall be protected by JWT authentication (except public endpoints) |
| NFR-S-02 | Passwords shall be hashed using BCrypt (min work factor 12) |
| NFR-S-03 | All data in transit shall use TLS 1.2+ |
| NFR-S-04 | Input validation shall be enforced at API and domain layer |
| NFR-S-05 | SQL injection shall be prevented via parameterized queries (EF Core) |
| NFR-S-06 | File uploads shall be scanned for malicious content |

### Reliability
| ID | Requirement |
|---|---|
| NFR-R-01 | System uptime ≥ 99.5% (excluding planned maintenance) |
| NFR-R-02 | Database backups shall run daily with 30-day retention |
| NFR-R-03 | Background jobs (Hangfire) shall be retried up to 3 times on failure |

### Usability
| ID | Requirement |
|---|---|
| NFR-U-01 | UI shall be fully responsive (mobile, tablet, desktop) |
| NFR-U-02 | UI shall comply with WCAG 2.1 Level AA accessibility standards |
| NFR-U-03 | Core workflows (post job, move candidate) shall complete in ≤ 3 clicks |

### Maintainability
| ID | Requirement |
|---|---|
| NFR-M-01 | Code shall achieve ≥ 80% unit test coverage on domain and application layers |
| NFR-M-02 | All public APIs shall be documented via Swagger/OpenAPI 3.0 |
| NFR-M-03 | Structured logging (Serilog) shall be implemented for all key operations |

---

## 6. Assumptions

1. The platform is initially deployed as a single-tenant application; multi-tenancy is Phase 4
2. AI features will use pre-built NLP libraries or OpenAI API (not custom ML models)
3. Email delivery uses SMTP initially (SendGrid in production)
4. File storage uses local file system in development, Azure Blob Storage in production
5. The application requires a modern browser (Chrome 90+, Firefox 88+, Edge 90+)
6. Candidates interact via email links and a public-facing application form (no login required for applying)

---

## 7. Constraints

| Constraint | Detail |
|---|---|
| Technology Stack | ASP.NET Core, Angular, SQL Server — no deviations |
| Budget | Self-funded portfolio project; use free-tier or open-source tools |
| Timeline | MVP in 4 weeks; full delivery in ~17 weeks |
| Team Size | Single developer (with AI assistance) |
| AI Dependencies | OpenAI API rate limits and token costs must be managed |
| Legal | GDPR-compatible data handling for EU-based candidates |

---

## 8. Risks

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| AI API costs exceed budget | Medium | Medium | Implement usage caps, caching of AI results |
| Complex CQRS/Clean Architecture creates delivery delays | High | Medium | Start with simpler patterns in Phase 1, refactor progressively |
| Resume parsing accuracy is poor | Medium | High | Allow manual correction; fall back to manual input |
| SignalR scaling issues at high concurrency | Low | High | Use Azure SignalR Service in production |
| SQL query performance degrades with large datasets | Medium | High | Implement indexing strategy + query optimization early |
| Security vulnerability in file uploads | Medium | Critical | Validate MIME types, scan files, restrict upload paths |
