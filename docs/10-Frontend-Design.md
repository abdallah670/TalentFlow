# 10. Frontend Design Document (TalentFlow)

## Technology Stack
- **Angular 17+** – Core framework for the SPA.
- **Angular Material** – UI component library (mat-table, mat-dialog, mat-card, etc.) and theming.
- **NgRx** – State management (Store, Effects, Selectors).
- **Chart.js** (or ngx-charts) – Analytics dashboards.
- **Angular CDK** – Drag‑and‑drop for future pipeline board interactions.
- **SignalR client** (`@microsoft/signalr`) – Real‑time in‑app notifications.
- **RxJS** – Reactive programming throughout.

## Application Structure (Feature‑based)
src/
├── app/
│ ├── core/ # Singleton services, guards, interceptors
│ │ ├── auth/ # Login/logout, token refresh
│ │ ├── interceptors/ # JWT interceptor, error interceptor, tenant header
│ │ ├── guards/ # AuthGuard, TenantGuard, RoleGuard
│ │ └── services/ # TenantContextService, NotificationService (SignalR)
│ ├── shared/ # Reusable components, directives, pipes
│ │ ├── components/ # DataTable, StatusBadge, FileUpload, ConfirmDialog
│ │ ├── directives/ # PermissionDirective (show/hide based on permissions)
│ │ └── pipes/ # TimeAgo, StatusLabel, FileSize
│ ├── features/
│ │ ├── dashboard/ # Home dashboard with KPIs
│ │ ├── jobs/ # Job list, create/edit, job detail
│ │ ├── candidates/ # Candidate list, profile, import
│ │ ├── applications/ # Pipeline board, application detail
│ │ ├── interviews/ # Schedule interview dialog, feedback form
│ │ ├── assessments/ # Assessment builder, submission viewer
│ │ ├── offers/ # Offer creation, approval, tracking
│ │ ├── analytics/ # Analytics module with charts
│ │ ├── admin/ # Tenant admin: users, roles, pipeline templates, audit log
│ │ └── notifications/ # Notification list, bell icon
│ ├── layout/ # Main layout shell, sidebar, header, footer
│ └── app-routing.module.ts # Lazy‑loaded routes
├── assets/ # Images, i18n files
├── environments/ # Environment config
└── styles/ # Global SCSS, theme files

text

## Routing Design (Lazy‑Loaded)

Routes are lazy‑loaded for performance. Route guards protect each feature.

```typescript
const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    canActivate: [AuthGuard, TenantGuard],
    component: MainLayoutComponent,
    children: [
      { path: 'dashboard', loadChildren: () => import('./features/dashboard/dashboard.module').then(m => m.DashboardModule) },
      { path: 'jobs', loadChildren: () => import('./features/jobs/jobs.module').then(m => m.JobsModule) },
      { path: 'candidates', loadChildren: () => import('./features/candidates/candidates.module').then(m => m.CandidatesModule) },
      { path: 'applications/:jobId', loadChildren: () => import('./features/applications/applications.module').then(m => m.ApplicationsModule) },
      { path: 'interviews', loadChildren: ... },
      { path: 'offers', loadChildren: ... },
      { path: 'analytics', loadChildren: ..., canActivate: [RoleGuard], data: { permission: 'analytics.view' } },
      { path: 'admin', loadChildren: ..., canActivate: [RoleGuard], data: { roles: ['TenantAdmin'] } },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },
  { path: '**', component: NotFoundComponent }
];
AuthGuard – Verifies JWT validity and refresh token flow.

TenantGuard – Ensures tenant_id exists in claims; triggers tenant‑specific setup.

RoleGuard – Checks for required roles or permissions.

Feature Modules & Components
Dashboard Module
DashboardComponent – Summary cards (Open Jobs, Active Candidates, Time‑to‑Hire average), quick action buttons.

Jobs Module
JobListComponent – Paginated data table with columns: Title, Department, Status, Candidate Count, Created Date. Inline filtering by status and department.

JobFormComponent – Reactive form with dynamic custom fields (FormArray). Pipeline template selector.

JobDetailComponent – Displays job details, assigned pipeline, and a mini pipeline funnel.

Candidates Module
CandidateListComponent – Search by name/email, filter by source/skills. Paginated table with quick actions (view, delete).

CandidateProfileComponent – Shows candidate info, resume download link (proxied), application history (timeline), current stage per application.

CandidateImportComponent – CSV upload with column mapping preview.

Applications Module (Pipeline Board)
PipelineBoardComponent – Columns per pipeline stage. Each column displays candidate cards (name, days in stage). Future drag‑and‑drop via Angular CDK.

ApplicationDetailComponent – Candidate info, stage history timeline, actions (move stage, schedule interview, generate offer).

StageTransitionDialog – Reusable modal; shows current stage, dropdown of allowed next stages, mandatory field inputs (if required), comment box.

Interviews Module
ScheduleInterviewDialog – Form with date/time picker, location/URL, interview type, interviewer multi‑select.

InterviewFeedbackFormComponent – Structured form: score (1‑5), strengths, weaknesses, recommendation.

Assessments Module
AssessmentBuilderComponent – Tenant admin can create/edit assessments: title, description, passing score, dynamic question list (multiple‑choice, text).

AssessmentSubmissionComponent – Candidate answers view; shows auto‑calculated score.

Offers Module
OfferCreateComponent – Form with salary, start date, expiry, benefits.

OfferApprovalComponent – Approvers see pending offers; approve/reject with comment.

OfferTrackerComponent – Status timeline: Draft → Pending → Approved → Sent → Accepted/Declined.

Analytics Module
AnalyticsDashboardComponent – Contains sub‑components:

TimeToHireChartComponent – Line chart with filters (department, date range).

FunnelChartComponent – Bar chart showing candidate counts per stage with conversion percentages.

SourceEffectivenessChartComponent – Pie chart.

RecruiterPerformanceComponent – Bar chart.

All support CSV/PNG export.

Admin Module (Tenant Admin)
UserManagementComponent – User list, create/edit user, role assignment.

RoleManagementComponent – Create/edit roles, assign permissions.

PipelineTemplateBuilderComponent – Drag‑and‑drop stage editor (future visual), or a simple ordered list with add/edit/delete stages.

AuditLogViewerComponent – Paginated table with filters (entity type, user, date), expandable old/new values diff.

TenantSettingsComponent – Branding (logo upload, color picker), timezone, notification defaults.

Notifications Module
NotificationBellComponent – Displays unread count badge in header; subscribes to SignalR hub; updates in real‑time.

NotificationListComponent – Paginated list of notifications; mark as read individually or all.

Shared Components
DataTableComponent – Generic component. Inputs: columns config (field, header, sortable, template), data source (observable), pagination options. Outputs: pageChange, sortChange, rowClick. Used across lists.

StatusBadgeComponent – Color‑coded badge for entity status (Job: Open/Closed, Candidate: Active/Hired, Offer: Sent/Accepted).

FileUploadComponent – Drag‑and‑drop area, progress bar, file type/size validation. Emits file metadata on success.

ConfirmDialogComponent – Reusable confirmation modal.

PermissionDirective (*appIfPermission="'job.create'") – Shows/hides elements based on permissions from auth state.

State Management (NgRx)
Store Modules
AuthState – { user, tenant, tokens, isAuthenticated }

JobState – { jobs, selectedJob, loading, filters }

CandidateState – { candidates, selectedCandidate, filters }

ApplicationState – { pipelineData (grouped by stage), currentApplication, history }

InterviewState – { interviews, feedback }

OfferState – { offers, approvals }

NotificationState – { notifications, unreadCount }

AnalyticsState – { timeToHireData, funnelData, sourceData, recruiterData }

Sample Effect: Load Pipeline Board
typescript
loadPipelineBoard$ = createEffect(() =>
  this.actions$.pipe(
    ofType(ApplicationActions.loadPipeline),
    switchMap(({ jobId }) =>
      this.applicationService.getPipeline(jobId).pipe(
        map(data => ApplicationActions.loadPipelineSuccess({ data })),
        catchError(error => of(ApplicationActions.loadPipelineFailure({ error })))
      )
    )
  )
);
Selector Example
typescript
export const selectPipelineGroupedByStage = createSelector(
  selectApplications,
  (applications) => {
    const groups = new Map<string, Application[]>();
    applications.forEach(a => {
      const stage = a.currentStage;
      if (!groups.has(stage)) groups.set(stage, []);
      groups.get(stage)!.push(a);
    });
    return groups;
  }
);
UI/UX Design Guidelines
Dashboard – Cards with KPIs and sparklines. Recent activity feed.

Pipeline Board – Columns represent stages. Cards show candidate avatar (placeholder), name, days in stage. Empty state messages per stage.

Forms – Reactive forms with validation errors shown inline. Required fields marked. Custom fields rendered dynamically.

Responsiveness – Sidebar collapses on tablets. Tables switch to card layouts on mobile. Pipeline board becomes vertical accordion on small screens.

Loading States – Skeleton screens or progress bars for all async operations.

Error Handling – Toast notifications for transient errors; full‑page error for fatal issues.

Theming & Tenant Branding
Angular Material theming is used with custom palettes.

On login, tenant settings (primary color, logo URL) are fetched from TenantSettings.

CSS variables are dynamically set:

css
document.documentElement.style.setProperty('--primary-color', tenant.primaryColor);
All SCSS files reference these variables.

Tenant logo is displayed in the header.

Security Considerations in Frontend
Token Storage – JWT stored in memory (via NgRx store), not localStorage. Refresh token stored in HttpOnly secure cookie (preferred) or memory.

HTTP Interceptors – Automatically attach Authorization: Bearer <token> to requests. Handle 401 responses by attempting token refresh; on failure, redirect to login.

Permission Directive – UI elements are conditionally rendered based on permissions in the JWT claims. This is a UX optimization, not a security boundary (server enforces actual authorization).

XSS Prevention – Angular’s built‑in sanitization for all interpolated content. Content‑Security‑Policy headers restrict script sources.

CSRF – Not applicable because API uses bearer tokens, not cookies.