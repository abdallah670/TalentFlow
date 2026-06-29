# 11. Security Design Document (TalentFlow)

## 1. Authentication

### 1.1 JWT Access Token
- **Short‑lived** – 15 minutes.
- **Signature** – RS256 (asymmetric RSA) in production; HS256 acceptable for development.
- **Payload Claims**:
  - `sub` – User ID.
  - `tenant_id` – Tenant identifier.
  - `roles` – Array of role names.
  - `permissions` – Flattened list of permission codes.
- **Validation** – Each request is validated by the JWT Bearer middleware. Expired, invalid, or tampered tokens are rejected with `401 Unauthorized`.

### 1.2 Refresh Token
- **Opaque string** – Cryptographically random bytes (base64‑encoded).
- **Stored hashed** – Only the SHA‑256 hash is stored in the `User` table (`RefreshTokenHash`).
- **Expiry** – 7 days.
- **Rotation** – On successful use, a new refresh token is issued and the old one is immediately revoked (deleted/hash cleared). This prevents token replay.
- **Reuse detection** – If a refresh token is ever reused (i.e. it has already been rotated), the system invalidates **all** refresh tokens for that user (forcing re‑authentication). This detects stolen refresh tokens.

### 1.3 Password Policy
- **Minimum length** – 8 characters.
- **Complexity** – Must contain at least three of: uppercase, lowercase, digit, special character.
- **Hashing** – BCrypt with work factor 12 (tunable). Each password is salted automatically.
- **Account lockout** – After 10 consecutive failed login attempts, the account is temporarily locked for 15 minutes. A successful reset of the lockout counter requires a valid password or reset flow.

### 1.4 Login Security
- **Rate limiting** – The `/auth/login` endpoint is rate‑limited to 5 attempts per minute per client IP (using `AspNetCoreRateLimit`). A custom policy can also be applied per user.
- **Captcha** – (Optional future) After repeated failures, a CAPTCHA challenge can be presented.

## 2. Authorization (RBAC + Permissions)

### 2.1 Roles
Roles are defined at the system level or within a tenant (tenant‑scoped). System roles are applied platform‑wide (e.g., `SystemAdmin`). Tenant‑scoped roles are managed by the tenant admin.

| Role             | Scope   | Typical Responsibilities                                   |
| ---------------- | ------- | ---------------------------------------------------------- |
| **SystemAdmin**  | System  | Manage tenants, monitor platform health, configure global settings |
| **TenantAdmin**  | Tenant  | Manage users and roles, pipeline templates, branding, audit log view |
| **Recruiter**    | Tenant  | Manage jobs/candidates, move candidates, schedule interviews  |
| **HiringManager**| Tenant  | View pipeline, give feedback, approve offers               |
| **Interviewer**  | Tenant  | Submit interview feedback only                             |

### 2.2 Permissions
Permissions are granular, allowing fine‑grained control. They follow a `resource.action` (or `resource.action.detail`) pattern.

**Examples:**
- `job.create`, `job.edit`, `job.delete`
- `candidate.create`, `candidate.view`, `candidate.delete`
- `application.move.stage.screening`, `application.move.stage.offer`
- `interview.schedule`, `interview.feedback.submit`
- `offer.create`, `offer.approve`, `offer.send`
- `analytics.view`, `audit.view`
- `tenant.settings.edit`

### 2.3 Policy‑Based Enforcement
- Permissions are assigned to roles via `RolePermission` mappings.
- A user’s effective permissions are the union of all permissions from their assigned roles.
- The JWT access token includes a `permissions` claim containing a list of these codes.
- Backend APIs use `[Authorize(Policy = "CanMoveCandidateStage")]` where the policy is registered as:
  ```csharp
  options.AddPolicy("CanMoveCandidateStage", policy =>
      policy.RequireClaim("permissions", "application.move.stage.screening", "application.move.stage.offer"));
Complex authorization logic (e.g., “user must be the assigned recruiter for this job”) is performed inside command handlers using IAuthorizationService or by directly checking domain rules.

3. Multi‑Tenant Security
3.1 Tenant Context Resolution
Source of truth – The tenant_id claim in the JWT is the only source of tenant identity. No URL parameter or custom header overrides it.

Middleware – TenantResolutionMiddleware runs early in the pipeline. It extracts the claim and sets a scoped ICurrentTenantService.TenantId.

Missing claim – If the token is valid but lacks a tenant_id, the request is rejected with 401 Unauthorized. This prevents cross‑tenant access by design.

3.2 Data Isolation (Application Layer)
EF Core Global Query Filter – Every entity that implements ITenantScoped (virtually all business entities) gets a global filter:

csharp
modelBuilder.Entity<Job>().HasQueryFilter(j => j.TenantId == _currentTenant.TenantId);
This filter is automatically applied to all queries (including navigation properties). Developers cannot “forget” to filter.

Command/Handler Guard – Before any mutation, handlers load the aggregate from the repository (which respects the query filter). If a user attempts to modify an entity from another tenant by guessing its ID, the load returns null, and a NotFoundException is thrown.

Direct SQL – All raw SQL queries (e.g., for reporting) explicitly pass the current TenantId as a parameter.

3.3 Defense in Depth (Database Level)
Row‑Level Security (RLS) – Can be enabled in SQL Server for additional protection. A security policy would be applied that appends WHERE TenantId = @app_tenant_id to every query, even if EF Core filters are accidentally bypassed.

4. File Security
4.1 Upload Protection
Endpoint – All files are uploaded through the API (multipart/form-data), never directly to blob storage.

Validation – Before storage, the system checks:

File extension whitelist (.pdf, .docx, .txt).

Magic bytes (file signature) verification to prevent content‑type spoofing.

Maximum file size (10 MB).

Naming – Files are renamed to a random GUID. The original filename is stored in the database but never used for storage paths, preventing path traversal.

4.2 Storage & Retrieval
Private Azure Blob Container – Public access is disabled.

Proxy Endpoint – Download requests go to GET /api/v1/files/{id}.

The endpoint verifies the user’s identity, tenant ownership, and authorization (e.g., only assigned interviewer can download a candidate’s resume).

The API generates a time‑limited SAS URL (e.g., 5 minutes) or streams the content directly, without exposing the raw blob URL.

4.3 Malware Protection (Future)
Integration with Azure Defender for Cloud to scan uploaded blobs automatically.

5. Data Protection
5.1 Encryption at Rest
Database – SQL Server Transparent Data Encryption (TDE) or Azure SQL default encryption.

Blob Storage – All blobs are encrypted at rest using Azure Storage Service Encryption.

Backups – Database and blob backups are encrypted.

5.2 Encryption in Transit
TLS 1.2+ – Enforced for all HTTP communications. HTTP Strict Transport Security (HSTS) headers are sent.

5.3 Sensitive Field Handling
Passwords – Hashed with BCrypt (work factor 12).

Refresh Tokens – Stored as SHA‑256 hashes.

Personally Identifiable Information (PII) – GDPR‑sensitive fields can be encrypted at the application level using a symmetric key (AES‑256) stored in Azure Key Vault (future enhancement).

5.4 Secret Management
Production secrets (JWT signing keys, connection strings, API keys) are stored in Azure Key Vault or environment variables mounted from Kubernetes secrets. Never in configuration files.

6. Threat Mitigation & OWASP Top 10 Controls
#	Risk	Mitigation
1	Injection	EF Core parameterized queries exclusively. Raw SQL only used with parameterized commands. Input validation on all user‑supplied data.
2	Broken Authentication	JWT with short expiry, refresh token rotation & theft detection, rate limiting on login, secure password storage (BCrypt). Session management is stateless.
3	Sensitive Data Exposure	TLS everywhere, no secrets in logs, full‑disk encryption, hashed tokens/passwords. Response headers include Cache‑Control: no‑store.
4	XML External Entities (XXE)	XML parsing is disabled. The system only accepts JSON and multipart/form‑data.
5	Broken Access Control	Policy‑based authorization on every endpoint, tenant isolation via global query filters, ownership checks inside command handlers. Direct object reference attacks prevented.
6	Security Misconfiguration	Docker images hardened (non‑root user), unnecessary services disabled, CORS explicitly set to allowed origins only, custom error pages without stack traces, security headers (CSP, X‑Frame‑Options) set.
7	Cross‑Site Scripting (XSS)	Angular automatically sanitizes templates. Content‑Security‑Policy (CSP) headers restrict script sources. The API encodes all output.
8	Insecure Deserialization	System.Text.Json with strict type handling and whitelisting. Polymorphic deserialization is avoided.
9	Using Components with Known Vulnerabilities	Dependabot/Renovate for automated dependency updates. OWASP Dependency Check runs in CI pipeline.
10	Insufficient Logging & Monitoring	Comprehensive immutable audit log for all CUD operations. Application Insights captures exceptions, performance anomalies, and suspicious patterns. Alerts configured for auth failures.
7. Additional Security Headers
X‑Content‑Type‑Options: nosniff

X‑Frame‑Options: DENY

Referrer‑Policy: strict‑origin‑when‑cross‑origin

Permissions‑Policy: geolocation=(), microphone=(), camera=()

Content‑Security‑Policy: default‑src 'self'; script‑src 'self'; style‑src 'self' 'unsafe‑inline'; img‑src 'self' data:;

8. Rate Limiting
API Level – AspNetCoreRateLimit middleware is configured to limit:

100 requests per minute per authenticated user (general API).

20 requests per minute for write operations (POST, PUT, DELETE).

5 requests per minute for the login endpoint per client IP.

Response – When exceeded, the API returns 429 Too Many Requests with a Retry‑After header.

9. Audit & Compliance Logging
Every Create, Update, Delete on core entities generates an immutable audit record (see Audit Strategy in Database Design).

Audit logs are never modifiable by application code.

Integration with SIEM systems can be achieved via structured logging export (future).

10. Penetration Testing & Security Scanning
SAST – Static analysis (SonarQube / GitHub CodeQL) in CI.

DAST – OWASP ZAP scans against the staging environment during each release cycle.

Dependency Scanning – OWASP Dependency Check on every pull request.

Manual Penetration Testing – Conducted befo