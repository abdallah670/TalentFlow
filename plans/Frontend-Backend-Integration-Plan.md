# TalentFlow — Frontend ↔ Backend Integration Plan (Registration & Login)

> **Document Version:** 1.0
> **Sources of Truth:** `plans/Registration-Login-Flows.md` (flows), `descripe endpoints/TalentFlow-API-Endpoints-EN.md` (API contract), and a source audit of `api/` + `frontend/` (current code).
> **Status:** Final — Ready for implementation
> **Date:** September 2026

---

## Table of Contents

1. [Executive Summary](#1-executive-summary)
2. [Current-State Audit (Code-Verified)](#2-current-state-audit-code-verified)
3. [Endpoint ↔ Flow Mapping Matrix](#3-endpoint--flow-mapping-matrix)
4. [Payload Field Mapping (Registration)](#4-payload-field-mapping-registration)
5. [Phase 1 — Blocker Fixes](#5-phase-1--blocker-fixes)
6. [Phase 2 — State, Guards & Routing](#6-phase-2--state-guards--routing)
7. [Phase 3 — UX, Error Mapping & Edge Cases](#7-phase-3--ux-error-mapping--edge-cases)
8. [Phase 4 — Deferred Scope](#8-phase-4--deferred-scope)
9. [Implementation Order](#9-implementation-order)
10. [Validation Checklist](#10-validation-checklist)

---

## 1. Executive Summary

The **backend has caught up** with the original gap analysis: every endpoint documented in
`descripe endpoints/TalentFlow-API-Endpoints-EN.md` exists in `AuthController.cs`, `CandidateController.cs`,
`TenantController.cs`, and `UsersController.cs`. Email verification, multi-tenant login, invitations, and
user management are all implemented server-side.

The remaining work is **almost entirely frontend contract alignment**. Registration and login will NOT work
end-to-end today because:

- The frontend calls `POST /Auth/register-candidate`, which **does not exist** on the backend.
- The candidate wizard submits all 4 steps in one request; the backend only accepts Step-1 fields at
  `/Auth/register` and expects Steps 2–4 via `/candidate/*` endpoints **after email verification**.
- The employer registration payload field names don't match the backend contract, and no `slug` is generated.
- `login()` never branches on the backend's 3 response shapes (success / `requiresTenantSelection` / failure).
- Role names mismatch (`Admin` on the frontend vs `TenantAdmin` in the JWT).

**Legend:** 🔴 blocker · 🟠 high · 🟡 medium · ✅ done

---

## 2. Current-State Audit (Code-Verified)

| # | Item | Frontend | Backend | Severity |
|---|------|----------|---------|----------|
| 1 | Candidate registration endpoint | Calls `POST /Auth/register-candidate` (`auth.service.ts:73`) | Only `POST /Auth/register` exists — body: `{firstName, lastName, userName, email, password, confirmPassword}` | 🔴 |
| 2 | Candidate wizard Steps 2–4 | All 4 steps sent in one body via `registerCandidate` (`registration.service.ts:submit`) | Steps 2–4 belong to `PATCH /candidate/professional-profile`, `POST /candidate/resume`, `PATCH /candidate/skills`, `PATCH /candidate/preferences` — callable only **after email confirmation** | 🔴 |
| 3 | Employer registration payload | Sends `companyName, websiteUrl, linkedinUrl, selectedPlan, roleType` (`employer-registration.service.ts`) | Expects `tenantName, slug, subscriptionPlan, companySize, industry, website, linkedIn, officeLocation`; `slug` is required and unique — frontend never generates it | 🔴 |
| 4 | Login response handling | `login()` always calls `handleAuthentication()` (`auth.service.ts:46–51`) | `/Auth/login` returns **3 shapes**: (a) success, (b) `requiresTenantSelection: true` + `availableTenants` + 10-min temp token, (c) failure messages (`isAuthenticated: false` + `message`) | 🔴 |
| 5 | Guard ↔ service state sync | `auth-guard.ts` reads NgRx store (`selectAuthState`); `AuthService` writes to **signals** only | — | 🔴 (deadlock risk) |
| 6 | Role names | `isAdmin` checks `'Admin'`; `admin-guard` likewise | JWT role claim is `TenantAdmin` | 🟠 |
| 7 | Token lifetime | Cookies set for 7 days, no Remember Me distinction | Plan requires: Remember Me = 30 days, otherwise session cookie | 🟡 |
| 8 | Workspace picker | `select-workspace` page exists but is not driven by `availableTenants` / `/Auth/select-tenant` | `/Auth/select-tenant` body: `{userId, tenantId}` with the **temporary** token | 🟠 |
| 9 | Verify-email page | Exists; sends `POST /Auth/verify-email` `{email, token}` | Email link carries `?userId=&token=` (no email) — page must read `email` from query/store and `token` from URL; `POST /Auth/resend-verification` available | 🟡 |
| 10 | Setup-account page | Exists | Must call `GET /Auth/invitation-info?token=` (prefill) then `POST /Tenant/accept-invitation` `{token, password, confirmPassword}` (auto-login response) | 🟡 (verify wiring) |
| 11 | Error/edge handling | Generic error toasts | Requires: 429 rate-limit message, lockout countdown, unverified-email → resend flow, invitation `status` = `valid\|expired\|used\|invalid` screens | 🟡 |
| 12 | Base URL | `environment.baseUrl = http://localhost:5000/api` | `launchSettings.json` = `http://localhost:5000` ✅ — API doc's `https://localhost:44358/api` is **outdated** | ✅ |
| 13 | JWT claim parsing | Reads `email_confirmed`, standard role/nameidentifier claims | Backend issues exactly these claims (`email_confirmed`, `TenantId`, `role`, `sub`) | ✅ |
| 14 | Privacy policy checkbox | Not present in wizard Step 1 | Backend rejects registration without `acceptedPrivacyPolicy: true` (API doc §9.6) | 🟠 |

---

## 3. Endpoint ↔ Flow Mapping Matrix

### 3.1 Candidate (Flows doc §1)

| Flow Step | API Endpoint | Auth | Frontend Target |
|-----------|--------------|------|-----------------|
| Step 1 — Account creation | `POST /Auth/register` | Anonymous | `registration.component.ts` Step 1 → call immediately, **stop wizard**, store temp token |
| Email verification | `POST /Auth/verify-email` `{email, token}` | Anonymous | `verify-email` page (token from URL, email from query param) |
| Resend verification (24h link) | `POST /Auth/resend-verification` `{email}` | Anonymous | "Resend" button on verify-email page and on unverified-login response |
| Check email status (pre-check) | `POST /Auth/email-status` `{email}` | Anonymous | Optional UX pre-check before login/registration |
| Step 2 — Professional profile | `PATCH /candidate/professional-profile` | Bearer | Wizard Step 2 (post-verification) |
| Step 3 — Resume | `POST /candidate/resume` (`multipart/form-data`: `UserId`, `File`) | Bearer | Wizard Step 3 |
| Step 3 — Skills | `PATCH /candidate/skills` `{userId, skillIds[]}` | Bearer | Wizard Step 3 (map skill names → ids via `SkillController`) |
| Step 4 — Preferences | `PATCH /candidate/preferences` | Bearer | Wizard Step 4 |
| Login | `POST /Auth/login` | Anonymous | `login` page — 3 response shapes (§4.3) |
| Password reset | `POST /Auth/forgot-password` → `POST /Auth/reset-password` | Anonymous | `forgot-password` / `reset-password` pages |
| Refresh session | `POST /Auth/refresh` | — | `token-interceptor` on 401 |
| Logout | `POST /Auth/logout` `{refreshToken}` | Bearer | Header/logout action |

### 3.2 Employer (Flows doc §2)

| Flow Step | API Endpoint | Auth | Frontend Target |
|-----------|--------------|------|-----------------|
| Steps 1–4 — Company + admin signup | `POST /Auth/register-employer` | Anonymous | `employer-registration.service.ts:submit` (remapped payload, §4.2) |
| Email verification | Same as candidate (`verify-email` / `resend-verification`) | Anonymous | Same pages |
| Login (single tenant) | `POST /Auth/login` | Anonymous | `login` page |
| Login (multi-tenant) | `POST /Auth/login` → case (b) → `POST /Auth/select-tenant` `{userId, tenantId}` | Temp token | `select-workspace` page |
| Tenant data | `GET /Tenant/current` | Bearer | Dashboard / workspace header |
| Invite member | `POST /Tenant/invite-member` — **`role` is a NUMBER** (Roles enum) | TenantAdmin | Settings → User Management |
| Resend invitation | `POST /Tenant/invite/resend` `{email}` | TenantAdmin | User Management |
| Accept invitation (info) | `GET /Auth/invitation-info?token=` | Anonymous | `setup-account` page prefill (read-only name/email/role/company) |
| Accept invitation (submit) | `POST /Tenant/accept-invitation` `{token, password, confirmPassword}` | Anonymous | `setup-account` page → auto-login (response = full JWT) |

### 3.3 User Management (Flows doc §4) — all `TenantAdmin`

| Flow Step | API Endpoint | Notes |
|-----------|--------------|-------|
| List users | `GET /Users` | — |
| Edit name/email | `PUT /Users/{id}` | Role NOT included |
| Change role | `PUT /Users/{id}/role` — **`role` is a STRING** here (opposite of invite-member) | Backend revokes the user's refresh tokens → forced re-login |
| Deactivate user | `PATCH /Users/{id}/disable` | Immediate forced logout across all devices |
| Create user directly | `POST /Users/Create_user` | Admin-created, no email invitation |

---

## 4. Payload Field Mapping (Registration)

### 4.1 Candidate — frontend wizard → backend

**Step 1 only** goes to `POST /Auth/register`:

| Frontend field | Backend field | Note |
|----------------|---------------|------|
| `firstName` | `firstName` | |
| `lastName` | `lastName` | |
| — | `userName` | **Must be generated** (e.g. from email prefix) or collected in UI |
| `email` | `email` | |
| `password` | `password` | ≥8 chars + upper + lower + digit |
| `confirmPassword` | `confirmPassword` | |
| *(new)* | `acceptedPrivacyPolicy` | Must be `true` or backend rejects |

Steps 2–4 go to `/candidate/*` endpoints **after email verification** (see §3.1). `coverLetter` has no
dedicated endpoint yet → hold in local state until the application flow exists.

### 4.2 Employer — frontend wizard → `/Auth/register-employer`

| Frontend field (current) | Backend field | Action |
|--------------------------|---------------|--------|
| `firstName` | `firstName` | keep |
| `lastName` | `lastName` | keep |
| `email` | `email` | keep |
| `password` / `confirmPassword` | `password` / `confirmPassword` | keep |
| `companyName` | `tenantName` | rename |
| — | `slug` | **generate** (slugify `tenantName`, ensure uniqueness — handle `"Slug already exists."` by suggesting a variant) |
| `selectedPlan` | `subscriptionPlan` | rename ("Free" / "Pro" / "Enterprise") |
| `companySize` | `companySize` | keep (enum string: "1–10", "11–50", …) |
| `industry` | `industry` | keep |
| `websiteUrl` | `website` | rename |
| `linkedinUrl` | `linkedIn` | rename |
| `officeLocation` | `officeLocation` | keep |
| `roleType`, `otherRoleDetail` | — | No backend field — keep in local state (informational only) |
| `workspaceName`, `workspaceUrl` | — | Drop (superseded by `tenantName`/`slug`) |

### 4.3 Login — the 3 response shapes (`POST /Auth/login`)

| Shape | Detection | Frontend action |
|-------|-----------|-----------------|
| (a) Success | `isAuthenticated: true` | Store token + refreshToken; if `email_confirmed=false` in JWT → `/verify-email`; else → dashboard (respect `currentStep` / `onboardingCompleted`) |
| (b) Multi-tenant | `requiresTenantSelection: true` | Store 10-min temp token + `availableTenants`; navigate to `/select-workspace`; on pick call `/Auth/select-tenant` `{userId, tenantId}` |
| (c) Failure | `isAuthenticated: false` + `message` | Map `message` to UI (§7 table); handle HTTP 429 specially |

---

## 5. Phase 1 — Blocker Fixes

1. **Candidate registration split**
   - Replace `registerCandidate()` URL with `POST /Auth/register`; add `userName` (generate from email prefix, e.g. `john.doe`).
   - Add `acceptedPrivacyPolicy: true` + a privacy-policy checkbox in wizard Step 1 (Flows doc §1.1).
   - After success: persist temp token, navigate to `/verify-email?email=…`. Do NOT run Steps 2–4 yet.
   - After verification succeeds: resume the wizard and submit Steps 2–4 one-by-one to the `/candidate/*` endpoints using the real token.

2. **Employer payload remap** — apply §4.2 in `employer-registration.service.ts:submit()`; add slug generation (`tenantName.toLowerCase().replace(/[^a-z0-9]+/g,'-')`) and a slug-collision retry prompt.

3. **Login response branching** — rework `AuthService.login()`:
   - Parse `isAuthenticated` / `requiresTenantSelection` / `message`.
   - On (b): stash `availableTenants` + temp token (memory or sessionStorage — it's 10-min); route to `/select-workspace`.
   - On (c): throw a typed error so the login page renders the mapped message (§7).

4. **Workspace picker wiring** — `select-workspace` renders from the stored `availableTenants` (company name + role per entry, per Flows doc §2.2); submit calls `selectTenant()` → `POST /Auth/select-tenant` `{userId, tenantId}`; final JWT stored as a normal login.

---

## 6. Phase 2 — State, Guards & Routing

5. **Unify state**: NgRx effects must mirror every `AuthService` signal write into the store (or migrate the guard to read the service signals). Verify `selectAuthState.user.emailConfirmed` is populated from the JWT `email_confirmed` claim.
6. **Role alignment**: change `isAdmin` / `admin-guard` / role constants from `'Admin'` to `'TenantAdmin'` (create a single `ROLES` constants file; also map the invite-member numeric enum → names: 1=TenantAdmin, 2=Recruiter, 3=Hiring Manager, 4=Interviewer).
7. **Remember Me**: login page passes the flag; on success store cookies for **30 days** when checked, otherwise session-only cookie. Backend enforces lifetime via refresh tokens; Remember Me is client-side cookie lifetime.
8. **Route wiring**: `/verify-email`, `/setup-account`, `/select-workspace`, `/forgot-password`, `/reset-password` are public; all other routes behind `authGuard` + `adminGuard`.

---

## 7. Phase 3 — UX, Error Mapping & Edge Cases

Backend message → UI mapping table (single `AUTH_ERRORS` map in a shared file):

| Backend response / HTTP | UI behavior |
|--------------------------|-------------|
| `"Invalid email or password."` | Inline error under the form |
| `"Too many failed attempts. Try again after X minute(s)."` | Lockout banner with countdown |
| HTTP `429` (login / forgot-password) | "Too many requests — please try again shortly." |
| `"Please verify your email before logging in."` | Show "Resend verification" button → `POST /Auth/resend-verification` |
| `"Your account is inactive"` | "Your account has been deactivated. Contact support." |
| `"Email Already Exist"` / `"Email already exists."` | Inline email-field error |
| `"Tenant already exists."` / `"Slug already exists."` | Company-name/slug inline error + suggested slug |
| `"Passwords do not match."` | Inline confirm-password error |
| `invitation-info.status: valid` | Prefill read-only name/email/role/company, show password form |
| `expired` / `used` / `invalid` | Dedicated states per Flows doc §3.5 ("expired → ask admin to resend", etc.) |
| Accept-invitation success | Auto-login from returned JWT → dashboard + welcome banner |

Also: verify-email page supports **both** the email link (`userId` + `token` in URL — read `email` from query param or logged-in state) and manual entry; handle "already verified" gracefully.

---

## 8. Phase 4 — Deferred Scope

- External job-link apply flow (Flows doc §1.4) — no backend endpoint yet.
- SSO / social login — frontend buttons exist but no backend; **disable/hide them** until `/Auth/google` / `/Auth/linkedin` exist.
- Bulk CSV import — no backend endpoint.
- Cover letter persistence — no `/candidate/*` endpoint; keep in local state.

---

## 9. Implementation Order

1. §5.1 candidate split + privacy checkbox (unblocks the entire candidate flow).
2. §5.2 employer remap + slug (unblocks employer flow).
3. §5.3 login branching + §5.4 workspace picker (unblocks multi-tenant).
4. §6.5 state unification + §6.6 role constants (unblocks guards).
5. §6.7 Remember Me, §6.8 route wiring.
6. §7 error mapping + invitation states.
7. §8 deferred cleanup (hide social buttons).

---

## 10. Validation Checklist

- [ ] Candidate: Step 1 → verification email → verify → Active → Steps 2–4 persist via `/candidate/*` → dashboard.
- [ ] Candidate login: unverified → "verify + resend" path works; verified → dashboard.
- [ ] Employer: Account → Role → Company → Plan → submit → verification → tenant provisioned (`TenantAdmin` in JWT) → dashboard.
- [ ] Slug: generated, unique; collision produces a helpful inline error.
- [ ] Multi-tenant: login returns `requiresTenantSelection` → `/select-workspace` → `select-tenant` → final JWT → dashboard.
- [ ] Invitation: admin invite (numeric role) → email link → `invitation-info` prefill → set password → auto-login.
- [ ] Invitation edge states: expired / used / invalid each show the correct screen.
- [ ] Deactivation: `PATCH /Users/{id}/disable` → target user logged out on next refresh; login shows deactivated message.
- [ ] Role change: `PUT /Users/{id}/role` (string role) → target forced re-login, new permissions applied.
- [ ] Password reset: forgot → email (1h link) → reset → login success.
- [ ] All login failure messages render per §7 table; 429 shows the friendly rate-limit message.
- [ ] Remember Me: 30-day cookies when checked; session-only otherwise.
- [ ] No request to any non-existent endpoint (`register-candidate` removed); no CORS errors.
- [ ] Guards correctly read `isAuthenticated` / `emailConfirmed` / `TenantAdmin` from unified state.

---

*End of document — Version 1.0*

---
