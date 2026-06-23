# TalentFlow — Software Requirements Specification (SRS)

> **Document Version:** 1.0  
> **Date:** June 2026  
> **Standard:** IEEE 830 (adapted)  
> **Status:** Approved for Development

---

## 1. Introduction

### 1.1 Purpose
This Software Requirements Specification defines the complete technical requirements for the TalentFlow AI-Powered Hiring & Candidate Assessment Platform. It serves as the authoritative reference for the development team, QA team, and architects.

### 1.2 Scope
TalentFlow is a web-based application consisting of:
- **Backend:** ASP.NET Core 8 Web API (RESTful + SignalR)
- **Frontend:** Angular 17+ SPA
- **Database:** SQL Server 2022+
- **Background Processing:** Hangfire
- **File Storage:** Local FS (dev) / Azure Blob Storage (prod)
- **AI Services:** OpenAI API integration

### 1.3 Definitions & Abbreviations
| Term | Definition |
|---|---|
| ATS | Applicant Tracking System |
| CQRS | Command Query Responsibility Segregation |
| RBAC | Role-Based Access Control |
| JWT | JSON Web Token |
| NLP | Natural Language Processing |
| SRS | Software Requirements Specification |
| EF Core | Entity Framework Core |
| SMB | Small to Medium Business |
| SLA | Service Level Agreement |
| DTO | Data Transfer Object |

---

## 2. System Overview

### 2.1 System Architecture Overview
```
┌─────────────────────────────────────────────────────────────┐
│                     CLIENT LAYER                            │
│   Angular SPA (Browser)  ←→  SignalR (WebSocket)           │
└─────────────────────────────────────────────────────────────┘
                              ↕ HTTPS / WSS
┌─────────────────────────────────────────────────────────────┐
│                    API GATEWAY LAYER                        │
│   ASP.NET Core 8 Web API                                    │
│   ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────────┐  │
│   │Auth API  │ │Jobs API  │ │ Apps API │ │Assessments   │  │
│   └──────────┘ └──────────┘ └──────────┘ └──────────────┘  │
│   ┌──────────┐ ┌──────────┐ ┌──────────┐                   │
│   │Interview │ │Analytics │ │Admin API │                   │
│   └──────────┘ └──────────┘ └──────────┘                   │
└─────────────────────────────────────────────────────────────┘
                              ↕
┌─────────────────────────────────────────────────────────────┐
│                  APPLICATION LAYER (CQRS)                   │
│   MediatR Commands & Queries | FluentValidation             │
│   AutoMapper | Domain Events | Application Services         │
└─────────────────────────────────────────────────────────────┘
                              ↕
┌─────────────────────────────────────────────────────────────┐
│                    DOMAIN LAYER                             │
│   Entities | Value Objects | Domain Events | Aggregates     │
│   Business Rules | Repository Interfaces | Specifications   │
└─────────────────────────────────────────────────────────────┘
                              ↕
┌─────────────────────────────────────────────────────────────┐
│                INFRASTRUCTURE LAYER                         │
│   EF Core | SQL Server | Hangfire | Serilog                 │
│   Azure Blob Storage | SMTP/SendGrid | OpenAI API           │
└─────────────────────────────────────────────────────────────┘
```

---

## 3. Functional Requirements (System Level)

### 3.1 Authentication & Session Management

**SRS-AUTH-01:** The system shall implement JWT-based authentication with:
- Access tokens: 15-minute expiry, RS256 or HS256 signing
- Refresh tokens: 7-day expiry, stored as HTTP-only cookies OR in DB
- Refresh token rotation: invalidate old token on refresh
- Token blacklist for logout/revocation

**SRS-AUTH-02:** Password Requirements:
- Minimum 8 characters
- At least 1 uppercase, 1 lowercase, 1 digit, 1 special character
- Hashed using BCrypt with cost factor ≥ 12
- Previous 5 passwords cannot be reused

**SRS-AUTH-03:** Role-Based Access Control:
```
Roles:
  - Admin           → Full system access
  - HR              → Jobs, Candidates, Pipeline, Interviews, Reports
  - Recruiter       → Candidates, Applications, Pipeline
  - HiringManager   → View candidates, make hire/reject decisions
  - Interviewer     → View assigned interviews, submit feedback
  - Candidate       → Submit application, view own application status (public form)
```

### 3.2 Job Management

**SRS-JOBS-01:** Job entity schema (required fields):
```
Title (string, 3-200 chars)
Description (string, 20-5000 chars)
Department (FK to Department)
Location (string, nullable for remote)
JobType (enum: FullTime, PartTime, Contract, Remote, Hybrid)
SalaryRange (optional: Min, Max, Currency)
RequiredSkills (many-to-many with Skills)
ExperienceLevel (enum: Entry, Mid, Senior, Lead)
Status (enum: Draft, Active, Closed, Archived)
PublishedAt (datetime, nullable)
ExpiresAt (datetime, nullable)
MaxApplications (int, nullable)
PipelineStages (ordered list, customizable)
```

**SRS-JOBS-02:** Pipeline Stages (default, customizable):
```
Applied → Phone Screen → Technical Interview → Final Interview → Offer → Hired
                                                                      → Rejected
```

### 3.3 Candidate Management

**SRS-CAND-01:** Candidate profile schema:
```
FirstName, LastName (required)
Email (unique, required)
Phone (optional)
Location (optional)
LinkedInUrl (optional, URL format)
GitHubUrl (optional, URL format)
ResumeFileId (FK to FileStorage)
Skills (many-to-many)
Tags (free-text labels)
Source (enum: Manual, JobBoard, Referral, LinkedIn, GitHub)
Status (enum: Active, Hired, Archived, Blacklisted)
AIMatchScore (float, 0-100, computed)
Notes (text, internal recruiter notes)
```

### 3.4 Application Tracking

**SRS-APP-01:** Application entity:
```
CandidateId (FK)
JobId (FK)
CurrentStageId (FK to PipelineStage)
Status (enum: Active, Withdrawn, Rejected, Hired)
Source (where candidate applied from)
CoverLetter (text, optional)
AppliedAt (datetime)
LastActivityAt (datetime)
```

**SRS-APP-02:** Stage Transition Log:
```
ApplicationId (FK)
FromStageId (FK)
ToStageId (FK)
ChangedByUserId (FK)
ChangedAt (datetime)
Notes (optional text)
```

### 3.5 Interview Management

**SRS-INT-01:** Interview entity:
```
ApplicationId (FK)
Type (enum: PhoneScreen, Technical, Behavioral, CultureFit, Final)
Format (enum: Video, Phone, OnSite, Async)
ScheduledAt (datetime)
DurationMinutes (int)
MeetingLink (URL, optional)
Location (string, optional)
Status (enum: Scheduled, Completed, Cancelled, NoShow)
InterviewerIds (many-to-many, at least 1 required)
```

**SRS-INT-02:** Interview Feedback entity:
```
InterviewId (FK)
InterviewerId (FK)
CriterionScores: [
  { CriterionName, Score (1-5), Comments }
]
OverallScore (1-5, computed avg of criterion scores)
Recommendation (enum: StrongHire, Hire, Neutral, NoHire, StrongNoHire)
Summary (text)
AIGeneratedSummary (text, AI-generated)
SubmittedAt (datetime)
IsLate (bool, computed if submitted > 48h after interview)
```

### 3.6 Technical Assessments

**SRS-ASSESS-01:** Assessment template:
```
Title (string)
Description (text)
TotalTimeMinutes (int)
PassingScore (float, %)
AssessmentType (enum: MCQ, Coding, Mixed, OpenEnded)
Questions: [
  {
    Type (enum: MCQ, Coding, OpenEnded),
    Text (rich text),
    Options (for MCQ: list of options with IsCorrect flag),
    MaxScore (float),
    TimeLimit (int, seconds, optional per-question)
  }
]
```

**SRS-ASSESS-02:** Assessment attempt:
```
AssessmentId (FK)
CandidateId (FK)
ApplicationId (FK)
StartedAt (datetime)
SubmittedAt (datetime, nullable)
IsExpired (bool)
Answers: [
  { QuestionId, TextAnswer, SelectedOptionIds, CodeAnswer }
]
AutoScore (float, MCQ only)
ManualScore (float, nullable)
FinalScore (float, = AutoScore + ManualScore)
Status (enum: NotStarted, InProgress, Submitted, Evaluated)
```

### 3.7 AI Engine Requirements

**SRS-AI-01: Resume Parsing**
- Input: PDF or DOCX file
- Output: Structured JSON (name, email, phone, skills[], experience[], education[])
- Accuracy target: ≥ 85% for standard resume formats
- Fall-through: If parsing fails, flag for manual review; never block the workflow

**SRS-AI-02: Skill Extraction**
- NLP-based identification of technical and soft skills from free text
- Maps extracted skills to the system's canonical skills taxonomy
- Confidence score per extracted skill (threshold: 0.70)

**SRS-AI-03: Match Scoring**
- Algorithm: Weighted cosine similarity between job requirements vector and candidate skills vector
- Factors: Skills match (50%), Experience years (20%), Education (10%), Location (10%), Seniority (10%)
- Score range: 0–100, displayed to recruiter only

**SRS-AI-04: Interview Question Generation**
- Input: Job description + candidate profile + interview type
- Output: 8–12 targeted interview questions (behavioral + technical)
- Questions categorized by: Technical, Behavioral, Cultural, Situational

**SRS-AI-05: Feedback Summarization**
- Input: All interviewer feedback records for a candidate
- Output: Executive summary paragraph + recommendation
- Highlight areas of consensus and disagreement among interviewers

---

## 4. Non-Functional Requirements

### 4.1 Performance Requirements
| Requirement | Target | Measurement |
|---|---|---|
| API Latency (P95) | ≤ 200ms | Application Performance Monitoring |
| API Latency (P99) | ≤ 500ms | APM |
| Dashboard Load | ≤ 2 seconds (full render) | Browser DevTools |
| Concurrent Users | ≥ 500 | Load testing (k6 / JMeter) |
| File Upload (10MB) | ≤ 5 seconds | Integration test |
| SignalR Connection | ≤ 500 simultaneous connections | Load test |
| Database Query (indexed) | ≤ 50ms | EF Core logging |

### 4.2 Security Requirements

**SRS-SEC-01:** Transport Security
- TLS 1.2+ mandatory for all communications
- HTTP Strict Transport Security (HSTS) header enforced
- Certificate pinning not required (web app)

**SRS-SEC-02:** API Security Headers
```
Content-Security-Policy: default-src 'self'
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
Referrer-Policy: strict-origin-when-cross-origin
Permissions-Policy: camera=(), microphone=(), geolocation=()
```

**SRS-SEC-03:** Input Validation
- All inputs validated at API boundary (FluentValidation)
- All inputs validated at domain layer (value object constraints)
- HTML sanitization for all rich-text inputs
- File upload: validate MIME type, file size (max 10MB), extension whitelist (.pdf, .doc, .docx)

**SRS-SEC-04:** SQL Injection Prevention
- All database access through EF Core parameterized queries
- Raw SQL queries forbidden (use Dapper only for complex reporting with parameterized queries)

**SRS-SEC-05:** Authentication Security
- Brute force protection: rate limit login to 5 attempts / 10 minutes per IP
- Account lockout: 15-minute lockout after 5 failed attempts
- Sensitive operations (role change, delete) require re-authentication

### 4.3 Scalability Requirements
- Stateless API design (all state in database or distributed cache)
- Horizontal scaling: API pods can be added without architectural changes
- Database connection pooling via EF Core
- Background job queue (Hangfire) backed by SQL Server (production: Redis)
- File storage abstracted via interface (swap local ↔ Azure Blob without API changes)

### 4.4 Availability Requirements
- Target uptime: 99.5% (SLA)
- Planned maintenance windows: Sundays 02:00–04:00 UTC
- Recovery Time Objective (RTO): ≤ 1 hour
- Recovery Point Objective (RPO): ≤ 24 hours (daily backup)

---

## 5. External Interface Requirements

### 5.1 User Interfaces
- Angular 17 SPA with Angular Material components
- Responsive layout: 320px (mobile) → 1440px+ (desktop)
- Theme: Light and Dark mode support
- Real-time updates via SignalR

### 5.2 Software Interfaces
| External System | Purpose | Protocol |
|---|---|---|
| SQL Server 2022 | Primary data store | TCP/ADO.NET |
| OpenAI API | AI features (resume parsing, scoring, generation) | HTTPS/REST |
| SMTP / SendGrid | Email notifications | SMTP / HTTPS |
| Azure Blob Storage | Production file storage | HTTPS/SDK |
| Hangfire Dashboard | Background job monitoring | HTTP (internal) |
| Serilog + Seq | Structured logging | HTTP |

### 5.3 Communication Interfaces
- REST API: HTTPS, JSON payload, OpenAPI 3.0 spec
- WebSocket: SignalR for real-time notifications
- Background tasks: Hangfire recurring and triggered jobs
- File transfer: Multipart/form-data for uploads

---

## 6. Data Requirements

### 6.1 Data Volume Projections
| Entity | Year 1 (SMB) | Year 2 (SaaS) |
|---|---|---|
| Jobs | ~500 | ~50,000 |
| Candidates | ~5,000 | ~500,000 |
| Applications | ~15,000 | ~1,500,000 |
| Interviews | ~3,000 | ~300,000 |
| Audit Log Entries | ~100,000 | ~10,000,000 |
| File Storage | ~5 GB | ~500 GB |

### 6.2 Data Backup Strategy
- Full backup: Daily at 02:00 UTC, retained 30 days
- Differential backup: Every 4 hours, retained 7 days
- Transaction log backup: Every 30 minutes, retained 24 hours
- Backup validation: Weekly automated restore test

### 6.3 Data Classification
| Classification | Examples | Controls |
|---|---|---|
| **Public** | Job postings, company names | No restriction |
| **Internal** | Application statuses, pipeline stages | Authenticated access |
| **Confidential** | Candidate PII, salary data, AI scores | Role-restricted access |
| **Restricted** | Authentication credentials, audit logs | Admin-only + encrypted |

---

## 7. Quality Attributes Summary

| Attribute | Requirement |
|---|---|
| Correctness | All business rules validated at domain layer |
| Reliability | Hangfire retry (3 attempts); graceful degradation of AI features |
| Usability | 3-click workflow for core actions; WCAG 2.1 AA |
| Efficiency | P95 API ≤ 200ms; dashboard ≤ 2s |
| Maintainability | Clean Architecture; ≥ 80% test coverage on domain/application |
| Portability | Docker-ready; cloud-agnostic storage interface |
| Security | OWASP Top 10 mitigated; JWT + RBAC + input validation |
