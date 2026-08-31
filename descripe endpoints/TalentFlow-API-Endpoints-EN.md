# TalentFlow API — Endpoints Reference

> **Base URL (local):** `https://localhost:44358/api`
> **Auth:** Bearer JWT — add `Authorization: Bearer <token>` to any endpoint that isn't `[AllowAnonymous]`
> **Password rules:** at least 8 characters + one uppercase letter + one lowercase letter + one number (example: `Test@1234`)

---

## Table of Contents
1. [Auth — Registration & Login](#1-auth--registration--login)
2. [Auth — Email Confirmation](#2-auth--email-confirmation)
3. [Auth — Password Recovery](#3-auth--password-recovery)
4. [Auth — Multi-tenant / Workspace](#4-auth--multi-tenant--workspace)
5. [Auth — General](#5-auth--general)
6. [Tenant — Company & Invitations](#6-tenant--company--invitations)
7. [Users — User Management](#7-users--user-management)
8. [Candidate](#8-candidate)
9. [Important Notes for Frontend](#9-important-notes-for-frontend)

---

## 1. Auth — Registration & Login

### `POST /Auth/register`
Registers a new **Candidate** (a regular person looking for a job).

**Auth:** Not required (Anonymous)

**Request Body:**
```json
{
  "firstName": "string",
  "lastName": "string",
  "userName": "string",
  "email": "string",
  "password": "string",
  "confirmPassword": "string"
}
```

**Response 200:**
```json
{
  "isAuthenticated": true,
  "id": "guid",
  "userName": "string",
  "email": "string",
  "roles": ["Candidate"],
  "token": "jwt...",
  "tokenExpiration": "2026-...",
  "refreshToken": "string",
  "refreshTokenExpiration": "2026-...",
  "message": "Registered successfully. Please check your email to confirm your account."
}
```

> ⚠️ The token returned here has **`email_confirmed: false`** inside the JWT claims. The frontend must read this claim and redirect the user to the email confirmation page, rather than treating them as fully logged in.

**Possible errors:**
- `"Email Already Exist"` — the email is already registered
- `"Passwords do not match."` — password and confirmation don't match

---

### `POST /Auth/register-employer`
Registers a **new company + its first admin** (Tenant + TenantAdmin) in a single request.

**Auth:** Not required (Anonymous)

**Request Body:**
```json
{
  "tenantName": "string",
  "slug": "string",
  "subscriptionPlan": "string",
  "companySize": "string",
  "industry": "string",
  "website": "string",
  "linkedIn": "string",
  "officeLocation": "string",
  "firstName": "string",
  "lastName": "string",
  "userName": "string",
  "email": "string",
  "password": "string",
  "confirmPassword": "string"
}
```

**Response 200:** Same shape as `/Auth/register`, but `roles: ["TenantAdmin"]`.

**Possible errors:**
- `"Email already exists."`
- `"Tenant already exists."` — company name is duplicated
- `"Slug already exists."` — the slug must be unique
- `"Passwords do not match."`

---

### `POST /Auth/login`
Login (for any user type — Candidate or Employer).

**Auth:** Not required

**Request Body:**
```json
{
  "email": "string",
  "password": "string"
}
```

**Response — 3 different cases:**

**a) Normal successful login (user belongs to a single company, or is a Candidate):**
```json
{
  "isAuthenticated": true,
  "token": "jwt...",
  "refreshToken": "...",
  "roles": ["..."],
  "currentStep": 2,
  "onboardingCompleted": false
}
```

**b) User belongs to more than one company (Multi-tenant):**
```json
{
  "isAuthenticated": false,
  "requiresTenantSelection": true,
  "availableTenants": [
    { "tenantId": "guid", "tenantName": "string", "role": "string" }
  ],
  "token": "temporary-jwt-valid-for-10-minutes",
  "message": "Please select a workspace to continue."
}
```
Here, the frontend must redirect the user to a **"Choose a company"** page, then call `/Auth/select-tenant` with the chosen tenantId.

**c) Login failure:**
| Message | Reason |
|---|---|
| `"Invalid email or password."` | Wrong email or password |
| `"Too many failed attempts. Try again after X minute(s)."` | Lockout after 5 failed attempts (15 minutes) |
| `"Please verify your email before logging in."` | Email not yet confirmed |
| `"Your account is inactive"` | Admin disabled the account |

> ⚠️ **Rate limiting:** More than 5 requests to `/login` per minute from the same IP returns `429 Too Many Requests`.

---

### `POST /Auth/select-tenant`
Select a specific company after login (when the user belongs to more than one company).

**Auth:** Requires the **temporary** token returned from `/login` in case (b) above.

**Request Body:**
```json
{
  "userId": "guid",
  "tenantId": "guid"
}
```
(The `userId` is actually taken from the token itself on the backend, so it doesn't need to be correct, but the field must be present in the JSON)

**Response 200:** A final, full JWT, same shape as a successful login.

---

## 2. Auth — Email Confirmation

### `GET /Auth/ConfirmEmail?userId=&token=`
The link that arrives directly in the email (the user clicks it from inside the email).

**Auth:** Not required

**Response:** `200 OK` with the text "Email confirmed", or `400` if the token is invalid/expired.

> ⚠️ This is a plain **GET** request, designed to be opened directly from the browser, not to be called by the frontend via `fetch`/`axios`.

---

### `POST /Auth/verify-email`
Confirms the email from a **frontend page** (not directly from the email link) — this is what the frontend should actually use.

**Auth:** Not required

**Request Body:**
```json
{
  "email": "string",
  "token": "string"
}
```
> The `token` comes from the query string of the link in the email (`?userId=...&token=...`) — the frontend must extract it from the URL and send it here along with the email.

**Response 200:**
```json
{ "success": true, "message": "Email confirmed successfully." }
```

**Error response:**
```json
{ "success": false, "message": "Invalid or expired confirmation link." }
```

---

### `POST /Auth/resend-verification`
Resend the confirmation email (if the first one expired — valid for 24 hours).

**Auth:** Not required

**Request Body:**
```json
{ "email": "string" }
```

**Response 200 (always success, even if the email isn't registered — for security reasons):**
```json
{ "success": true, "message": "If this email is registered, a verification link has been sent." }
```

---

### `POST /Auth/email-status`
Check the status of a given email (registered? confirmed?) — useful before the Login or Register step.

**Auth:** Not required

**Request Body:**
```json
{ "email": "string" }
```

**Response 200:**
```json
{ "isRegistered": true, "isConfirmed": false }
```

---

## 3. Auth — Password Recovery

### `POST /Auth/forgot-password`
**Request Body:** `{ "email": "string" }`
Sends a recovery link valid for **one hour**.

### `POST /Auth/reset-password`
**Request Body:**
```json
{
  "email": "string",
  "token": "string",
  "newPassword": "string",
  "confirmPassword": "string"
}
```

---

## 4. Auth — Multi-tenant / Workspace

### `GET /Auth/tenants`
Returns all the companies the current user belongs to (independent of the login flow — useful for the frontend if it wants to display them somewhere like a "Switch Workspace" option inside the app).

**Auth:** Requires a regular (Bearer) token

**Response 200:**
```json
[
  { "tenantId": "guid", "tenantName": "string", "role": "string" }
]
```

---

## 5. Auth — General

| Endpoint | Method | Auth | Description |
|---|---|---|---|
| `/Auth/RefreshToken` or `/Auth/refresh` | POST | No | Renews the access token using the refresh token. Body: `{ "refreshToken": "string" }` |
| `/Auth/logout` | POST | No | Revokes a single refresh token. Body: `{ "refreshToken": "string" }` |
| `/Auth/change-password` | POST | Yes | Body: `{ "oLdPassword", "newPassword", "confirmPassword" }` |
| `/Auth/Profile` | GET | Yes | Current profile data |

---

## 6. Tenant — Company & Invitations

### `GET /Tenant/current`
Data for the current company (based on the Tenant in the token).

**Auth:** Yes

---

### `PUT /Tenant/settings`
Update company settings.

**Auth:** Yes — `TenantAdmin` only

**Request Body:**
```json
{
  "name": "string",
  "slug": "string",
  "subscriptionPlan": "string",
  "companyLogoUrl": "string",
  "primaryColor": "string",
  "timeZone": "string",
  "dateFormat": "string"
}
```

---

### `POST /Tenant/invite-member`
Invite a new member to the company (valid for 7 days).

**Auth:** Yes — `TenantAdmin` only

**Request Body:**
```json
{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "role": 1,
  "customMessage": "string"
}
```
> `role` is a number, not a string — see the [roles table](#roles-enum) below.
> `tenantId` and `invitedByUserId` don't need to be sent correctly by the frontend — the backend takes them automatically from the token, but the fields must still be present in the JSON.

---

### `POST /Tenant/invite/resend`
Resend a pending invitation (if the first one was lost or is about to expire — extends the same invitation by another 7 days).

**Auth:** Yes — `TenantAdmin` only

**Request Body:**
```json
{ "email": "string" }
```

---

### `GET /Auth/invitation-info?token=`
Fetch invitation data (name/email/role/company name) — used on the **"Setup Account"** page to display read-only info before the user sets a password.

**Auth:** Not required

**Response 200:**
```json
{
  "isValid": true,
  "status": "valid",
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "role": "string",
  "companyName": "string"
}
```

**Possible `status` values:** `valid` | `expired` | `used` | `invalid`
> The frontend must show a different screen depending on this value (e.g., "This invitation has expired, request a new one" if `expired`).

---

### `POST /Tenant/accept-invitation`
Accept the invitation and create the account (or join a new company if the user already has an account).

**Auth:** Not required (the user doesn't have an account or token yet)

**Request Body:**
```json
{
  "token": "string",
  "password": "string",
  "confirmPassword": "string"
}
```

**Response 200:** A full JWT — the user is logged in immediately after accepting the invitation (auto-login).

> ⚠️ **Important:** The link that arrives in the invitation email (`.../api/Tenant/accept-invitation?token=...`) is **just a GET link that opens a page**, not the endpoint itself. The frontend must have a page (e.g. `/accept-invitation?token=...`) with a form that takes the password and calls this endpoint via POST. Clicking the email link directly will return a 405 if that happens by mistake.

---

<a name="roles-enum"></a>
### Roles Enum Table
| Number | Name |
|---|---|
| 1 | (based on the seed — most likely `TenantAdmin`) |
| 2 | Recruiter |
| 3 | Hiring Manager |
| 4 | Interviewer |
| 5–6 | Depending on the remaining roles defined in the `Roles` enum |

> ⚠️ The frontend needs the exact values from the backend team (the dropdown on the invitation page must match these numbers exactly).

---

## 7. Users — User Management

All of these endpoints require `TenantAdmin`.

### `GET /Users?command=...`
List of all users in the current company.

### `GET /Users/{id}`
Data for a single user.

### `PUT /Users/{id}`
Edit a user's name/email (**does not include changing the role**).
```json
{ "id": "guid", "firstName": "string", "lastName": "string", "email": "string" }
```

### `PUT /Users/{id}/role`
Change a user's role (separate endpoint).
```json
{ "id": "guid", "role": "string" }
```
> Here, `role` is a **string name** (e.g. `"Recruiter"`), not a number — note the difference from `invite-member`.
> After the change, all of that user's refresh tokens are automatically revoked — the frontend should expect that if this user is currently logged in, they will be forced to log out on their next refresh.

### `PATCH /Users/{id}/disable`
Disable a user. Immediately revokes all of their refresh tokens (forced logout from all devices).

### `POST /Users/Create_user`
Create a new user directly as an admin (without an email invitation).
```json
{
  "firstName": "string", "lastName": "string", "userName": "string",
  "email": "string", "password": "string", "role": "string"
}
```

---

## 8. Candidate

All of these endpoints relate to completing the Candidate's profile after registration (Steps 2–4 of the wizard).

| Endpoint | Method | Description |
|---|---|---|
| `/candidate/me` | GET | Current profile data |
| `/candidate/professional-profile` | PATCH | Step 2: phone, job title, company, years, LinkedIn, portfolio |
| `/candidate/resume` | POST (`multipart/form-data`) | Step 3: upload CV. Fields: `UserId`, `File` |
| `/candidate/skills` | PATCH | Step 3: `{ "userId": "guid", "skillIds": ["guid", ...] }` |
| `/candidate/preferences` | PATCH | Step 4: location, remote, salary range, availability, work authorization |

> The frontend should call these endpoints **after** the user confirms their email, not during the initial Register step.

---

## 9. Important Notes for Frontend

1. **Reading JWT claims:** The token contains:
   - `email_confirmed` (`"true"`/`"false"` as a string) — use it to determine if the user needs to go to the verify-email page
   - `TenantId` — the current company
   - `role` (standard `ClaimTypes.Role` claim)
   - `sub` — the User ID

2. **Refresh Token:** Use `/Auth/refresh` (not necessarily `/Auth/RefreshToken` — both work as aliases for the same thing).

3. **The accept-invitation and verify-email pages must be built in the frontend** — the links sent in emails point to frontend pages (not the API directly), and it's the frontend that calls the appropriate POST endpoints.

4. **Multi-tenant flow:** Any login can return `requiresTenantSelection: true` instead of a direct success — the frontend must handle this case everywhere it performs a login (not just the main login screen).

5. **Rate limiting:** `/login` and `/forgot-password` are limited to 5 requests/minute per IP — if a `429` is returned, show a "try again shortly" message instead of a generic error.

6. **Privacy Policy:** If there's an `acceptedPrivacyPolicy` field (checkbox) on the registration pages, it must be sent as `true` inside the Register/RegisterEmployer body, otherwise the request will be rejected.
