# 15. Future Enhancements

## 15.1 SaaS Features

### 15.1.1 Subscription Management
- **Tiered plans** – Free (limited jobs/candidates), Pro (unlimited core features), Enterprise (advanced AI, SSO, dedicated database option).
- **Feature flags** – Granular toggles per plan stored in `Tenant.FeatureFlags` JSON field, enforced via `FeatureGate` attribute and Angular guard.
- **Tenant self‑service upgrade** – In‑app UI to upgrade from Free to Pro; automatic provisioning of higher limits.
- **Usage tracking** – Dashboard showing consumed resources (active jobs, storage used, API calls) against plan quota.

### 15.1.2 Billing
- **Payment integration** – Stripe or Chargebee for recurring subscriptions, invoices, and payment methods.
- **Invoice history** – Tenant admin can view, download, and export invoices.
- **Dunning management** – Automatic retries for failed payments; eventual tenant suspension (read‑only mode) after grace period.
- **Usage‑based add‑ons** – Purchase additional candidate slots, storage, or AI credits without upgrading plan.

### 15.1.3 Tenant Provisioning Automation
- **Self‑service sign‑up** – Public registration with email verification, CAPTCHA, and subdomain selection.
- **Guided setup wizard** – Multi‑step onboarding: company branding, first pipeline template, invite team members.
- **Templates gallery** – Pre‑built pipeline templates for common industries (tech, healthcare, retail) that new tenants can clone.
- **White‑label option** – Custom domain (CNAME), email sender domain, and full CSS customization for Enterprise tenants.

## 15.2 AI Features

### 15.2.1 Resume Parsing
- **Structured extraction** – Uploaded PDF/DOCX resumes processed via Azure Form Recognizer or custom NLP model; outputs JSON with sections: personal info, work experience, education, skills.
- **Data population** – Automatically fill `Candidate` fields and `ParsedResume` table (skills, years of experience, last role).
- **Multilingual support** – Parse resumes in major languages (English, Spanish, German, etc.).
- **Confidence scores** – Flag low‑confidence extractions for manual review.

### 15.2.2 Candidate Ranking
- **Skill matching** – Compare parsed candidate skills with job required skills (explicit match + semantic similarity via embeddings).
- **Scoring algorithm** – Weighted score combining skills, experience level, education, and source of application.
- **Ranked pipeline view** – Sort candidates within a stage by AI score, with toggle to revert to chronological.
- **Feedback loop** – Recruiters can “thumbs up/down” ranking to continuously improve model (reinforcement learning).

### 15.2.3 Skill Extraction & Gap Analysis
- **Job description analysis** – NLP extracts required and preferred skills from job posting text.
- **Candidate gap report** – For a job, visualize which required skills are scarce in the applicant pool.
- **Suggest skills** – When creating a job, auto‑suggest skills based on similar roles in the tenant.

### 15.2.4 Interview Question Generation
- **Contextual questions** – Generate technical/behavioral questions based on job skills and candidate’s resume gaps.
- **Question bank** – Tenants can curate an AI‑powered question bank, rated by effectiveness.
- **Live interview assistance** – (Future) Real‑time transcription and suggested follow‑up questions.

### 15.2.5 Candidate‑Job Matching (Recommendations)
- **Internal mobility** – For existing employees (if HRIS integrated), recommend open positions they qualify for.
- **Passive candidate matching** – From past applicants in the database, surface those who match new jobs.
- **Similar candidates** – “Candidates like this” based on skill vectors and hiring outcomes.

### 15.2.6 Recruitment Insights & Predictive Analytics
- **Time‑to‑hire prediction** – Estimate time to fill a role based on historical data and current pipeline velocity.
- **Attrition risk** – Flag candidates likely to drop out (e.g., long time in stage, no recent activity).
- **Source ROI** – Machine learning model that attributes hires to sourcing channels beyond last‑touch.

## 15.3 Enterprise Features

### 15.3.1 SSO & Identity Management
- **SAML 2.0 / OpenID Connect** – Integrate with Azure AD, Okta, OneLogin, and generic IdPs.
- **Just‑in‑time provisioning** – Auto‑create user on first SSO login, mapping groups to TalentFlow roles.
- **Multi‑Factor Authentication** – Enforce MFA via IdP or built‑in TOTP for standalone accounts.
- **SCIM provisioning** – Automated user lifecycle management from identity provider.

### 15.3.2 Advanced Permissions & Roles
- **Custom role builder** – UI to define roles with granular permission matrix (checkboxes per action).
- **Field‑level access** – Hide salary fields from interviewers; show only anonymized profiles to external reviewers.
- **Temporary access delegation** – Recruiters can delegate their tasks to a colleague during vacation with automatic expiry.
- **Department‑scoped roles** – Limit a recruiter’s visibility to only their own department’s jobs and candidates.

### 15.3.3 Custom Workflows & Pipeline Automation
- **Visual workflow designer** – Drag‑and‑drop editor to create branching pipelines with conditional logic (e.g., if location = Remote → skip on‑site interview).
- **Parallel stages** – Candidate can be in multiple stages simultaneously (e.g., background check and final interview).
- **SLA & Escalation** – Set maximum time in each stage; auto‑notify manager if exceeded.
- **Automated actions** – On stage entry, trigger actions like sending assessment, scheduling interview, or sending templated email.

### 15.3.4 External Job Board Integration
- **One‑click publishing** – Post jobs to LinkedIn, Indeed, Glassdoor, and niche boards via API.
- **Source tracking** – UTM parameters and referral codes automatically attributed to applications.
- **Job wrapping** – Embeddable job widget for company career page.

### 15.3.5 Compliance & Data Residency
- **Data‑center selection** – At tenant creation, choose geographic region for data storage (GDPR).
- **Data retention policies** – Configurable per tenant: auto‑anonymize candidates after X months.
- **Export & portability** – One‑click export of all tenant data (candidates, jobs, audit logs) in machine‑readable format.
- **Consent management** – Track and record candidate consent for data processing.

### 15.3.6 Marketplace & Integrations
- **HRIS Sync** – Bi‑directional integration with BambooHR, Workday, SAP SuccessFactors (employee data, convert candidate to employee).
- **Calendar Integration** – Microsoft Graph and Google Calendar for interview scheduling with availability lookup.
- **Video Interviewing** – Embed Zoom/Teams/Google Meet links; auto‑record permissions.
- **Background Check** – API integration with Checkr or Sterling for automated background screening.
- **Webhooks & API Gateway** – Publish events (candidate moved, offer accepted) to customer‑defined endpoints.

### 15.3.7 Offline & Mobile
- **Progressive Web App (PWA)** – Installable, offline‑capable for feedback submission and candidate review.
- **Push notifications** – Browser push for critical alerts when the app is not open.
- **Native mobile app** – (Long‑term) iOS/Android with biometric unlock and camera integration for document scanning.