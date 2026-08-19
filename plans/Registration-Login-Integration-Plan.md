# TalentFlow — Registration & Login Flow: Gap Analysis & Implementation Plan

> **Document Version:** 1.0
> **Source of Truth:** `docs/Registration-Login-Flows.md`
> **Status:** Final (Review + Implementation Plan)
> **After:** codebase audit of `api/` and `frontend/` (current source)

---

## Table of Contents

1. [Executive Summary](#1-executive-summary)
2. [Step-by-Step Verification vs. the Plan](#2-step-by-step-verification-vs-the-plan)
3. [Confirmed Backend Gaps](#3-confirmed-backend-gaps)
4. [Confirmed Frontend Gaps](#4-confirmed-frontend-gaps)
5. [Complete Implementation / Integration Plan](#5-complete-implementation--integration-plan)
6. [Recommended Implementation Order](#6-recommended-implementation-order)
7. [Validation Checklist](#7-validation-checklist)

---

## 1. Executive Summary

The **backend does NOT implement the plan**. A prior plan file (`.kilo/plans/1786949090876-…-auth-registration-login-implementation-plan.md`) already identified most of the gaps, but **none of its fixes have been applied** — the current source still contains every critical bug it flagged. The **frontend is significantly ahead of the backend**: it calls 9+ endpoints that do not exist, reads JWT claims the backend never issues, and its wizards/routes diverge from the plan (candidate route missing, employer account/role steps missing, multi-tenant not wired).

The bottom line: **registration and login will not work end-to-end today**, and several flows will fail at runtime even after the obvious endpoints are added, because of contract mismatches (payload shapes, routes, claims, and the email-link/verify page mismatch).

**Legend used below:** ✅ works · ⚠️ partial / divergent · ❌ missing / broken

---

## 2. Step-by-Step Verification vs. the Plan

### 2.1 Candidate Registration & Login (`Registration-Login-Flows.md §1`)

| Item | Plan requirement | Backend | Frontend | Verdict |
|---|---|---|---|---|
| 1.1 Step 1 (Account) | First/Last name, Email, Password + confirm, Privacy checkbox | `RegisterCommand` has **no `ConfirmPassword`**; requires `CompanyName`; validator needs ≥6 chars but Identity needs ≥8. Creates user with `IsActive=true` (no Pending state) — `RegisterCommandHandler.cs:59,84-91` | Wizard Step 1 collects these; validates passwords match; **no Privacy-policy checkbox field** | ⚠️ partial |
| Email verification (24h link) | Pending until verified; link valid 24h; then Active | `DataProtection TokenLifespan = 1h` applies to **all** tokens (`Program.cs:192-195`) → email links expire in 1h, not 24h. Link points to **GET `/Auth/ConfirmEmail?userId=&token=`**. `ConfirmEmailHandler` confirms but does **not** flip `IsActive` (already true) | `verify-email` page calls **POST `/Auth/verify-email`** with `{email, token}`, but backend link sends **`userId` + `token`** (no `email`) → **contract mismatch** | ❌ |
| Step 2 Professional | phone, job title, company, years, LinkedIn, portfolio | `CandidateController` has `PATCH /candidate/professional-profile` | Collected in wizard, but **never sent to backend** (see Step 4 flow) | ⚠️ |
| Step 3 Resume & Skills | PDF/DOCX ≤10MB drag-drop, skills chips, cover letter | `POST /candidate/resume`, `PATCH /candidate/skills` exist | Wizard has UI; not wired to endpoints | ⚠️ |
| Step 4 Preferences | location, remote toggle, salary, availability, work auth | `PATCH /candidate/preferences` exists | Wizard has UI; not wired | ⚠️ |
| Flow shape | Step 1 → verify → **then** Steps 2–4 (per plan) | — | Wizard submits **all 4 steps in one** `registerCandidate` call at Step 4 (`registration.service.ts`, `registration.component.ts:55-56`) — Option A, diverges from plan | ❌ |
| 1.2 Login | Valid + verified → dashboard | `LoginCommandHandler` returns JWT + refresh; checks lockout, `IsActive`, `EmailConfirmed` | login page → navigates to `/` | ⚠️ |
| Unverified → "Please verify + Resend" | — | Manual check returns message `"You Must Conference Email"` | Login page shows error only; does not route to `/verify-email` | ❌ |
| Invalid credentials | — | Returns `"Invalid email or password"` | Shows via SweetAlert | ✅ |
| 5 failed → 15 min lockout | — | `AccessFailedAsync` / `IsLockedOutAsync` present | Shows countdown message text only | ⚠️ |
| Deactivated → "deactivated" | — | `IsActive` check returns `"Your account is inactive"` | Shows error | ⚠️ |
| Remember Me 30 days | — | — | No `rememberMe` field wired anywhere | ❌ |
| 1.3 Password reset | email → 1h link → new password → login | `forgot-password`, `reset-password`, 1h token exist | Pages exist and wired | ✅ (mostly) |
| 1.4 Apply via external job link | Step 1 only, then auto to job application | — | No job-link / `jobId` handling, no "Apply via job link" flow | ❌ |
### 2.2 Company / Employer Registration & Login (`§2`)

| Item | Plan | Backend | Frontend | Verdict |
|---|---|---|---|---|
| Step 1 Account | first/last, email, pwd + confirm | `TenantRegisterCommand` has **no `ConfirmPassword`**; route is **`/api/Tenant/Regisret_Tenant` (typo, `TenantController.cs:21`)** | **No Account-step UI component exists** | ❌ |
| Step 2 Role | Founder/HR/Other | not modeled | **No Role-step UI exists** | ❌ |
| Step 3 Company details | name, size, industry, website, LinkedIn, location | in `TenantRegisterCommand` | `company-setup` component (loaded at `/register/employer`) | ⚠️ |
| Step 4 Subscription plan | Free / Pro / Enterprise | `SubscriptionPlan` string stored | `subscription` component | ⚠️ |
| Step 5 Confirmation | verify → provision tenant + default roles + default pipeline + assign TenantAdmin | Handler creates tenant + 5 pipeline stages and assigns `TenantAdmin`; **roles are only global** (from `SeedRole`), not per-tenant; `IsActive=true` immediately; **returns no JWT** (`TenantRegisterCommandHandler.cs:221-231`). Default role name is `TenantAdmin` while frontend checks `Admin` | review component submits via `registerEmployer` → missing endpoint | ❌ |
| 2.2 Employer login + scenarios | tenant active / trial expired / suspended | no trial / suspension logic; no multi-tenant | login page only | ❌ |
| Multi-tenant "Select Workspace" | after login if >1 tenant | **No endpoint**; `User.TenantId` is a single `Guid` (no many-to-many) — multi-tenant not modeled | `select-workspace` shows **hardcoded** data; `getAvailableTenants()` calls missing `/Auth/tenants` | ❌ |
| 2.3 Employer password reset | same as candidate | same endpoint | wired | ✅ |

### 2.3 Invited Team Member Flow (`§3`)

| Item | Plan | Backend | Frontend | Verdict |
|---|---|---|---|---|
| 3.1 Admin invites | invite form → user Invited, 7-day token, email | `InviteTeamMemberCommandHandler` exists (7-day token) but **no controller/endpoint**; requires `TenantId` / `InvitedByUserId` from request (no auth wiring) | No User-Management invite UI integrated | ❌ |
| 3.2 Invitation email | subject/body per spec | Body differs (no admin name, no company name) | — | ⚠️ |
| 3.3 Accept & set password | validate token (valid/expired/used); prefill name/email/role; set pwd → Active → auto-login | `AcceptInvitationCommandHandler` exists but **no endpoint**; **bug:** calls `CreateAsync(user, password)` **and** `AddPasswordAsync(user, password)` → double password set fails (`:90,103`) | `setup-account` reads name/email/role from query params, but invite link sends **only `token`** → prefill empty; calls missing `/Auth/accept-invitation` | ❌ |
| 3.4 Subsequent login | normal login | yes | yes | ✅ |
| 3.5 Edge cases | already-in-system, expired, active-in-tenant, deactivated-before-accept, multi-tenant | Not implemented (no endpoint) | Not handled | ❌ |

### 2.4 User Management (`4`)

| Plan | Backend | Frontend | Verdict |
|---|---|---|---|
| Admin deactivates → logout all sessions, invalidate tokens | `PATCH /users/{id}/disable` exists, but does **not** revoke refresh tokens / active sessions | No admin UI | ❌ |
| Admin changes role → refresh on next request | `PUT /users/{id}` → `UpdateUserCommand` (Auth) is **an empty class**; `User` handler may differ | No UI | ❌ |
| Offboarding | not implemented | — | ❌ |

### 2.5 Summary tables (`§5`)

- Bulk CSV import, SSO: not implemented. SSO is flagged "Future" in the plan (acceptable); CSV is planned but absent.
---

## 3. Confirmed Backend Gaps

### 3.1 Critical Bugs (verified in current source)

1. **`RegisterCommandHandler` returns `IsAuthenticated=false`, no JWT/RefreshToken** (`:84-91`) → frontend `mapAuthResponse` throws on registration.
2. **`TenantRegisterCommandHandler` same** (`:221-231`).
3. **No `email_confirmed` claim in the JWT** (`JWTSErvice.CreateJwtToken` only emits `Sub`, `Email`, `Jti`, `TenantId`, roles). The frontend `auth.service` + `auth-guard` depend on `email_confirmed` → **every user is treated as unverified** → the guard always bounces to `/verify-email`. This is the most flow-breaking issue.
4. `RequireConfirmedEmail=true` (`Program.cs:103`) while the plan wants a manual check.
5. `DataProtection TokenLifespan=1h` for all tokens (`Program.cs:194`) — email verification should be 24h.
6. `TenantController` route typo `Regisret_Tenant`.
7. `AcceptInvitationCommandHandler` double-password bug.
8. `UpdateUserCommand` (Auth) is an empty class.
9. CORS `AllowAnyOrigin` **plus** frontend `withCredentials:true` → browser blocks credentialed requests; also insecure.
10. Refresh route mismatch: frontend calls `/Auth/refresh`; backend exposes `/Auth/RefreshToken`, and its command only has `RefreshToken` (frontend sends `{ Token, RefreshToken }`).

### 3.2 Missing Endpoints (all called by the frontend)

| Endpoint | Method | Status |
|---|---|---|
| `/Auth/register-candidate` | POST | **Missing on backend** (frontend calls it) |
| `/Auth/register-employer` | POST | **Missing** |
| `/Auth/verify-email` | POST | **Missing** (only GET `ConfirmEmail` exists) |
| `/Auth/resend-verification` | POST | **Missing** |
| `/Auth/email-status` | POST | **Missing** |
| `/Auth/tenants` | GET | **Missing** |
| `/Auth/accept-invitation` | POST | **Missing controller** (handler exists) |
| `/Auth/google`, `/Auth/linkedin` | GET | **Missing** |

### 3.3 Contract Mismatches (fail even after endpoints added)

- Email link sends `userId`+`token`; verify page sends `email`+`token`.
- `RegisterCommand` requires `CompanyName` + no `ConfirmPassword`; candidate payload has neither `CompanyName`/`UserName` but has `confirmPassword` + profile fields.
- `TenantRegisterCommand` has no `ConfirmPassword`; employer payload has `roleType`/`otherRoleDetail`/`workspaceName`/`workspaceUrl` that the command does not bind.
- Refresh token route/shape mismatch.
- Role name mismatch: plan/frontend use `Admin`; backend seeds `TenantAdmin`.

---

## 4. Confirmed Frontend Gaps

1. **No route for candidate registration** — `register-choice` navigates to `/register/candidate` but `app.routes.ts` has no such route → falls to `**` → back to `/`.
2. **No Account or Role steps** in the employer flow; `/register/employer` loads `company-setup` (Company Details) directly.
3. **Broken navigation:** `workspace.goBack()` → `/register/company-setup` (does not exist); `company-setup.goBack()` → `/register/employer` (itself).
4. **Guards not applied** — `authGuard`/`adminGuard` exist but no route uses `canActivate`.
5. **`select-workspace` is hardcoded**, not driven by `getAvailableTenants()`.
6. **`setup-account` prefill empty** (invite link carries only `token`); no expired/used state UI.
7. **Candidate wizard submits everything at Step 4** (Option A) — needs splitting per plan (Step 1 → verify → Steps 2–4).
8. **Login** does not branch on unverified / multi-tenant / lockout countdown; social buttons hit missing endpoints.
9. **Remember Me** not implemented.
10. **NgRx effects** call missing endpoints on login (`loadTenants$`) → 404 on every login.
---

## 5. Complete Implementation / Integration Plan

Recommended order — **backend first** (so the frontend has a target), then **frontend integration**, then **edge cases**.

### Phase 1 — Backend core auth fixes (foundation)

1. **`Program.cs`**
   - Set `options.User.RequireConfirmedEmail = false` (rely on manual check in `LoginCommand`).
   - Add two token providers with distinct lifespans: `EmailConfirmationTokenProvider` **24h**, keep DataProtection **1h** for password reset. Register `options.Tokens.EmailConfirmationTokenProvider`.
   - Replace CORS `AllowAll` with a specific-origin policy + `.AllowCredentials()` (required for `withCredentials:true`), add frontend origin config.
2. **JWT service (`JWTSErvice.CreateJwtToken`)** — add `email_confirmed` (`user.EmailConfirmed`) claim, plus `name` / `unique_name`, keep `sub`, `tenantid`, roles; add `IsActive`/status claim if needed.
3. **`RegisterCommandHandler`** — add `ConfirmPassword` match, map email→`UserName` when absent, drop `CompanyName` for candidate, set `IsActive=false` (Pending) + `EmailConfirmed=false`, generate + return **JWT & RefreshToken** (immediate), set `email_confirmed=false`.
4. **`TenantRegisterCommandHandler`** — add `ConfirmPassword`, `IsActive=false`, return **JWT & RefreshToken**, assign `TenantAdmin`, and (for plan fidelity) provision per-tenant default roles (Admin, Recruiter, Hiring Manager, Interviewer) + default "Standard Hiring" pipeline; align naming with frontend/plan.
5. **`AcceptInvitationCommandHandler`** — remove redundant `AddPasswordAsync`; mark invitation `IsUsed`/`IsAccepted`; handle the *existing-user* case by adding the user to the tenant; return JWT+refresh with `email_confirmed=true`.
6. **Refresh token endpoint** — change route to `[HttpPost("refresh")]` (or align frontend to `/RefreshToken`); align command to accept `{ Token, RefreshToken }`.
7. **`TenantController`** — fix route typo to `Register_Tenant` (or alias); move employer registration under `/Auth/register-employer` for consistency.

### Phase 2: Backend new endpoints (frontend already calls these)

Add to `AuthController`:

8. `POST /Auth/register-candidate` → new `RegisterCandidateCommand` (or map into `RegisterCommand`), return JWT+refresh.
9. `POST /Auth/register-employer` → maps to `TenantRegisterCommand`, return JWT+refresh.
10. `POST /Auth/verify-email` — accept `{ email, token }` (support `userId` too) → `ConfirmEmailAsync`; on success set `IsActive=true` (Pending → Active). Keep GET `ConfirmEmail` for email-link clicks.
11. `POST /Auth/resend-verification` — `{ email }` → regenerate 24h token, resend email.
12. `POST /Auth/email-status` — `{ email }` → `{ isRegistered, isConfirmed }`.
13. `GET /Auth/tenants` — return current user's tenants + role (workspace picker). **Requires a `UserTenant` model** (see Phase 4).
14. `POST /Auth/accept-invitation` → `AcceptInvitationCommand`.
15. `GET /Auth/invitation-info?token=` → `{ firstName, lastName, email, role, companyName }` for `setup-account` prefill + expiry/used detection.
16. Team-member admin endpoints: `POST /Tenant/invite`, `POST /Tenant/invite/resend`, invite list; wire `TenantId`/`InvitedByUserId` from the authenticated user's claims/tenant.
### Phase 3: Candidate & Employer end-to-end integration

17. **Candidate flow split (per plan §1):** Step 1 → `register-candidate` → redirect `/verify-email` → after verification, Steps 2–4 post to candidate endpoints (`PATCH /candidate/professional-profile`, `POST /candidate/resume`, `PATCH /candidate/skills`, `PATCH /candidate/preferences`) using the returned token.
18. **Employer flow:** add Account (Step 1) + Role (Step 2) components/routes; keep Company → Workspace → Subscription → Review; submit via `register-employer`; fix `goBack` targets and the `/register/company-setup` route.
19. **Login routing:** on success, if `availableTenants.length > 1` → `/select-workspace`; if unverified → `/verify-email` with resend; show lockout countdown; honor Remember Me (30-day refresh cookie vs session cookie).

### Phase 4: Data model / multi-tenancy (required for workspace picker)

20. Add `UserTenant` junction table (many-to-many): one user belongs to multiple tenants, each with a role; keep `User.TenantId` as "current/default" or derive from JWT `tenant_id` claim. Add migration.
21. Admin user management: role-change (`PUT /users/{id}/role`) updating claims on next refresh; deactivate endpoint that **revokes all refresh tokens / invalidates session** (security-stamp bump).
22. Invitation edge cases: expired (resend), used, duplicate-email-in-system (add to tenant + notify), already-active-in-tenant (error), deactivated-before-accept (invalidate token), multi-tenant invitations (independent).

### Phase 5: Frontend wiring & polish

23. `app.routes.ts`: add `register/candidate`; apply `canActivate: [authGuard]` (and `adminGuard` where needed) to protected routes.
24. `register-choice`: fix candidate navigation.
25. `verify-email`: support both `email` + `token` POST and the `userId` + `token` email link; wire resend.
26. `setup-account`: call `invitation-info` to prefill read-only name/email/role; show clear expired/used/invalid states.
27. `select-workspace`: drive from `getAvailableTenants()` (NgRx state), not hardcoded data; `selectTenant` stores the choice (cookie/local).
28. `auth.service` / effects: align all URLs to new backend routes; read `email_confirmed`/tenant from the JWT.
29. Wire social login buttons to `GET /Auth/google` / `/Auth/linkedin` + callbacks (defer if SSO out of scope).
30. Fix CORS / `withCredentials` pairing on both sides.

### Phase 6: Edge cases & hardening

31. Remember Me (30 days) vs session cookie; interceptor token expiry handling.
32. Password policy alignment (frontend min 6 vs backend/plan min 8) — standardize on **8 + upper + lower + digit** everywhere.
33. Rate limiting on login/forgot-password; lockout countdown string; deactivated message; tenant suspended/trial-ended messages.
34. Privacy-policy checkbox (plan Step 1) — add field + consent record.
35. Normalize role names (`Admin` vs `TenantAdmin`) across seed, JWT claims, guards, and `admin-guard`.
---

## 6. Recommended Implementation Order (priority sequence)

1. **Phase 1 (items 1–7):** JWT `email_confirmed` claim + `RequireConfirmedEmail=false` + token-provider lifespans + CORS. *(Unblocks guard deadlock and 24h email.)*
2. **Phase 1 (items 3–5) + Phase 2 (items 8–9, 14):** registration returns tokens; add `register-candidate` / `register-employer` / `accept-invitation`; fix `AcceptInvitationCommandHandler`.
3. **Phase 2 (items 10–12):** verify-email / resend / email-status endpoints + `ConfirmEmail` `IsActive` flip.
4. **Phase 5 (items 25–26):** verify-email + setup-account prefill wired to new endpoints.
5. **Phase 3 (items 17–18):** split candidate flow + add employer Account/Role steps; fix routes.
6. **Phase 4 (item 20) + Phase 2 (item 13):** multi-tenant model + `/Auth/tenants`; wire `select-workspace`.
7. **Phase 5 (items 23, 27–30):** guard wiring, login branching, social buttons, CORS.
8. **Phase 4 (items 21–22) + Phase 6:** admin user management, invitation edge cases, hardening.

Validate at each step with the §7 checklist below.

---

## 7. Validation Checklist (acceptance criteria)

- [ ] Candidate: Step 1 → 24h email → verify → Active → Steps 2–4 persist to `/candidate/*`.
- [ ] Employer: Account → Role → Company → Plan → Review → verify → tenant + default roles/pipeline → onboarding.
- [ ] Login: verified+valid → dashboard; unverified → "verify + resend"; invalid → error; 5 fails → 15-min lockout; deactivated → deactivated message.
- [ ] Multi-tenant: login → `/select-workspace` (driven by `/Auth/tenants`).
- [ ] Invitation: admin invites → 7-day email → prefill → set password → auto-login; expired / used / already-member / deactivated handled.
- [ ] Admin: deactivate → immediate logout (tokens invalidated); role change → takes effect on next request.
- [ ] Password reset: email → 1h link → new password → login.
- [ ] JWT carries `email_confirmed`; authGuard routes unverified users correctly; no CORS errors with `withCredentials`.
- [ ] Social login buttons resolve to working endpoints (or are intentionally disabled if SSO is out of scope).

---

*End of document.*