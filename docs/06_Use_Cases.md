# 6. Use Cases

## UC-01: Create Job Requisition
**Actors:** Recruiter, Hiring Manager  
**Preconditions:** User authenticated and has permission `job.create`.  
**Main Flow:**
1. User clicks "New Job" from the dashboard.
2. System displays form with fields: title (required), description, department (dropdown), location, employment type (dropdown), salary range, custom fields (dynamic key‑value), and pipeline template selector (pre‑populated with tenant’s templates).
3. User fills in details and selects a pipeline template.
4. User submits.
5. System validates inputs: title max 200 chars, department required, template exists and belongs to tenant.
6. System creates a new `Job` entity with `TenantId` from context, `Status = Draft`, `CreatedBy = currentUserId`, stores to DB.
7. Audit log entry created ("JobCreated").
8. System returns job details with ID, status 201.
**Alternative Flows:**
- 5a. Validation fails: return 422 with error details.
- 6a. User can save as "Draft" or "Open" directly.
**Postconditions:** Job created; audit logged. Job appears in job list.

## UC-02: Move Candidate Stage
**Actors:** Recruiter (primary), Hiring Manager (for certain stages).  
**Preconditions:** Candidate has an active application; user has permission for the target transition.  
**Main Flow:**
1. Recruiter opens the application pipeline board or candidate detail view.
2. Recruiter selects target stage (e.g., "Technical Interview") from allowed next stages.
3. System checks if the transition is allowed by the pipeline template and if user has the required permission (`candidate.move.stage.technicalinterview`).
4. System checks if mandatory fields are needed: if the current stage requires feedback/interview score, system prompts a modal to fill them before proceeding.
5. Recruiter confirms, optionally adds a comment.
6. System creates a `StageTransition` record: `ApplicationId`, `FromStage = current`, `ToStage = target`, `TransitionedByUserId`, `TransitionedAt`, `Comment`.
7. Updates `Application.CurrentStageId` (or stage name) and `Application.UpdatedAt`.
8. Raises `CandidateMovedToStageEvent` domain event.
9. MediatR handlers: (a) Create in‑app `Notification` for next responsible person, (b) Queue email via Hangfire, (c) Audit log entry written.
**Alternative Flows:**
- 3a. Transition not allowed: return error "Invalid transition".
- 4a. Mandatory feedback missing: redirect to feedback form; after submission, transition proceeds.
**Postconditions:** Candidate stage updated; history recorded; notifications sent.

## UC-03: Schedule Interview
**Actors:** Recruiter  
**Preconditions:** Candidate application in a stage that allows interview scheduling (configured).  
**Main Flow:**
1. Recruiter opens candidate detail, clicks "Schedule Interview".
2. System presents form: date/time picker, location/URL, interview type (phone, video, onsite), and multi‑select of interviewers (users with role Interviewer or HiringManager).
3. Recruiter submits.
4. System creates `Interview` entity linked to application, sets status "Scheduled".
5. Notifications sent to all assigned interviewers (in‑app and email).
6. Audit log entry: "InterviewScheduled".
**Postconditions:** Interview scheduled; interviewers notified.

## UC-04: Submit Interview Feedback
**Actors:** Interviewer  
**Preconditions:** Interview exists with status "Scheduled"; user is assigned interviewer.  
**Main Flow:**
1. Interviewer opens "My Interviews" or clicks notification link.
2. System displays feedback form: score (1‑5), strengths (text), weaknesses (text), recommendation (Hire/No‑Hire/Strong Hire).
3. Interviewer fills and submits.
4. System creates `InterviewFeedback` record linked to interview.
5. Interview status changes to "Completed".
6. Notification sent to Hiring Manager (candidate feedback ready).
7. Audit log entry: "FeedbackSubmitted".
**Postconditions:** Feedback stored; interview completed.

## UC-05: Generate and Approve Offer
**Actors:** Recruiter, Approver(s)  
**Preconditions:** Candidate has passed final stage of pipeline; user has `offer.create` permission.  
**Main Flow:**
1. Recruiter opens candidate application, clicks "Generate Offer".
2. System checks candidate eligibility (all mandatory stages passed, no existing offer in progress).
3. Recruiter fills offer form: salary, start date, benefits, expiration.
4. System creates `Offer` with status "Draft".
5. Recruiter submits for approval.
6. System changes status to "PendingApproval", creates `OfferApproval` records for each configured approver.
7. Notifications sent to approvers.
8. Approver opens offer, reviews, and selects "Approve" or "Reject" with comment.
9. On all approvals, status changes to "Approved"; on first rejection, "Rejected".
10. If approved, Recruiter clicks "Send to Candidate"; system sends email via Hangfire.
11. Offer status becomes "Sent"; candidate response tracked manually or via link (future).
**Postconditions:** Offer tracked with full approval history; audit logs for each status change.

## UC-06: View Analytics Dashboard
**Actors:** HR Ops, Recruiter (with `analytics.view` permission).  
**Preconditions:** Authenticated, tenant context.  
**Main Flow:**
1. User navigates to Analytics module, selects "Time‑to‑Hire" dashboard.
2. System queries aggregated data: for each month, average days between `Application.CreatedAt` and `Offer.AcceptedDate` (when status="Accepted") within the tenant.
3. Renders a line chart.
4. User can filter by department, job, date range.
5. User clicks "Export CSV"; system generates and downloads file.
**Postconditions:** No state change; analytics read‑only.