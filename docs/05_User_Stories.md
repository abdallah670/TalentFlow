# 5. User Stories

## Epics and Feature Breakdown

### Epic 1: Tenant & User Administration
**Features:**
- Tenant Registration (Platform Admin)
- Tenant Settings Configuration (Tenant Admin)
- User Management (CRUD, role assignment)
- Role & Permission Configuration
- Global Tenant List (Platform Admin)

**User Stories:**
- As a Platform Admin, I want to create a new tenant so that a new company can use TalentFlow.
- As a Tenant Admin, I want to update my company’s branding (logo, color) so that the platform feels like an internal tool.
- As a Tenant Admin, I want to add a new user and assign them the "Recruiter" role so they can start managing jobs.
- As a Tenant Admin, I want to deactivate a user who left the company so that they no longer have access.

### Epic 2: Job & Pipeline Management
**Features:**
- Job CRUD (Create, Read, Update, Soft Delete)
- Pipeline Template Builder
- Job Status Management (Open, Closed, Draft)
- Custom Fields on Jobs

**User Stories:**
- As a Recruiter, I want to create a new job requisition and assign a pipeline template so that I can start sourcing candidates.
- As a Tenant Admin, I want to edit the "Standard Hiring" pipeline to add a "Technical Test" stage so that it matches our process.
- As a Recruiter, I want to view all open jobs with their candidate counts so I can prioritize my efforts.
- As a Hiring Manager, I want to see jobs where I am listed as the hiring manager so I can review candidate pipelines.

### Epic 3: Candidate & Application Tracking
**Features:**
- Candidate Profile (CRUD, resume upload, skills)
- Application Creation (manual add to job)
- Stage Movement (transition with validation)
- Pipeline Board (Kanban view)
- Candidate History (audit trail of stage changes)

**User Stories:**
- As a Recruiter, I want to add a new candidate to a job so that I can start tracking them.
- As a Recruiter, I want to move a candidate from "Screening" to "HR Interview" so that the hiring process continues. (AC: transition validated, comment required, notification sent)
- As a Recruiter, I want to view all candidates for a job in a Kanban board grouped by stage so I can see the pipeline at a glance.
- As a Hiring Manager, I want to see a candidate’s full stage history so I know how they progressed.
- As a Recruiter, I want to bulk‑move candidates from one stage to another (e.g., after a mass screening) to save time.

### Epic 4: Interview & Assessment
**Features:**
- Interview Scheduling (manual date/time, interviewers)
- Feedback Submission (scorecard)
- Assessment Creation (question bank, scoring)
- Assessment Assignment & Submission

**User Stories:**
- As a Recruiter, I want to schedule an interview for a candidate and assign interviewers so that the team can evaluate them.
- As an Interviewer, I want to receive a notification when I am assigned an interview so I don’t miss it.
- As an Interviewer, I want to submit my feedback and score via a structured form so the hiring manager can make a decision.
- As an HR Admin, I want to create a technical assessment with multiple‑choice questions so that we can test candidates objectively.
- As a Recruiter, I want to assign an assessment to a candidate and see their score automatically calculated.

### Epic 5: Offer Management
**Features:**
- Offer Generation (salary, start date, etc.)
- Approval Workflow (configurable)
- Offer Tracking (status: Draft, Sent, Accepted, etc.)
- Offer Letter Generation (future template)

**User Stories:**
- As a Recruiter, I want to generate an offer for a candidate who passed all stages so we can hire them.
- As an Approver, I want to receive a notification to approve/reject an offer so that we have proper authorization.
- As a Recruiter, I want to send the approved offer to the candidate via email so they can respond.
- As a Recruiter, I want to mark the offer as "Accepted" when the candidate confirms, which changes the application to "Hired".

### Epic 6: Notifications & Communication
**Features:**
- In‑App Notification Bell (real‑time via SignalR)
- Email Notifications (via Hangfire/SendGrid)
- Notification Preferences per User

**User Stories:**
- As a user, I want to see a list of unread notifications when I click the bell icon so I can catch up on actions.
- As a Recruiter, I want to receive an email when a candidate moves to my stage so I can act quickly.
- As a user, I want to disable email notifications for "Candidate Moved" events while keeping in‑app alerts.

### Epic 7: Audit & Compliance
**Features:**
- Immutable Audit Log
- Audit Log Viewer (tenant admin)
- Filtering by Entity, User, Date
- Export Audit Log

**User Stories:**
- As a Tenant Admin, I want to view the audit log filtered by entity type (e.g., Job) to see who made changes.
- As a Tenant Admin, I want to export the audit log to CSV for compliance reporting.
- As a Developer, the system must automatically capture old/new values for every update to core entities.

### Epic 8: Analytics & Reporting
**Features:**
- Time‑to‑Hire Dashboard
- Pipeline Funnel
- Source Effectiveness
- Recruiter Performance
- Export Reports (CSV/PDF)

**User Stories:**
- As an HR Ops, I want to see the average time‑to‑hire trend over the last 6 months so I can report to leadership.
- As a Recruiter, I want to see my pipeline funnel to identify stages where candidates drop off.
- As an HR Ops, I want to see which sourcing channels produce the most hires so we can invest accordingly.