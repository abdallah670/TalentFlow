# TalentFlow — Registration & Login Flows

> **Document Version:** 1.1  
> **Last Updated:** July 2026  
> **Status:** Final  

---

## Table of Contents

1. [Candidate Registration & Login](#1-candidate-registration--login)
2. [Company/Employer Registration & Login](#2-companyemployer-registration--login)
3. [Invited Team Member Flow](#3-invited-team-member-flow)
4. [User Management Scenarios](#4-user-management-scenarios)
5. [Summary Tables](#5-summary-tables)

---

## 1. Candidate Registration & Login

### 1.1 Self-Registration (Public Career Page / Job Application)

The candidate self-registration flow consists of four progressive steps, with email verification triggered after Step 1.

#### Step 1 — Account Creation

| Field | Type | Required | Validation Rules |
|-------|------|----------|-----------------|
| First Name | Text | Yes | Maximum 100 characters |
| Last Name | Text | Yes | Maximum 100 characters |
| Email | Email | Yes | Unique system-wide; used as the login identifier |
| Password | Password | Yes | Minimum 8 characters; must include uppercase, lowercase, and a digit |
| Confirm Password | Password | Yes | Must match the Password field |
| I agree to Privacy Policy | Checkbox | Yes | Link to the full Privacy Policy document |

> **Email Verification:** Upon submission of Step 1, a verification email is dispatched. The account status remains **Pending** until the email is verified. The verification link is valid for **24 hours**. Once verified, the account status transitions to **Active** and the candidate can log in.

#### Step 2 — Professional Profile

| Field | Type | Required | Validation Rules |
|-------|------|----------|-----------------|
| Phone Number | Telephone | No | Country code picker with format validation |
| Current Job Title | Text | No | Free text |
| Current Company | Text | No | Free text |
| Total Years of Experience | Number | No | Range: 0–50 |
| LinkedIn URL | URL | No | Must conform to URL format |
| Portfolio/GitHub URL | URL | No | Must conform to URL format |

#### Step 3 — Resume & Skills

| Field | Type | Required | Validation Rules |
|-------|------|----------|-----------------|
| Resume Upload | File | No | Accepted formats: PDF, DOCX; maximum size: 10MB; drag-and-drop interface |
| Skills | Chips / Tags | No | Free text with autocomplete suggestions from a predefined skill library |
| Cover Letter | Textarea | No | Maximum 2,000 characters; per-job application context |

#### Step 4 — Preferences

| Field | Type | Required | Validation Rules |
|-------|------|----------|-----------------|
| Preferred Location | Text | No | City name or "Remote" |
| Remote Only | Toggle | No | Default: false |
| Salary Expectation | Range | No | Minimum and maximum with currency selection |
| Available From | Date | No | Notice period start date |
| Work Authorization | Dropdown | No | Options: Citizen, Visa, Sponsorship needed |

---

### 1.2 Candidate Login

| Field | Type | Required | Notes |
|-------|------|----------|-------|
| Email | Email | Yes | Registered email address |
| Password | Password | Yes | — |
| Remember Me | Checkbox | No | Extends session to 30 days |

#### Login Scenarios

| Scenario | System Response |
|----------|----------------|
| Valid credentials + email verified | Login succeeds → redirect to Dashboard |
| Valid credentials + email not verified | Display: "Please verify your email first." Offer: "Resend verification?" |
| Invalid credentials | Display: "Invalid email or password" |
| 5 failed attempts | Account locked for 15 minutes. Display: "Too many attempts. Try again in X minutes or reset password." |
| Account deactivated by admin | Display: "Your account has been deactivated. Contact support." |

---

### 1.3 Candidate Password Reset

| Step | Action |
|------|--------|
| 1 | Candidate clicks "Forgot Password" on the login page |
| 2 | Candidate enters their registered email address |
| 3 | System sends a password reset email with a link valid for 1 hour |
| 4 | Candidate clicks the link → enters new password + confirmation |
| 5 | System redirects to the login page with a success message |

---

### 1.4 Candidate Applied via External Job Link (No Account Yet)

| Step | Action |
|------|--------|
| 1 | Candidate clicks a job link from LinkedIn, a career site, or an external source |
| 2 | Candidate lands on the job detail page with an "Apply" button |
| 3 | Candidate clicks "Apply" → presented with the Step 1 registration form only |
| 4 | After registration, candidate is automatically directed to the job application form (resume upload + application questions) |
| 5 | Account creation and application submission occur in a single unified flow |

---

## 2. Company/Employer Registration & Login

### 2.1 Founder/HR Self-Signup (Create Tenant)

#### Step 1 — Account

| Field | Type | Required | Validation Rules |
|-------|------|----------|-----------------|
| First Name | Text | Yes | — |
| Last Name | Text | Yes | — |
| Email | Email | Yes | Work email preferred |
| Password | Password | Yes | Minimum 8 characters with complexity requirements |
| Confirm Password | Password | Yes | Must match the Password field |

#### Step 2 — Role

| Field | Type | Required | Validation Rules |
|-------|------|----------|-----------------|
| I am the… | Radio | Yes | Options: Founder/CEO, HR Manager/Recruiter, Hiring Manager/Team Lead, Other |
| If Other | Text | No | Specify the role |

#### Step 3 — Company Details

| Field | Type | Required | Validation Rules |
|-------|------|----------|-----------------|
| Company Name | Text | Yes | — |
| Company Size | Dropdown | Yes | Options: 1–10, 11–50, 51–200, 201–500, 501–1,000, 1,000+ |
| Industry | Dropdown | Yes | Options: Technology, Healthcare, Finance, Retail, etc. |
| Company Website | URL | No | Must conform to URL format |
| Company LinkedIn | URL | No | Must conform to URL format |
| Office Location | Text | No | Primary office location |

#### Step 4 — Subscription Plan

| Plan | Price | Features |
|------|-------|----------|
| Free | $0 | 3 open jobs, 50 candidates, basic pipeline |
| Pro | $49/month | Unlimited jobs, advanced analytics, assessments |
| Enterprise | Custom | SSO, dedicated database, API access, custom workflows |

#### Step 5 — Confirmation

After email verification, the tenant is provisioned:

1. Default roles are created: **Admin**, **Recruiter**, **Hiring Manager**, **Interviewer**
2. A default pipeline template ("Standard Hiring") is generated
3. The registering user is automatically assigned as **Tenant Admin**
4. The user is redirected to the Dashboard with an onboarding wizard

---

### 2.2 Employer Login

| Field | Type | Required | Notes |
|-------|------|----------|-------|
| Email | Email | Yes | Work email address |
| Password | Password | Yes | — |
| Remember Me | Checkbox | No | 30-day session |

#### Login Scenarios

| Scenario | System Response |
|----------|----------------|
| Valid credentials + tenant active | Login succeeds → redirect to Dashboard |
| Valid credentials + trial expired | Display: "Your trial has ended. Upgrade to continue." → redirect to billing page |
| Valid credentials + tenant suspended | Display: "Your account has been suspended. Contact support." |
| Invalid credentials | Display: "Invalid email or password" |
| 5 failed attempts | Account locked for 15 minutes |
| User belongs to multiple tenants | Display the "Select Workspace" screen after login |

#### Multi-Tenant User Login

A user (e.g., an external recruiter) may belong to multiple tenants. After successful authentication, if multiple tenants are detected, the system displays a **"Select Workspace"** screen. Each workspace entry shows the company name, logo, and the user's role within that tenant.

---

### 2.3 Employer Password Reset

Same flow as the candidate password reset: email → reset link → new password → redirect to login with success message.

---

## 3. Invited Team Member Flow

### 3.1 Admin Invites a Member

**Admin Action:**

1. Navigate to **Settings → User Management → Invite Member**
2. Complete the invitation form:

| Field | Type | Required | Validation Rules |
|-------|------|----------|-----------------|
| First Name | Text | Yes | — |
| Last Name | Text | Yes | — |
| Email | Email | Yes | Work email address |
| Role | Dropdown | Yes | Options: Admin, Recruiter, Hiring Manager, Interviewer |
| Custom Message | Textarea | No | Optional personal note |

3. Click **Send Invitation**

**System Action:**

1. A user record is created with status **Invited** (no password set yet)
2. A unique invitation token is generated (valid for 7 days)
3. An invitation email is sent to the recipient

---

### 3.2 Invitation Email

**Subject:** `[First Name], you've been invited to join [Company Name] on TalentFlow`

**Body:**

```
Hi [First Name],

[Admin First Name] has invited you to join [Company Name] on TalentFlow as a [Role].

Click the button below to set up your account:

[ Set Up Your Account ]

This link expires in 7 days.

If you weren't expecting this invitation, please ignore this email. — The TalentFlow Team
```

---

### 3.3 Invited Member Accepts & Sets Up Account

#### Step 1 — Click Invitation Link

The system validates the token:

| Token State | System Response |
|-------------|----------------|
| Valid | Proceed to Step 2 |
| Expired | Display: "This invitation has expired. Please ask your admin to resend." |
| Already used | Display: "This invitation has already been accepted." |

#### Step 2 — Set Password

| Field | Type | Required | Validation Rules |
|-------|------|----------|-----------------|
| Create Password | Password | Yes | Minimum 8 characters with complexity requirements |
| Confirm Password | Password | Yes | Must match the Password field |

> First Name, Last Name, Email, and Role are pre-filled and read-only.

#### Step 3 — Confirmation

1. Password is set → account status changes to **Active**
2. The user is automatically logged in
3. The user is redirected to the Dashboard with a welcome banner

> **Note:** No email verification is required — the admin has already verified the user's identity.

---

### 3.4 Invited Member — Subsequent Logins

Standard login with Email + Password. No difference from self-signup users after the initial setup is complete.

---

### 3.5 Edge Cases

| Scenario | Handling |
|----------|----------|
| Admin invites someone already in the system (same email) | Display: "This email already has a TalentFlow account. They will be added to your tenant." The existing user receives a notification: "You've been added to [Company Name] as [Role]." |
| Token expired (7+ days) | Display: "Invitation expired. Contact your admin to resend." The admin can click "Resend Invitation" in User Management. |
| User already active in this tenant | Error: "This user is already a member of your tenant." |
| Admin deactivates user before they accept | Token is invalidated immediately. The user sees: "This invitation is no longer valid." |
| User invited to multiple tenants | Each invitation is handled independently. After login, the workspace picker is displayed. |

---

## 4. User Management Scenarios (Post-Login)

### 4.1 Admin Deactivates a User

1. Admin navigates to **User Management** and clicks **Deactivate**
2. The user is immediately logged out across all sessions (all session tokens are invalidated)
3. Subsequent login attempts display: "Your account has been deactivated. Contact your admin."

### 4.2 Admin Changes a User's Role

1. Admin changes the role in **User Management**
2. The user's permissions update on the next token refresh
3. No disruption to the current session; new permissions apply on the next authenticated request

### 4.3 User Offboarding (Employee Leaves)

1. Admin deactivates the user
2. The user's audit trail remains preserved (all actions they performed are retained)
3. The user's assigned tasks (interviews, approvals) are reassigned or flagged for admin attention

---

## 5. Summary Tables

### 5.1 Registration Cases Summary

| Case | Who Registers | Email Verified? | Password Set When? | Immediate Access? |
|------|---------------|-----------------|---------------------|-------------------|
| Candidate self-signup | Candidate | Yes (email link) | During registration | After email verified |
| Candidate applies via job link | Candidate | Yes (email link) | During registration | After email verified |
| Founder/HR self-signup | Company representative | Yes (email link) | During registration | After email verified + tenant provisioned |
| Admin invites member | Admin | Pre-verified by admin | On first login via invitation link | Immediately after password set |
| Existing user added to another tenant | Admin | Already verified | Already set | Immediately after accepting tenant invite |
| Bulk CSV import | Admin | Pre-verified (admin) | Temporary password sent via email | Must change password on first login |
| SSO (Enterprise — Future) | Identity Provider | Pre-verified by IdP | Not needed (IdP handles authentication) | Immediately after IdP authentication |

### 5.2 Login Summary

| User Type | Login Method | Workspace Picker? | Lockout? | Remember Me? |
|-----------|-------------|-------------------|----------|--------------|
| Candidate | Email + Password | No (single context) | Yes (5 attempts) | Yes (30 days) |
| Employer (single tenant) | Email + Password | No | Yes (5 attempts) | Yes (30 days) |
| Employer (multi-tenant) | Email + Password | Yes (after login) | Yes (5 attempts) | Yes (30 days) |
| Invited member (first login) | Invitation link + set password | No | N/A (first login) | N/A |
| SSO user (future) | IdP redirect | Yes (if multi-tenant) | Handled by IdP | Handled by IdP |

---

*Document Version: 1.1 | Last Updated: July 2026*