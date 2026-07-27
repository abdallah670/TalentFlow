# TalentFlow — Frontend Implementation Plan

> **Last Updated:** 2026-07-26  
> **Project:** TalentFlow — AI-Powered Hiring & Candidate Assessment Platform  
> **Framework:** Angular 21 (Standalone Components)  
> **State Management:** NgRx + Signals (hybrid)  
> **UI Library:** Angular Material  

---

## Table of Contents

1. [Current State Assessment](#1-current-state-assessment)
2. [Architecture Overview](#2-architecture-overview)
3. [Dependency Installation](#3-dependency-installation)
4. [Phase 0 — Foundation & Bug Fixes](#4-phase-0--foundation--bug-fixes)
5. [Phase 1 — Core UI & Auth Pages](#5-phase-1--core-ui--auth-pages)
6. [Phase 2 — Shared Components & Admin Module](#6-phase-2--shared-components--admin-module)
7. [Phase 3 — Recruitment Core UI](#7-phase-3--recruitment-core-ui)
8. [Phase 4 — Interview & Assessment UI](#8-phase-4--interview--assessment-ui)
9. [Phase 5 — Analytics & Dashboard](#9-phase-5--analytics--dashboard)
10. [Phase 6 — Advanced Features & AI Placeholders](#10-phase-6--advanced-features--ai-placeholders)
11. [Phase 7 — Testing & Polish](#11-phase-7--testing--polish)
12. [Dependency Map](#12-dependency-map)
13. [File Structure (Target)](#13-file-structure-target)

---

## 1. Current State Assessment

### ✅ Already Built

| Layer | Files / Components | Status |
|-------|-------------------|--------|
| **Core — Auth Service** | `core/services/auth.service.ts` | ✅ Signals-based, JWT cookie storage, login/register/refresh/verify/reset methods |
| **Core — Auth Guard** | `core/guards/auth-guard.ts` | ✅ Exists but **BUG**: blocks non-admin users |
| **Core — Admin Guard** | `core/guards/admin-guard.ts` | ✅ Works correctly |
| **Core — Token Interceptor** | `core/interceptors/token-interceptor.ts` | ✅ Auto-attaches Bearer token, handles 401 refresh |
| **Core — Error Handler** | `core/errors/global-error-handler.ts` | ✅ Uses MatSnackBar |
| **Core Module** | `core/core.module.ts` | ✅ Singleton guard pattern |
| **Data — Auth Models** | `data/models/auth.model.ts` | ✅ AuthRequest, AuthResponse, RegistrationRequest, etc. |
| **Environments** | `environments/` | ✅ dev + prod configs with API base URL |
| **App Shell** | `app.ts`, `app.config.ts` | ✅ Standalone bootstrap, RouterOutlet |
| **Folder Scaffolding** | `domain/`, `data/`, `presentation/` | ✅ Empty folders following Clean Architecture |

### ❌ Missing / Needs Work

| Area | Details | Priority |
|------|---------|----------|
| **Routes** | `app.routes.ts` has empty array | 🔴 Critical |
| **Auth Guard Bug** | Blocks all non-admin users (line 39-51) | 🔴 Critical |
| **Login Page** | Not built | 🔴 Critical |
| **Registration Page** | Not built | 🔴 Critical |
| **Main Layout** | Sidebar + Header not built | 🔴 Critical |
| **Dependencies** | Material, NgRx, SignalR, Chart.js not installed | 🔴 Critical |
| **app.html** | Still has Angular default placeholder | 🟡 Medium |
| **Feature Modules** (10) | All empty folders | 🟡 Medium |
| **Shared Components** | None built | 🟡 Medium |
| **NgRx State** | Not set up (auth uses signals only) | 🟡 Medium |
| **Missing Packages** | sweetalert2, ngx-cookie-service not in package.json | 🟡 Medium |

---

## 2. Architecture Overview

### Technology Stack

| Technology | Purpose |
|-----------|---------|
| **Angular 21** | Core framework (standalone components) |
| **Angular Material** | UI components (table, dialog, card, form fields, snackbar) |
| **NgRx** | Global state management (Store, Effects, Selectors) |
| **Angular Signals** | Local component state, computed values |
| **Chart.js / ngx-charts** | Analytics dashboards |
| **@microsoft/signalr** | Real-time notifications |
| **RxJS** | Reactive programming, HTTP, WebSocket |
| **Vitest** | Unit testing |

### State Management Strategy

```
┌─────────────────────────────────────────────────────┐
│                    NgRx Store                        │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌────────┐ │
│  │ AuthState│ │ JobState │ │CandState │ │AppState│ │
│  └──────────┘ └──────────┘ └──────────┘ └────────┘ │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌────────┐ │
│  │IntervState│ │AssessSt.│ │OfferState│ │NotifSt.│ │
│  └──────────┘ └──────────┘ └──────────┘ └────────┘ │
│  ┌──────────┐                                       │
│  │AnalytSt. │                                       │
│  └──────────┘                                       │
└─────────────────────────────────────────────────────┘
         ▲                    ▲
         │ HTTP               │ SignalR
         ▼                    ▼
┌─────────────────┐  ┌─────────────────┐
│  API Services   │  │ NotificationHub │
└─────────────────┘  └─────────────────┘
```

**Rule of thumb:**
- **NgRx** for data shared across components (auth, jobs list, candidates, pipeline)
- **Signals** for local UI state (form loading, dropdown open/close, selected filters)

### Mock Backend Strategy

To develop UI independently while waiting for real API endpoints, use an `HttpInterceptor` that intercepts API calls and returns mock data:

```
┌──────────────┐     ┌──────────────────┐     ┌──────────────┐
│  Component   │ ──► │  MockInterceptor │ ──► │  Real API    │
│  (dev mode)  │     │  (if mock=true)  │     │  (prod mode) │
└──────────────┘     └──────────────────┘     └──────────────┘
```

---

## 3. Dependency Installation

### Command
```bash
npm install @angular/material @ngrx/store @ngrx/effects @ngrx/entity @microsoft/signalr chart.js ngx-charts sweetalert2 ngx-cookie-service
```

### Post-Install
```bash
ng add @angular/material  # Sets up theme, animations, typography
```

### package.json additions needed
```json
{
  "dependencies": {
    "@angular/material": "^21.2.0",
    "@ngrx/store": "^21.0.0",
    "@ngrx/effects": "^21.0.0",
    "@ngrx/entity": "^21.0.0",
    "@microsoft/signalr": "^8.0.0",
    "chart.js": "^4.4.0",
    "ngx-charts": "^20.0.0",
    "sweetalert2": "^11.0.0",
    "ngx-cookie-service": "^19.0.0"
  }
}
```

---

## 4. Phase 0 — Foundation & Bug Fixes

**Goal:** Fix critical bugs, install dependencies, clean up scaffolding.

### Tasks

| # | Task | File(s) | Details |
|---|------|---------|---------|
| 0.1 | **Install dependencies** | `package.json` | Install Material, NgRx, SignalR, Chart.js, sweetalert2, ngx-cookie-service |
| 0.2 | **Fix Auth Guard** | `core/guards/auth-guard.ts` | Remove lines 39-51 (admin check). Guard should only verify: authenticated + email confirmed |
| 0.3 | **Clean App Shell** | `app.html`, `app.css` | Replace default Angular template with just `<router-outlet />` |
| 0.4 | **Add global styles** | `styles.css` | Import Angular Material theme, define CSS variables |
| 0.5 | **Add missing packages to package.json** | `package.json` | Add sweetalert2, ngx-cookie-service to dependencies |
| 0.6 | **Set up Angular Material theme** | `styles.css`, `angular.json` | Configure Material theme, include typography/animations |

### Files Modified
- `frontend/package.json`
- `frontend/src/app/core/guards/auth-guard.ts`
- `frontend/src/app/app.html`
- `frontend/src/app/app.css`
- `frontend/src/styles.css`
- `frontend/angular.json`

---

## 5. Phase 1 — Core UI & Auth Pages

**Goal:** Build login/registration flow, main layout shell, and routing structure.

### Tasks

| # | Task | Component | Files to Create | Details |
|---|------|-----------|-----------------|---------|
| 1.1 | **Login Page** | `LoginComponent` | `presentation/pages/login/` | Email/password form, social login buttons (Google/Facebook), validation, loading spinner, error display |
| 1.2 | **Registration Page** | `RegisterComponent` | `presentation/pages/register/` | First name, last name, email, username, password fields, validation, success/error feedback |
| 1.3 | **Email Verification** | `VerifyEmailComponent` | `presentation/pages/verify-email/` | Token handling from query params, resend verification button |
| 1.4 | **Forgot Password** | `ForgotPasswordComponent` | `presentation/pages/forgot-password/` | Email input, submit, success message |
| 1.5 | **Reset Password** | `ResetPasswordComponent` | `presentation/pages/reset-password/` | Token from query params, new password + confirm, validation |
| 1.6 | **Main Layout** | `MainLayoutComponent` | `presentation/layouts/main-layout/` | Sidebar navigation (collapsible), top header (user menu, notification bell placeholder, tenant branding), `<router-outlet>` content area, responsive |
| 1.7 | **App Routing** | — | `app.routes.ts` | Lazy-loaded routes for all 10 feature modules with guards |
| 1.8 | **NgRx Auth State** | — | `core/state/auth/` | Actions (login, logout, refresh, register), Effects, Reducers, Selectors. Migrate from signal-only approach |

### Route Structure

```typescript
const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'verify-email', component: VerifyEmailComponent },
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  {
    path: '',
    canActivate: [authGuard],
    component: MainLayoutComponent,
    children: [
      { path: 'dashboard', loadChildren: () => import('./presentation/pages/dashboard/dashboard.routes') },
      { path: 'jobs', loadChildren: () => import('./presentation/pages/jobs/jobs.routes') },
      { path: 'candidates', loadChildren: () => import('./presentation/pages/candidates/candidates.routes') },
      { path: 'applications/:jobId', loadChildren: () => import('./presentation/pages/applications/applications.routes') },
      { path: 'interviews', loadChildren: () => import('./presentation/pages/interviews/interviews.routes') },
      { path: 'assessments', loadChildren: () => import('./presentation/pages/assessments/assessments.routes') },
      { path: 'offers', loadChildren: () => import('./presentation/pages/offers/offers.routes') },
      { path: 'analytics', loadChildren: () => import('./presentation/pages/analytics/analytics.routes'), canActivate: [roleGuard], data: { permission: 'analytics.view' } },
      { path: 'admin', loadChildren: () => import('./presentation/pages/admin/admin.routes'), canActivate: [adminGuard] },
      { path: 'notifications', loadChildren: () => import('./presentation/pages/notifications/notifications.routes') },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },
  { path: '**', component: NotFoundComponent }
];
```

### Files Created
- `frontend/src/app/presentation/pages/login/login.component.ts`
- `frontend/src/app/presentation/pages/register/register.component.ts`
- `frontend/src/app/presentation/pages/verify-email/verify-email.component.ts`
- `frontend/src/app/presentation/pages/forgot-password/forgot-password.component.ts`
- `frontend/src/app/presentation/pages/reset-password/reset-password.component.ts`
- `frontend/src/app/presentation/layouts/main-layout/main-layout.component.ts`
- `frontend/src/app/presentation/layouts/main-layout/main-layout.component.html`
- `frontend/src/app/presentation/layouts/main-layout/main-layout.component.css`
- `frontend/src/app/presentation/components/not-found/not-found.component.ts`
- `frontend/src/app/core/state/auth/auth.actions.ts`
- `frontend/src/app/core/state/auth/auth.effects.ts`
- `frontend/src/app/core/state/auth/auth.reducer.ts`
- `frontend/src/app/core/state/auth/auth.selectors.ts`
- `frontend/src/app/core/state/auth/auth.state.ts`

---

## 6. Phase 2 — Shared Components & Admin Module

**Goal:** Build reusable component library and admin management UI.

### Shared Components

| # | Component | Purpose | Inputs | Outputs |
|---|-----------|---------|--------|---------|
| 2.1 | `DataTableComponent` | Generic paginated/sortable table | `columns: ColumnConfig[]`, `data: Observable<any[]>`, `pageSize`, `totalItems` | `pageChange`, `sortChange`, `rowClick` |
| 2.2 | `StatusBadgeComponent` | Color-coded status badge | `status: string`, `type: 'job' | 'candidate' | 'offer'` | — |
| 2.3 | `FileUploadComponent` | Drag-and-drop file upload | `acceptedTypes: string`, `maxSize: number` | `fileSelected: FileEvent` |
| 2.4 | `ConfirmDialogComponent` | Reusable confirmation modal | `title: string`, `message: string`, `confirmText`, `cancelText` | `confirm: void`, `cancel: void` |
| 2.5 | `EmptyStateComponent` | Empty state placeholder | `icon: string`, `title: string`, `message: string`, `actionText: string` | `action: void` |
| 2.6 | `PermissionDirective` | Show/hide by permission | `appIfPermission: string` | — |

### Admin Module

| # | Task | Component | Details |
|---|------|-----------|---------|
| 2.7 | **User Management** | `UserManagementComponent` | User list table, create/edit dialog, role assignment dropdown |
| 2.8 | **Role Management** | `RoleManagementComponent` | Role list, create/edit, permission assignment with checkboxes |
| 2.9 | **Tenant Settings** | `TenantSettingsComponent` | Branding: logo upload, color picker, timezone select, notification defaults |

### Files Created
- `frontend/src/app/presentation/components/data-table/data-table.component.ts`
- `frontend/src/app/presentation/components/status-badge/status-badge.component.ts`
- `frontend/src/app/presentation/components/file-upload/file-upload.component.ts`
- `frontend/src/app/presentation/components/confirm-dialog/confirm-dialog.component.ts`
- `frontend/src/app/presentation/components/empty-state/empty-state.component.ts`
- `frontend/src/app/presentation/directives/permission.directive.ts`
- `frontend/src/app/presentation/pages/admin/user-management/user-management.component.ts`
- `frontend/src/app/presentation/pages/admin/role-management/role-management.component.ts`
- `frontend/src/app/presentation/pages/admin/tenant-settings/tenant-settings.component.ts`

---

## 7. Phase 3 — Recruitment Core UI

**Goal:** Build jobs, candidates, pipeline board, and application management UI.

### Jobs Module

| # | Task | Component | Details |
|---|------|-----------|---------|
| 3.1 | **Department List** | `DepartmentListComponent` | Simple CRUD table with inline edit |
| 3.2 | **Skill Management** | `SkillListComponent` | Tag-style list with add/remove |
| 3.3 | **Job List** | `JobListComponent` | Paginated table: Title, Department, Status, Candidate Count, Created Date. Filters: status, department, search |
| 3.4 | **Job Form** | `JobFormComponent` | Reactive form with FormArray for custom fields, pipeline template selector, validation |
| 3.5 | **Job Detail** | `JobDetailComponent` | Job info display, assigned pipeline visualization, mini funnel chart, quick actions (publish, close) |

### Candidates Module

| # | Task | Component | Details |
|---|------|-----------|---------|
| 3.6 | **Candidate List** | `CandidateListComponent` | Search by name/email, filter by source/skills, paginated table with quick actions |
| 3.7 | **Candidate Profile** | `CandidateProfileComponent` | Personal info card, resume download, skills/experience/education, application history timeline |
| 3.8 | **Candidate Import** | `CandidateImportComponent` | CSV upload with drag-drop, column mapping preview, validation errors |

### Applications Module

| # | Task | Component | Details |
|---|------|-----------|---------|
| 3.9 | **Pipeline Board** | `PipelineBoardComponent` | Kanban columns per stage. Candidate cards: name, avatar placeholder, days in stage. Stage move via dropdown |
| 3.10 | **Application Detail** | `ApplicationDetailComponent` | Candidate info summary, stage history timeline, action buttons (move stage, schedule interview, generate offer) |
| 3.11 | **Stage Transition Dialog** | `StageTransitionDialog` | Modal: current stage, allowed next stages dropdown, mandatory fields, comment box, validation |

### NgRx States

| # | State | Actions | Effects |
|---|-------|---------|---------|
| 3.12 | **JobState** | loadJobs, createJob, updateJob, deleteJob, publishJob, closeJob | API calls with filter/sort/pagination |
| 3.13 | **CandidateState** | loadCandidates, createCandidate, updateCandidate, deleteCandidate, importCandidates | API calls with search/filter/pagination |
| 3.14 | **ApplicationState** | loadPipeline, moveStage, rejectApplication, hireCandidate | Pipeline data grouped by stage |

### Files Created
- `frontend/src/app/presentation/pages/jobs/job-list/job-list.component.ts`
- `frontend/src/app/presentation/pages/jobs/job-form/job-form.component.ts`
- `frontend/src/app/presentation/pages/jobs/job-detail/job-detail.component.ts`
- `frontend/src/app/presentation/pages/jobs/department-list/department-list.component.ts`
- `frontend/src/app/presentation/pages/jobs/skill-list/skill-list.component.ts`
- `frontend/src/app/presentation/pages/candidates/candidate-list/candidate-list.component.ts`
- `frontend/src/app/presentation/pages/candidates/candidate-profile/candidate-profile.component.ts`
- `frontend/src/app/presentation/pages/candidates/candidate-import/candidate-import.component.ts`
- `frontend/src/app/presentation/pages/applications/pipeline-board/pipeline-board.component.ts`
- `frontend/src/app/presentation/pages/applications/application-detail/application-detail.component.ts`
- `frontend/src/app/presentation/pages/applications/stage-transition-dialog/stage-transition-dialog.component.ts`
- `frontend/src/app/core/state/job/job.actions.ts`, `job.effects.ts`, `job.reducer.ts`, `job.selectors.ts`
- `frontend/src/app/core/state/candidate/candidate.actions.ts`, `candidate.effects.ts`, `candidate.reducer.ts`, `candidate.selectors.ts`
- `frontend/src/app/core/state/application/application.actions.ts`, `application.effects.ts`, `application.reducer.ts`, `application.selectors.ts`

---

## 8. Phase 4 — Interview & Assessment UI

**Goal:** Build interview scheduling, feedback, assessments, offers, and notifications.

### Interviews Module

| # | Task | Component | Details |
|---|------|-----------|---------|
| 4.1 | **Schedule Interview Dialog** | `ScheduleInterviewDialog` | Date/time picker, duration, location/URL, interview type dropdown, interviewer multi-select with search |
| 4.2 | **Interview Feedback Form** | `InterviewFeedbackFormComponent` | Numeric score (1-5), strengths/weaknesses textareas, recommendation (Hire/No-Hire/Hold), validation |

### Assessments Module

| # | Task | Component | Details |
|---|------|-----------|---------|
| 4.3 | **Assessment Builder** | `AssessmentBuilderComponent` | Title, description, passing score, dynamic question list (add/edit/remove: multiple-choice or text) |
| 4.4 | **Assessment Assignment** | `AssessmentAssignmentComponent` | Assign to candidate, view status, due date |
| 4.5 | **Assessment Submission** | `AssessmentSubmissionComponent` | Display candidate answers, auto-calculated score for multiple-choice, manual score for text |

### Offers Module

| # | Task | Component | Details |
|---|------|-----------|---------|
| 4.6 | **Offer Creation** | `OfferCreateComponent` | Salary (min/max), start date, expiration date, benefits, offer letter text editor |
| 4.7 | **Offer Approval** | `OfferApprovalComponent` | Pending approvals list, approve/reject with comment modal |
| 4.8 | **Offer Tracker** | `OfferTrackerComponent` | Visual status timeline: Draft → Pending → Approved → Sent → Accepted/Declined/Expired |

### Notifications Module

| # | Task | Component | Details |
|---|------|-----------|---------|
| 4.9 | **Notification Bell** | `NotificationBellComponent` | Bell icon in header with unread count badge. Subscribes to SignalR hub |
| 4.10 | **Notification List** | `NotificationListComponent` | Paginated list with type icons, message, timestamp. Mark as read individually or all |

### NgRx States

| # | State | Details |
|---|-------|---------|
| 4.11 | **InterviewState** | Actions: load, schedule, submitFeedback |
| 4.12 | **AssessmentState** | Actions: load, create, assign, submitAnswers, getResults |
| 4.13 | **OfferState** | Actions: load, create, approve, reject, send, accept, decline |
| 4.14 | **NotificationState** | Actions: load, markRead, markAllRead, newNotification (from SignalR) |

---

## 9. Phase 5 — Analytics & Dashboard

**Goal:** Build KPI dashboard, charts, and audit log viewer.

### Dashboard Module

| # | Task | Component | Details |
|---|------|-----------|---------|
| 5.1 | **Dashboard Summary** | `DashboardComponent` | Summary cards: Open Jobs, Active Candidates, Scheduled Interviews, Offers Sent. Icons + trend indicators. Quick action buttons |
| 5.2 | **Time-to-Hire Chart** | `TimeToHireChartComponent` | Line chart with date range picker and department filter |
| 5.3 | **Pipeline Funnel** | `FunnelChartComponent` | Horizontal bar chart: candidate count per stage + conversion % |
| 5.4 | **Source Effectiveness** | `SourceEffectivenessChartComponent` | Pie/donut chart: candidate distribution by source |
| 5.5 | **Recruiter Performance** | `RecruiterPerformanceComponent` | Bar chart: candidates processed per recruiter |
| 5.6 | **CSV/PNG Export** | — | Export buttons on all charts |

### Audit Module

| # | Task | Component | Details |
|---|------|-----------|---------|
| 5.7 | **Audit Log Viewer** | `AuditLogViewerComponent` | Paginated table with filters (entity type, user, date range), expandable rows showing old/new values JSON diff |

### NgRx State

| # | State | Details |
|---|-------|---------|
| 5.8 | **AnalyticsState** | Actions: loadSummary, loadFunnel, loadAnalytics. Selectors for all chart data |

---

## 10. Phase 6 — Advanced Features & AI Placeholders

**Goal:** Tenant branding, pipeline builder, bulk operations, global search, AI placeholders.

| # | Task | Component | Details |
|---|------|-----------|---------|
| 6.1 | **Tenant Branding** | — | On login, fetch tenant settings → set CSS variables via `document.documentElement.style.setProperty`. Apply logo in header |
| 6.2 | **Pipeline Template Builder** | `PipelineTemplateBuilderComponent` | Ordered list of stages with add/edit/delete/reorder (drag-and-drop or up/down buttons). Each stage: name, allowed next stages, required permission, mandatory feedback flag |
| 6.3 | **Advanced Pipeline UI** | — | Visual indicators: parallel stages badge, conditional transition labels, auto-advancement badges |
| 6.4 | **Bulk Operations** | — | Checkbox selection on candidate list, bulk actions toolbar: "Move to Stage" dropdown, "Bulk Reject" with confirmation |
| 6.5 | **Platform Admin Dashboard** | `PlatformAdminDashboardComponent` | All tenants table (name, subdomain, status, user count), health metrics, usage stats |
| 6.6 | **Global Search** | `GlobalSearchComponent` | Search input in header with debounce, results dropdown grouped by entity type (Jobs, Candidates, Interviews, Assessments) |
| 6.7 | **AI Feature Placeholders** | — | Feature-flagged UI: "AI Match Score" badge on candidate cards, "Rank by AI" button, resume parsing status indicator |

---

## 11. Phase 7 — Testing & Polish

**Goal:** Unit tests, accessibility, responsive design.

| # | Task | Details |
|---|------|---------|
| 7.1 | **Unit Tests** | Vitest tests for all components (rendering, user interaction, state changes), services (API calls, error handling), NgRx effects (success/failure paths), reducers (state transitions). Target >80% coverage |
| 7.2 | **Accessibility Audit** | Run axe DevTools / Lighthouse. Fix: color contrast, missing ARIA labels, keyboard navigation, focus management in modals |
| 7.3 | **Responsive Design Polish** | Test mobile/tablet viewports. Fix: sidebar collapse, table-to-card layout switch, pipeline board accordion on small screens, touch-friendly controls |

---

## 12. Dependency Map

```
Phase 0: Install Dependencies + Fix Auth Guard + Clean Shell
    │
    ▼
Phase 1: Login/Register Pages + Layout + Routes + Auth NgRx
    │
    ▼
Phase 2: Shared Components Library + Admin Module (Users, Roles, Settings)
    │
    ▼
Phase 3: Jobs → Candidates → Pipeline Board + NgRx States
    │
    ▼
Phase 4: Interviews → Assessments → Offers → Notifications + NgRx States
    │
    ▼
Phase 5: Dashboard + Charts + Audit Log + Analytics NgRx
    │
    ▼
Phase 6: Tenant Branding → Pipeline Builder → Bulk Ops → Search → AI Placeholders
    │
    ▼
Phase 7: Unit Tests → Accessibility → Responsive Polish
```

### Blocking Dependencies

| Phase | Blocks | Blocked By |
|-------|--------|------------|
| Phase 0 | All phases | Nothing |
| Phase 1 | Phases 2-7 | Phase 0 |
| Phase 2 | Phases 3-7 | Phase 1 |
| Phase 3 | Phases 4-7 | Phase 2 |
| Phase 4 | Phases 5-7 | Phase 3 |
| Phase 5 | Phases 6-7 | Phase 4 |
| Phase 6 | Phase 7 | Phase 5 |
| Phase 7 | — | All previous |

---

## 13. File Structure (Target)

```
frontend/src/app/
├── app.config.ts
├── app.routes.ts
├── app.ts
├── app.html
├── app.css
│
├── core/
│   ├── guards/
│   │   ├── auth-guard.ts
│   │   ├── admin-guard.ts
│   │   └── role-guard.ts
│   ├── interceptors/
│   │   ├── token-interceptor.ts
│   │   ├── error-interceptor.ts
│   │   └── mock-interceptor.ts          # Dev-only mock backend
│   ├── services/
│   │   ├── auth.service.ts
│   │   ├── notification.service.ts       # SignalR hub connection
│   │   └── tenant-context.service.ts
│   ├── errors/
│   │   └── global-error-handler.ts
│   └── state/
│       ├── auth/
│       │   ├── auth.actions.ts
│       │   ├── auth.effects.ts
│       │   ├── auth.reducer.ts
│       │   ├── auth.selectors.ts
│       │   └── auth.state.ts
│       ├── job/
│       ├── candidate/
│       ├── application/
│       ├── interview/
│       ├── assessment/
│       ├── offer/
│       ├── notification/
│       └── analytics/
│
├── data/
│   ├── models/
│   │   ├── auth.model.ts
│   │   ├── job.model.ts
│   │   ├── candidate.model.ts
│   │   ├── application.model.ts
│   │   ├── interview.model.ts
│   │   ├── assessment.model.ts
│   │   ├── offer.model.ts
│   │   ├── notification.model.ts
│   │   └── analytics.model.ts
│   ├── datasources/
│   │   ├── auth.datasource.ts
│   │   ├── job.datasource.ts
│   │   ├── candidate.datasource.ts
│   │   └── ... (one per feature)
│   └── repositories/
│       ├── auth.repository.ts
│       ├── job.repository.ts
│       └── ... (one per feature)
│
├── domain/
│   ├── entities/
│   │   ├── user.entity.ts
│   │   ├── job.entity.ts
│   │   ├── candidate.entity.ts
│   │   └── ... (one per domain model)
│   ├── repositories/
│   │   ├── i-auth-repository.ts
│   │   ├── i-job-repository.ts
│   │   └── ... (interfaces)
│   └── usecases/
│       ├── login.usecase.ts
│       ├── create-job.usecase.ts
│       └── ... (one per business operation)
│
└── presentation/
    ├── components/          # Shared components
    │   ├── data-table/
    │   ├── status-badge/
    │   ├── file-upload/
    │   ├── confirm-dialog/
    │   └── empty-state/
    ├── directives/
    │   └── permission.directive.ts
    ├── layouts/
    │   ├── main-layout/
    │   │   ├── main-layout.component.ts
    │   │   ├── main-layout.component.html
    │   │   ├── main-layout.component.css
    │   │   ├── sidebar/
    │   │   └── header/
    │   └── admin-layout/
    └── pages/
        ├── login/
        ├── register/
        ├── verify-email/
        ├── forgot-password/
        ├── reset-password/
        ├── dashboard/
        ├── jobs/
        │   ├── job-list/
        │   ├── job-form/
        │   ├── job-detail/
        │   ├── department-list/
        │   └── skill-list/
        ├── candidates/
        │   ├── candidate-list/
        │   ├── candidate-profile/
        │   └── candidate-import/
        ├── applications/
        │   ├── pipeline-board/
        │   ├── application-detail/
        │   └── stage-transition-dialog/
        ├── interviews/
        │   ├── schedule-interview-dialog/
        │   └── interview-feedback-form/
        ├── assessments/
        │   ├── assessment-builder/
        │   ├── assessment-assignment/
        │   └── assessment-submission/
        ├── offers/
        │   ├── offer-create/
        │   ├── offer-approval/
        │   └── offer-tracker/
        ├── analytics/
        │   ├── dashboard/
        │   ├── time-to-hire-chart/
        │   ├── funnel-chart/
        │   ├── source-effectiveness/
        │   └── recruiter-performance/
        ├── admin/
        │   ├── user-management/
        │   ├── role-management/
        │   ├── tenant-settings/
        │   ├── audit-log-viewer/
        │   ├── pipeline-template-builder/
        │   └── platform-admin/
        ├── notifications/
        │   ├── notification-bell/
        │   └── notification-list/
        └── not-found/
```

---

## Summary: Total Effort Estimate

| Phase | Tasks | Estimated Time | Dependencies |
|-------|-------|---------------|--------------|
| **Phase 0** — Foundation & Bug Fixes | 6 | 1 day | None |
| **Phase 1** — Core UI & Auth Pages | 8 | 1 week | Phase 0 |
| **Phase 2** — Shared Components & Admin | 9 | 1 week | Phase 1 |
| **Phase 3** — Recruitment Core UI | 14 | 2 weeks | Phase 2 |
| **Phase 4** — Interview & Assessment UI | 14 | 2 weeks | Phase 3 |
| **Phase 5** — Analytics & Dashboard | 8 | 1 week | Phase 4 |
| **Phase 6** — Advanced Features & AI | 7 | 1.5 weeks | Phase 5 |
| **Phase 7** — Testing & Polish | 3 | 1 week | All previous |
| **TOTAL** | **69 tasks** | **~10.5 weeks** | |