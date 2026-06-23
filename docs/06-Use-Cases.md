# TalentFlow — Use Cases

> **Document Version:** 1.0  
> **Date:** June 2026  
> **Status:** Approved for Development

---

## 1. Introduction

This document details the primary use cases for the TalentFlow system. Each use case defines the interaction between external actors and the system to achieve a specific goal, including main flows, alternative flows, and exception handling.

---

## UC-01: Manage Job Posting Lifecycle

### 1.1 Overview
Describes the process by which an HR Specialist or Admin creates, publishes, edits, and eventually archives a job posting within the system.

### 1.2 Actors
*   **Primary:** HR Specialist, Administrator
*   **System:** TalentFlow Backend, Database

### 1.3 Preconditions
*   The actor is authenticated and holds the required role (Admin or HR).
*   The system has predefined departments and pipeline stages configured.

### 1.4 Main Success Scenario (Flow)
1.  Actor navigates to the 'Jobs' module and selects 'Create New Job'.
2.  System presents the job creation form.
3.  Actor enters details: Title, Department, Location, Type, Salary, and Required Skills.
4.  Actor requests AI assistance to generate the Job Description based on Title and Skills.
5.  System calls the AI service and populates the description field.
6.  Actor reviews and modifies the generated description.
7.  Actor selects 'Publish Job'.
8.  System validates all required fields and business rules.
9.  System saves the job with status 'Active', generates a public link, and logs the action.
10. System displays a success message with the job URL.

### 1.5 Alternative Flows
*   **1.5a Save as Draft:** In step 7, the actor chooses 'Save as Draft'. The system saves the job with status 'Draft'. It is not visible publicly.
*   **1.5b Edit Active Job:** The actor selects an existing 'Active' job and modifies its details. The system updates the record and logs the changes.
*   **1.5c Archive Job:** The actor selects an 'Active' job and clicks 'Archive'. The system changes the status to 'Archived', removes it from the public page, and prevents new applications.

### 1.6 Exception Flows
*   **1.6a AI Service Unavailable:** In step 5, if the AI service times out or returns an error, the system displays a warning "AI generation unavailable" and allows the actor to enter the description manually.
*   **1.6b Validation Failure:** In step 8, if required fields are missing (e.g., Department), the system highlights the missing fields and prevents saving until corrected.

---

## UC-02: Candidate Application and Resume Parsing

### 2.1 Overview
A candidate applies for an active job using the public careers portal by uploading their resume. The system automatically parses the document.

### 2.2 Actors
*   **Primary:** Candidate (Unauthenticated)
*   **System:** TalentFlow Frontend, Backend, AI Parsing Service, Storage Service

### 2.3 Preconditions
*   A job is in the 'Active' state and accepting applications.
*   The candidate has access to the job's public URL.

### 2.4 Main Success Scenario (Flow)
1.  Candidate navigates to the job posting URL.
2.  System displays job details and an 'Apply Now' form.
3.  Candidate uploads a resume document (PDF or DOCX).
4.  System uploads the file to secure storage and triggers the AI Parser.
5.  System parses the document and extracts PII (Name, Email, Phone), Skills, and Experience.
6.  System populates the application form with extracted data.
7.  Candidate reviews the pre-filled data, makes necessary corrections, and submits the application.
8.  System validates the submission (e.g., unique email for this job).
9.  System creates a Candidate Profile (if new) and an Application record linked to the job.
10. System sends a confirmation email to the candidate and logs the application.
11. System displays a success page to the candidate.

### 2.5 Alternative Flows
*   **2.5a Existing Candidate:** In step 9, if the email already exists in the system, a new Application record is created and linked to the existing Candidate Profile.
*   **2.5b Manual Entry:** The candidate skips the resume upload and manually fills in all required fields.

### 2.6 Exception Flows
*   **2.6a File Too Large/Invalid Format:** In step 3, if the file exceeds 10MB or is not a supported format, the system rejects the file and prompts the user to upload a valid document.
*   **2.6b Parsing Failure:** In step 5, if the resume cannot be parsed (e.g., image-based PDF), the system alerts the candidate to enter details manually.
*   **2.6c Duplicate Application:** In step 8, if the candidate has an active application for the same job, the system rejects the submission with a message "You have already applied for this position."

---

## UC-03: Process Application Pipeline

### 3.1 Overview
A Recruiter manages candidates through the hiring stages using the Kanban board, moving them from initial application to final decision.

### 3.2 Actors
*   **Primary:** Recruiter, HR Specialist
*   **System:** TalentFlow Backend, Notification Service

### 3.3 Preconditions
*   Actor is authenticated.
*   A job exists with active applications in the pipeline.

### 3.4 Main Success Scenario (Flow)
1.  Actor navigates to the 'Pipeline' view for a specific job.
2.  System displays the Kanban board with columns representing stages (e.g., Applied, Screened, Interview).
3.  Actor reviews a candidate card in the 'Applied' column, noting the AI Match Score.
4.  Actor clicks the card to review the full candidate profile and resume.
5.  Actor drags the card from 'Applied' to 'Screened'.
6.  System updates the application stage in the database.
7.  System creates an audit log entry for the stage change.
8.  System triggers configured automated actions (e.g., sending a status update email to the candidate).
9.  System updates the UI for all connected clients via SignalR to reflect the new stage.

### 3.5 Alternative Flows
*   **3.5a Reject Candidate:** Actor moves the card to the 'Rejected' stage. System prompts the actor to select a rejection reason (optional) and triggers a polite rejection email.
*   **3.5b Bulk Move:** Actor selects multiple candidate cards and moves them to the next stage simultaneously.

### 3.6 Exception Flows
*   **3.6a Concurrent Modification Conflict:** In step 6, if another user has already moved the candidate or deleted the application, the system rejects the change, alerts the actor, and refreshes the board.
*   **3.6b Missing Prerequisites:** If moving to 'Offer' stage requires interview feedback, and none exists, the system blocks the move and displays an error "Cannot move to Offer: No interview feedback submitted."

---

## UC-04: Schedule and Conduct Interview

### 4.1 Overview
A Recruiter schedules an interview, and assigned Interviewers submit structured feedback after it concludes.

### 4.2 Actors
*   **Primary:** Recruiter, Interviewer, Hiring Manager
*   **Secondary:** Candidate
*   **System:** TalentFlow Backend, Notification Service, AI Service

### 4.3 Preconditions
*   Candidate application is in an 'Interview' stage.
*   Interviewers have registered accounts in the system.

### 4.4 Main Success Scenario (Flow)
1.  Recruiter initiates scheduling from the candidate's application view.
2.  Recruiter inputs interview details (Date, Time, Type, Video Link) and selects Interviewers.
3.  System saves the interview record and emails calendar invites to the candidate and Interviewers.
4.  *...Time passes, interview occurs...*
5.  Interviewer logs in and navigates to 'My Interviews'.
6.  Interviewer clicks 'Submit Feedback' for the completed interview.
7.  System presents a structured feedback form with rating criteria (e.g., Technical Skills, Communication).
8.  Interviewer requests AI to suggest questions based on the resume (Alternative: did this before the interview).
9.  Interviewer enters scores (1-5) and notes, then selects an overall recommendation ('Hire' or 'No Hire').
10. Interviewer submits the form.
11. System saves the feedback immutably and notifies the Recruiter/Hiring Manager.

### 4.5 Alternative Flows
*   **4.5a Multi-Interviewer Consensus:** Multiple interviewers submit feedback. System aggregates scores. Hiring Manager views a unified dashboard showing all feedback and an AI-generated summary paragraph.
*   **4.5b Reschedule:** Recruiter edits the interview details before it occurs. System sends updated calendar invites.

### 4.6 Exception Flows
*   **4.6a Late Feedback:** If feedback is not submitted within 48 hours, the system marks it as 'Late' and sends a reminder email to the Interviewer.
*   **4.6b Unauthorized Access:** If an Interviewer attempts to view/submit feedback for an interview they are not assigned to, the system denies access.
