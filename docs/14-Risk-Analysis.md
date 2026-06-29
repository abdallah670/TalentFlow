# TalentFlow — Risk Analysis

> **Document Version:** 1.0  
> **Date:** June 2026  
> **Status:** Approved for Development

---

## 1. Introduction

This document identifies potential risks associated with the development, deployment, and operation of the TalentFlow platform. It categorizes risks, assesses their probability and impact, and defines specific mitigation strategies to minimize disruption to the project and the business.

---

## 2. Risk Assessment Matrix

Risks are evaluated based on **Probability** (Low, Medium, High) and **Impact** (Low, Medium, High, Critical). 

---

## 3. Technical Risks

### TR-01: AI Parsing Accuracy Variability
*   **Description:** The AI resume parser may fail to extract information correctly from heavily formatted, image-based, or non-standard resumes, leading to poor candidate profiles.
*   **Probability:** Medium
*   **Impact:** High (Creates friction for candidates and manual work for recruiters).
*   **Mitigation Strategy:** 
    *   Always display the extracted data to the candidate in an editable form *before* final submission.
    *   Retain the original uploaded PDF for manual review.
    *   Implement fallback logic: if parsing fails entirely, gracefully degrade to a manual entry form.

### TR-02: Multi-Tenancy Data Leakage (Phase 4)
*   **Description:** A flaw in the data access layer could allow users from Company A to view candidates or jobs belonging to Company B.
*   **Probability:** Low
*   **Impact:** Critical (Massive breach of trust, GDPR violations).
*   **Mitigation Strategy:** 
    *   Implement Global Query Filters in EF Core (`modelBuilder.Entity<Job>().HasQueryFilter(j => j.TenantId == _tenantProvider.TenantId)`).
    *   Enforce rigorous unit and integration testing specifically verifying cross-tenant access attempts.
    *   Conduct independent code reviews focused on database query generation.

### TR-03: SignalR Scalability
*   **Description:** Holding open WebSockets for real-time notifications may consume excessive server resources, causing the API to crash under high concurrent load.
*   **Probability:** Medium
*   **Impact:** High
*   **Mitigation Strategy:** 
    *   Offload WebSocket management to Azure SignalR Service in production, ensuring the App Service only handles HTTP logic.
    *   Implement connection timeouts and graceful reconnections in the Angular client.

### TR-04: Third-Party API Rate Limiting (OpenAI)
*   **Description:** Heavy usage of AI generation features might hit OpenAI's API rate limits or incur unexpected high costs.
*   **Probability:** High
*   **Impact:** Medium (Feature degradation, budget overrun).
*   **Mitigation Strategy:** 
    *   Implement caching for generated Job Descriptions if requested with similar parameters.
    *   Use Polly to implement retry policies with exponential backoff for 429 Too Many Requests responses.
    *   Set strict billing limits/alerts on the OpenAI platform.

---

## 4. Product & Business Risks

### BR-01: Low User Adoption due to Complexity
*   **Description:** The system may be perceived as too complex for the target SMB market, which prefers simple tools.
*   **Probability:** Medium
*   **Impact:** High (Failure to achieve business goals).
*   **Mitigation Strategy:** 
    *   Focus UI design on progressive disclosure (hide advanced features like assessments until needed).
    *   Ensure the core "Create Job -> View Kanban Board" workflow takes fewer than 3 clicks.
    *   Conduct user testing during Phase 1 & 2.

### BR-02: Compliance and Data Privacy (GDPR)
*   **Description:** Storing candidate resumes and PII creates significant regulatory liability. Failure to provide data deletion capabilities violates GDPR.
*   **Probability:** High
*   **Impact:** Critical
*   **Mitigation Strategy:** 
    *   Implement a robust "Right to be Forgotten" workflow that hard-deletes database records and Blob storage files.
    *   Ensure data processing agreements are in place with third-party processors (e.g., OpenAI, ensuring data is not used for model training).

---

## 5. Security Risks

### SR-01: Malicious File Uploads
*   **Description:** Candidates upload malware disguised as resumes, which could compromise the server or infect recruiter machines when downloaded.
*   **Probability:** Medium
*   **Impact:** Critical
*   **Mitigation Strategy:** 
    *   Strictly validate file extensions and MIME types.
    *   Do not store files on the application server's local disk in production; use isolated Azure Blob Storage.
    *   Integrate a virus scanning step (e.g., via Azure Defender for Storage) before allowing recruiters to download the file.

### SR-02: Insecure Direct Object Reference (IDOR)
*   **Description:** A user manipulates an API request (e.g., changing `/api/applications/123` to `124`) to view data they do not own.
*   **Probability:** Medium
*   **Impact:** High
*   **Mitigation Strategy:** 
    *   Use GUIDs (`UNIQUEIDENTIFIER`) instead of sequential integers for all primary keys, making them impossible to guess.
    *   Always validate ownership in the Application layer (e.g., `if (application.Job.TenantId != currentUser.TenantId) throw Unauthorized()`).

---

## 6. Execution & Schedule Risks

### ER-01: Scope Creep
*   **Description:** Adding requested features (e.g., custom reporting, video interviews) delays the MVP and subsequent phases.
*   **Probability:** High
*   **Impact:** Medium
*   **Mitigation Strategy:** 
    *   Strict adherence to the Development Roadmap (Document 12).
    *   Any feature not explicitly in the PRD is pushed to the "Future Enhancements" backlog (Phase 6+).

### ER-02: Single Developer Bottleneck
*   **Description:** As a portfolio project built by one developer, illness or loss of motivation could stall the project.
*   **Probability:** Medium
*   **Impact:** High
*   **Mitigation Strategy:** 
    *   Leverage AI coding assistants (GitHub Copilot, Cursor) to accelerate boilerplate generation.
    *   Maintain strict discipline using agile methodologies (short 1-week sprints) to ensure continuous, visible progress.
