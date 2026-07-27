# Plan: Complete Auth Flow Implementation — All Pages, State, and Services

## Context
- `plans/Registration-Login-Flows.md` defines 5 registration cases and login scenarios.
- `Design/` contains HTML mockups for every auth page. Current code only has basic placeholder pages.
- Current `styles.scss` has competing pink + partial MD3 tokens. Needs full replacement.
- Current auth uses hybrid approach: NgRx store + direct service calls with signals.
- `npm run build` passes on current code.

## Key Design Files to Implement
1. `Design/talentflow_enterprise_recruitment_platform/code.html` — Home page
2. `Design/login_talentflow/code.html` — Login split-screen
3. `Design/candidate_registration_step_1_talentflow/code.html` — Candidate Step 1 (Basic Info)
4. `Design/candidate_registration_step_2_talentflow/code.html` — Candidate Step 2 (Professional Profile)
5. `Design/candidate_registration_step_3_talentflow/code.html` — Candidate Step 3 (Resume & Skills)
6. `Design/candidate_registration_step_4_talentflow/code.html` — Candidate Step 4 (Preferences)
7. `Design/register_talentflow/code.html` — Employer/Company registration (single-page simplified)
8. `Design/employer_registration_company_details_talentflow/code.html` — Employer Step 2 (Company Details)
9. `Design/select_subscription_plan_talentflow/code.html` — Employer Step 4 (Subscription Plan)
10. `Design/set_up_your_account_talentflow/code.html` — Invited member setup
11. `Design/select_workspace_talentflow/code.html` — Multi-tenant workspace picker
12. `Design/forgot_password_talentflow/code.html` — Forgot password
13. `Design/verify_email_talentflow/code.html` — Verify email
14. `Design/reset-password` — Reset password (follow forgot-password pattern)

## Decisions

### 1. Theme & Global Styles
- **Fully replace** `frontend/src/styles.scss` with MD3 tokens from `Design/talentflow_enterprise/DESIGN.md`.
- Remove ALL pink/social theme variables.
- Keep legacy aliases (`--foreground`, `--card`, `--border`) mapped to MD3 for backward compatibility.
- Global auth classes (`.auth-container`, `.auth-card`, `.btn-primary`, `.form-group`, `.alert`, `.subtitle`, `.divider`, `.auth-link`, `.spinner`) remain in `styles.scss`.
- Page-specific classes live in component SCSS files.

### 2. State Management Architecture
**Current state**: Hybrid NgRx + signals. Components call `AuthService` directly with signals, but NgRx store also exists.

**Decision**: Keep the hybrid approach but align it:
- `AuthService` methods return `Observable` and update both cookies AND NgRx store via dispatch.
- Components can use either NgRx selectors OR service signals. For auth pages, continue using service signals for simplicity.
- Add new state properties to `AuthState`:
  - `registrationStep`: number (for multi-step candidate registration)
  - `registrationData`: object (to persist data across steps)
  - `availableTenants`: array (for workspace picker)
  - `isInvited`: boolean (for setup-account flow)
  - `invitationToken`: string | null

### 3. Auth Service Updates
Add these methods to `AuthService`:
- `registerCandidate(request)` — calls `/Auth/register-candidate`
- `registerEmployer(request)` — calls `/Auth/register-employer`
- `getAvailableTenants()` — calls `/Auth/tenants`
- `acceptInvitation(token, password)` — calls `/Auth/accept-invitation`
- `resendVerificationEmail()` — already exists
- `checkEmailStatus(email)` — calls `/Auth/email-status`

Update existing methods to dispatch NgRx actions:
- `login()` → dispatch `AuthActions.login()` then `AuthActions.loginSuccess()` on success
- `register()` → dispatch `AuthActions.register()` then `AuthActions.registerSuccess()`
- `logout()` → dispatch `AuthActions.logout()`

### 4. Route Structure
```
'' → HomeComponent (lazy)
'login' → LoginComponent (lazy)
'register' → RegisterComponent (employer step 1, lazy)
'register/company-details' → CompanyDetailsComponent (lazy)
'register/select-plan' → SelectPlanComponent (lazy)
'register/candidate' → CandidateRegistrationComponent (lazy, parent for steps)
  'register/candidate/step1' → CandidateStep1Component
  'register/candidate/step2' → CandidateStep2Component
  'register/candidate/step3' → CandidateStep3Component
  'register/candidate/step4' → CandidateStep4Component
'setup-account' → SetupAccountComponent (lazy)
'select-workspace' → SelectWorkspaceComponent (lazy)
'verify-email' → VerifyEmailComponent (lazy)
'forgot-password' → ForgotPasswordComponent (lazy)
'reset-password' → ResetPasswordComponent (lazy)
'**' → redirect to ''
```

### 5. Candidate Registration Flow (Steps 1-4)
**Parent component**: `CandidateRegistrationComponent` — holds shared stepper UI and `registrationData` in service/state.

**Step 1** (`candidate-registration-step1`):
- Design: `Design/candidate_registration_step_1_talentflow/code.html`
- Fields: First Name, Last Name (grid), Email, Password, Confirm Password, Privacy Policy checkbox, verification notice
- Validation: email format, password 8+ chars with uppercase/lowercase/digit, passwords match
- On submit: dispatch `registerCandidate()`, navigate to `/verify-email`

**Step 2** (`candidate-registration-step2`):
- Design: `Design/candidate_registration_step_2_talentflow/code.html`
- Fields: Phone (with country picker), Current Job Title, Current Company, Years of Experience, LinkedIn URL, Portfolio/GitHub URL
- Layout: Left branding panel (desktop) + right form + stepper at top

**Step 3** (`candidate-registration-step3`):
- Design: `Design/candidate_registration_step_3_talentflow/code.html`
- Fields: Resume upload (drag-drop, PDF/DOCX, 10MB), Skills (chips/tags), Cover Letter (textarea)
- Stepper at top showing progress

**Step 4** (`candidate-registration-step4`):
- Design: `Design/candidate_registration_step_4_talentflow/code.html`
- Fields: Preferred Location, Remote Only toggle, Salary Expectation (min/max + currency), Available From (date), Work Authorization dropdown
- Sidebar progress indicator on desktop
- Success overlay on submit

### 6. Employer Registration Flow
**Step 1 — Account** (`/register`):
- Design: `Design/register_talentflow/code.html` — single-page simplified
- Fields: Full Name, Work Email, Company Name, Password + Confirm Password, Privacy Policy checkbox
- Password strength meter (4 bars)
- On submit: save to state, navigate to `/register/company-details`

**Step 2 — Company Details** (`/register/company-details`):
- Design: `Design/employer_registration_company_details_talentflow/code.html`
- Fields: Company Name, Company Size (dropdown), Industry (dropdown), Website URL, LinkedIn URL, Office Location
- Layout: Glass-panel container with decorative background blobs, progress indicators at top
- On submit: save to state, navigate to `/register/select-plan`

**Step 3 — Select Subscription Plan** (`/register/select-plan`):
- Design: `Design/select_subscription_plan_talentflow/code.html`
- Layout: Sticky header with brand + Login/Contact Sales links, centered pricing cards grid
- 3 pricing tiers:
  - **Free** ($0/month): 3 active jobs, basic candidate management, standard analytics
  - **Pro** ($49/month, recommended): unlimited jobs, advanced pipelines, custom analytics, automated emails, team collaboration
  - **Enterprise** (Custom): everything in Pro + SSO, dedicated account manager, custom API, SLA
- Each card has feature checklist with check/close icons and a CTA button
- On select: save plan to state, navigate to `/verify-email`

**Step 4 — Confirmation** (future): After email verification, tenant is provisioned with default roles and pipeline template.

**Note**: The current simplified employer registration can be implemented as a single-page first, then expanded to multi-step.

### 7. Login Page
- Design: `Design/login_talentflow/code.html`
- Split-screen: left hero image + quote (desktop only), right form
- Form: Corporate Email, Password (with visibility toggle), Remember me, Forgot password link
- Primary CTA: "Sign in"
- SSO: Google + LinkedIn buttons
- Footer: "Contact IT Support" link
- TS: Add `loginWithLinkedIn()` method to template. Replace `(dblclick)` password toggle with button `(click)`.

### 8. Forgot Password
- Design: `Design/forgot_password_talentflow/code.html`
- Minimal centered card, `lock_reset` icon
- Email input + "Send Reset Link" button
- Hidden success state with checkmark
- "Back to Login" footer link
- TS: On success, show success div (already has `isSubmitted` signal)

### 9. Verify Email
- Design: `Design/verify_email_talentflow/code.html`
- Branding header bar (`all_inclusive` icon + TalentFlow)
- Circular illustration
- "Check your inbox" heading
- Two buttons: "Resend Email" + "Change Email"
- Support footer
- TS: Add `changeEmail()` method. Keep `resendVerification()` and `verifyEmail()`.

### 10. Reset Password
- Design: Follow `forgot_password` pattern but with two password fields
- Fields: New Password, Confirm Password (both with visibility toggles)
- "Reset Password" CTA
- "Back to Login" footer
- TS: Add `showConfirmPassword` signal. Update `togglePassword()` to handle both fields.

### 11. Set Up Your Account (Invited Member)
- Design: `Design/set_up_your_account_talentflow/code.html`
- Two-panel layout: left branding image with overlay, right form
- Read-only user info card (name, email, role pill)
- Create Password + Confirm Password with visibility toggle
- Password requirements checklist (8+ chars, uppercase, special char)
- "Complete Setup" CTA
- Data from route query params: `email`, `firstName`, `lastName`, `role`, `token`
- TS: On submit, call `acceptInvitation()`, then navigate to `/`

### 12. Select Workspace (Multi-Tenant)
- Design: `Design/select_workspace_talentflow/code.html`
- Sticky header: brand + search + avatar
- Scrollable workspace cards with role badges and job counts
- "Create New Workspace" dashed card
- Logout footer
- TS: Mock data for 3 workspaces. On select, store `selectedTenantId` in state/service and navigate to `/`.

### 13. Home Page
- Design: `Design/talentflow_enterprise_recruitment_platform/code.html`
- Sticky navbar with brand + nav links + Login/Sign Up buttons
- Hero: dot-grid background, badge pill, H1, subtitle, 2 CTAs, dashboard mockup
- Trusted By: 5 company logos
- Bento Grid: 3 feature cards (AI Sourcing, Collaborative Pipeline, Data-Backed Decisions)
- Built for Teams: two-column with glassmorphism mockup
- CTA Section: primary container card with "Get Started Now"
- Footer: brand, copyright, links, social icons

### 14. NgRx Store Updates
**New AuthState fields**:
```typescript
export interface AuthState {
  user: { id: string; email: string; username: string; roles: string[]; emailConfirmed: boolean } | null;
  token: string | null;
  isAuthenticated: boolean;
  loading: boolean;
  error: string | null;
  registrationStep: number;
  registrationData: any;
  availableTenants: Array<{ id: string; name: string; logo: string; role: string; jobCount: number }>;
  selectedTenantId: string | null;
  isInvited: boolean;
  invitationToken: string | null;
}
```

**New Actions**:
- `RegisterCandidate` / `RegisterCandidateSuccess` / `RegisterCandidateFailure`
- `RegisterEmployer` / `RegisterEmployerSuccess` / `RegisterEmployerFailure`
- `SetRegistrationStep` / `UpdateRegistrationData`
- `SetAvailableTenants` / `SelectTenant`
- `SetInvitation` / `AcceptInvitation` / `AcceptInvitationSuccess`
- `ResetRegistration`

**New Effects**:
- `registerCandidate$` — calls `AuthService.registerCandidate()`
- `registerEmployer$` — calls `AuthService.registerEmployer()`
- `loadTenants$` — calls `AuthService.getAvailableTenants()` after login
- `acceptInvitation$` — calls `AuthService.acceptInvitation()`

### 16. Guard Updates
**AuthGuard** (`auth-guard.ts`):
- After checking `isAuthenticated` and `isEmailConfirmed`:
  - If `selectedTenantId` is null AND `availableTenants.length > 1` → navigate to `/select-workspace`
  - If `isInvited` is true → navigate to `/setup-account`
  - Otherwise → allow access to `/`

### 17. Registration Flow Summary (per flows doc)
| Case | Route | Pages |
|------|-------|-------|
| Candidate self-signup | `/register/candidate` | Step1 → Step2 → Step3 → Step4 → Verify Email |
| Candidate via job link | `/register/candidate?jobId=X` | Step1 only (then auto-redirect to job application) |
| Founder/HR self-signup | `/register` | Account → Company Details → Select Plan → Verify Email → Setup |
| Admin invites member | `/setup-account?token=X&email=Y` | Set password → Dashboard |
| Existing user added to tenant | After login | Workspace picker if multi-tenant |
| Bulk CSV import | `/setup-account?token=X` | Set temporary password |
| SSO (future) | `/auth/sso` | Redirect to IdP |

## Implementation Order
1. `styles.scss` full MD3 replacement
2. Auth models expansion (`auth.model.ts`)
3. Auth service method additions + NgRx dispatch integration
4. NgRx actions, reducer, effects, selectors updates
5. Auth guard multi-tenant/invite logic
6. Home page component + route
7. Login page split-screen redesign
8. Candidate registration parent + Step1-4 components + routes
9. Employer register Step 1 (single-page) redesign
10. Employer register Step 2 (company details) + route
11. Select Subscription Plan page + route
12. Forgot Password redesign
13. Verify Email redesign
14. Reset Password redesign
15. Set Up Your Account new page + route
16. Select Workspace new page + route
17. Build verification

## Validation
- `npm run build` passes with zero errors
- All lazy-loaded routes resolve
- Auth pages match Design/ files exactly (colors, layout, typography)
- Colors: `#24389c` primary, `#3f51b5` primary-container, `#fbf8ff` background
- No inline styles in any HTML templates
- All registration flows from `Registration-Login-Flows.md` have corresponding UI

## Open Questions
- None.
