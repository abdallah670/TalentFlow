# 13. Testing Strategy (TalentFlow)

## 1. Overview
This document defines the comprehensive testing approach for TalentFlow. It covers all layers of the testing pyramid, from unit tests to production smoke tests, with a strong emphasis on multi‑tenant isolation, security, and performance.

## 2. Testing Principles
- **Shift‑left** – Tests run early and often in CI; no manual gate for basic quality checks.
- **Isolation** – Tests must not rely on shared state; each test is independent.
- **Realistic data** – Use production‑like data volumes for performance tests.
- **Security‑first** – Security tests are integrated into every phase, not a final afterthought.

## 3. Unit Testing

### 3.1 Scope
- Domain services (workflow validation, offer approval, stage transition rules).
- Command/Query handlers (business logic, validation).
- Value objects and entity methods.
- FluentValidation validators.
- Mapping profiles (AutoMapper).

### 3.2 Framework & Tools
- **xUnit** – Test framework.
- **NSubstitute** – Mocking library (preferred over Moq for performance and simplicity).
- **FluentAssertions** – Readable assertions.
- **AutoFixture** – Generate test data (optional).

### 3.3 Approach
- **Domain layer**: No dependencies mocked. Create aggregates with valid state, call methods, assert events raised and state changed.
- **Handlers**: Mock repository interfaces and infrastructure services. Verify that correct methods are called and that domain events are raised.
- **Validators**: Test all validation rules with valid and invalid inputs.

### 3.4 Coverage Target
- Domain layer: ≥ 90% line coverage.
- Application layer: ≥ 80% line coverage.
- Overall: ≥ 80% combined.

### 3.5 Example
```csharp
[Fact]
public void MoveToStage_WhenTransitionIsValid_ShouldUpdateStageAndRaiseEvent()
{
    var application = new Application(/* ... */);
    var template = WorkflowTemplate.CreateStandard();
    
    application.MoveToStage("Interview", actingUserId, template);
    
    application.CurrentStage.Name.Should().Be("Interview");
    application.DomainEvents.Should().ContainSingle(e => e is CandidateMovedToStageEvent);
}
4. Integration Testing
4.1 Scope
Repository implementations against a real database (EF Core).

Global query filter (tenant isolation) enforcement.

Audit interceptor (old/new values captured).

API middleware pipeline (tenant resolution, auth, rate limiting).

Hangfire job execution end‑to‑end.

4.2 Framework & Tools
Testcontainers for .NET – Spin up SQL Server (or Azure SQL Edge) in Docker during tests.

WebApplicationFactory – In‑memory API testing with full middleware, but external services (email, blob) mocked.

Respawn – Reset database state between test classes for performance.

4.3 Approach
Database integration: Each test class uses a fresh database schema (migrations run automatically). Repositories are tested against real SQL Server. Queries must include the tenant filter; tests verify that cross‑tenant data is invisible.

API integration: WebApplicationFactory creates a test server. Authenticated requests with JWT are used (tokens generated with test keys). Verify that endpoints return correct status codes, enforce permissions, and apply tenant isolation.

Audit interceptor: After SaveChanges, assert that AuditLog table contains the expected entries.

4.4 Example
csharp
[Fact]
public async Task GetJobs_WhenTenantA_ShouldNotReturnTenantBJobs()
{
    var tenantAJobs = await _apiClient.GetJobsAsync(tenantAToken);
    tenantAJobs.Should().OnlyContain(j => j.TenantId == tenantA.Id);
}
5. API Testing (Contract & Smoke)
5.1 Scope
Every public REST endpoint.

Correct HTTP status codes for valid/invalid requests.

Response schema matches OpenAPI specification.

Authentication and authorization boundaries.

Pagination and filtering standards.

5.2 Tools
Postman / Newman – Collections for manual exploration and automated CI runs.

OpenAPI (Swagger) – Schema validation.

FluentAssertions – In‑code API tests using HttpClient.

5.3 Approach
Maintain a Postman collection organized by domain (Jobs, Candidates, etc.). Include pre‑request scripts to obtain JWT tokens.

Run Newman in CI against a deployed staging environment.

Validate response body against JSON schema generated from DTOs.

6. End‑to‑End (E2E) Testing
6.1 Scope
Critical user journeys that span multiple services and UI interactions:

Login → create job → add candidate → move through stages → schedule interview → submit feedback → generate offer → approve → send → accept.

Multi‑tenant scenario: two tenants, verify data isolation from the UI perspective.

Role‑based access: recruiter vs. interviewer vs. admin.

6.2 Framework & Tools
Playwright (preferred) or Cypress.

Docker Compose with all services, including a seeded test database.

6.3 Approach
Tests run in CI against a full‑stack ephemeral environment (Docker Compose on GitHub Actions runner or dedicated test host).

Use test data factories to set up known states.

Assertions on UI elements (text, visibility) and network requests (API calls made).

6.4 Example
typescript
test('Complete hiring flow', async ({ page }) => {
  await page.goto('/login');
  // ... login as recruiter
  await page.click('text=New Job');
  // ... fill and submit form
  await page.click('text=Pipeline');
  await page.click('text=Move to Interview');
  await expect(page.locator('.notification')).toContainText('candidate moved');
});
7. Security Testing
7.1 Static Application Security Testing (SAST)
Tool: SonarQube / GitHub CodeQL.

Frequency: Every pull request.

Focus: Injection flaws, hardcoded secrets, weak cryptography.

7.2 Dependency Scanning
Tool: OWASP Dependency Check / Dependabot.

Frequency: Daily (or on every push).

Action: Automatically open PRs for vulnerable dependencies.

7.3 Dynamic Application Security Testing (DAST)
Tool: OWASP ZAP.

Environment: Staging.

Frequency: Weekly or before each release.

Scope: Spider the API, run active scans for OWASP Top 10.

7.4 Manual Penetration Testing
Conducted before initial production launch and after major architectural changes.

Focus areas:

Tenant isolation bypass (manipulate tenant_id claim, URL guessing).

Privilege escalation.

File upload vulnerabilities (malware, oversized files).

Rate limiting effectiveness.

8. Load & Performance Testing
8.1 Tools
k6 (preferred) for scriptable load tests.

JMeter as an alternative.

8.2 Test Scenarios
Scenario	Target	Notes
Candidate list fetch	500 concurrent recruiters fetching paginated lists	p95 < 200ms
Stage moves	100 concurrent stage transitions per minute	Ensure transactional consistency, no deadlocks
Dashboard load	50 concurrent HR ops loading analytics dashboard with 100k candidates in DB	Pre‑warmed caches, response < 1s
File upload	50 concurrent resume uploads (10 MB each)	Validate blob storage throughput
Login burst	200 logins in 1 minute	Rate limiter works, no 500 errors
8.3 Environment
Production‑like infrastructure (Azure staging). Use realistic data volumes (seeded 100k candidates, 10k jobs).

8.4 Monitoring
Application Insights for server metrics (response time, CPU, memory).

SQL Database Query Performance Insights for slow queries.

k6 output exported to JSON for reporting.

9. Testing in CI/CD Pipeline
On push / PR:

Unit tests (fast, <5 min).
Integration tests (parallel, <10 min with Testcontainers).
SAST & dependency scans.
On merge to main:

Deploy to staging environment.

Run E2E tests.

Run API smoke tests (Newman).

Run DAST (ZAP) asynchronously.

Release pipeline:

Load tests in a dedicated performance environment.

Blue‑green deployment with smoke tests before switching traffic.

10. Test Data Management
Anonymized production data is not used due to early stage; synthetic data generators create realistic tenant and candidate data.

Use Bogus or AutoFixture for unit/integration tests.

For load tests, a custom seeder generates bulk data with realistic distributions (e.g., 60% of candidates in early stages).

11. Defect Management
All test failures are immediately visible in CI.

Flaky tests are quarantined and fixed within the sprint.

Penetration test findings are triaged by severity; critical/high must be resolved before production release.