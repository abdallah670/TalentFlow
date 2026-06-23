# TalentFlow — User Stories

> **Document Version:** 1.0  
> **Date:** June 2026  
> **Status:** Approved for Development

---

## 1. Epic List

- **Epic 1: Authentication and Authorization** - Secure access to the platform for different types of users.
- **Epic 2: Job Management** - Lifecycle of a job posting from creation to closing.
- **Epic 3: Candidate Management** - Sourcing, parsing, and storing candidate information.
- **Epic 4: Application Pipeline** - Tracking candidates through the hiring stages.
- **Epic 5: Interview Management** - Scheduling and conducting interviews.
- **Epic 6: Technical Assessments** - Evaluating candidate skills through tests.
- **Epic 7: Evaluation and Feedback** - Collecting structured feedback from interviewers.
- **Epic 8: Analytics and Reporting** - Providing insights into the hiring process.
- **Epic 9: AI Integrations** - Enhancing workflows with artificial intelligence.
- **Epic 10: Administration and Configuration** - Managing the platform settings and users.

---

## 2. User Stories & Acceptance Criteria

### Epic 1: Authentication and Authorization

**US-1.1: User Registration**
* **Story:** As an HR Specialist, I want to register for a new account using my corporate email so that I can start using the platform to manage hiring.
* **Acceptance Criteria:**
    * **Given** the user is on the registration page, **When** they submit a valid email, strong password, and company details, **Then** an account is created in a 'Pending Verification' state.
    * **Given** an account is created, **When** the system processes the registration, **Then** a verification email with a secure link is sent to the provided email address.
    * **Given** a user has a verification link, **When** they click the link within 24 hours, **Then** their account status changes to 'Active'.
    * **Given** a user submits an email that is already registered, **When** they click submit, **Then** the system displays an appropriate error message and prevents duplicate registration.

**US-1.2: User Login**
* **Story:** As a registered user, I want to log in securely so that I can access my dashboard and role-specific features.
* **Acceptance Criteria:**
    * **Given** a user is on the login page, **When** they enter valid credentials (email and password), **Then** they are authenticated, issued a JWT access token and refresh token, and redirected to their dashboard.
    * **Given** a user enters invalid credentials, **When** they attempt to log in, **Then** they receive a generic "Invalid email or password" error.
    * **Given** a user attempts to log in with an unverified account, **When** they click submit, **Then** they are prompted to verify their email address.
    * **Given** a user fails to log in 5 consecutive times, **When** they make a 6th attempt, **Then** their account is locked for 15 minutes and an alert email is sent.

**US-1.3: Role-Based Access Control**
* **Story:** As an Administrator, I want to assign roles to users so that they only have access to features and data appropriate for their job function.
* **Acceptance Criteria:**
    * **Given** an Administrator is viewing a user's profile, **When** they select a role (HR, Recruiter, Hiring Manager, Interviewer) from a dropdown and save, **Then** the user's role is updated in the database.
    * **Given** a user with the 'Interviewer' role logs in, **When** they navigate the platform, **Then** they can only view interviews assigned to them and cannot access system settings or create jobs.
    * **Given** an unauthorized API request is made, **When** the backend receives the request, **Then** it returns a `403 Forbidden` status code.

### Epic 2: Job Management

**US-2.1: Create a Job Posting**
* **Story:** As an HR Specialist, I want to create a new job posting specifying details like title, description, and required skills so that I can start accepting applications.
* **Acceptance Criteria:**
    * **Given** the HR Specialist is on the 'Create Job' page, **When** they fill in all required fields (Title, Description, Department, Location, Job Type, Required Skills) and click 'Save as Draft', **Then** the job is saved with a status of 'Draft'.
    * **Given** the user is creating a job, **When** they leave a required field empty and attempt to save, **Then** validation errors are displayed next to the respective fields.
    * **Given** the user is creating a job, **When** they select 'Required Skills', **Then** they can either choose from an existing taxonomy of skills or add new custom skills.

**US-2.2: Publish a Job Posting**
* **Story:** As an HR Specialist, I want to publish a draft job posting so that candidates can view it and apply.
* **Acceptance Criteria:**
    * **Given** a job is in the 'Draft' state, **When** the HR Specialist clicks 'Publish', **Then** the job status changes to 'Active' and it becomes visible on the public careers page.
    * **Given** a job is being published, **When** the action is successful, **Then** the system records the 'PublishedAt' timestamp.

**US-2.3: AI Job Description Generation**
* **Story:** As an HR Specialist, I want the system to generate a job description based on a job title and a few keywords so that I can save time drafting it manually.
* **Acceptance Criteria:**
    * **Given** the user is creating a job, **When** they enter a Job Title and a list of keywords and click 'Generate Description', **Then** the AI service returns a professionally formatted job description within 5 seconds.
    * **Given** the AI generates a description, **When** it is displayed to the user, **Then** the user can manually edit the text before saving the job.

### Epic 3: Candidate Management

**US-3.1: Resume Upload and Parsing**
* **Story:** As a Candidate, I want to upload my resume so that the system can automatically extract my details and save me time filling out forms.
* **Acceptance Criteria:**
    * **Given** a Candidate is applying for a job, **When** they upload a PDF or DOCX file (under 10MB), **Then** the file is securely stored in Azure Blob Storage.
    * **Given** a resume is uploaded, **When** the system processes it, **Then** the AI parser extracts the candidate's First Name, Last Name, Email, Phone, Skills, and Experience.
    * **Given** the data is parsed, **When** the application form is displayed, **Then** the extracted data pre-fills the respective form fields, allowing the candidate to review and edit them.

**US-3.2: View Candidate Profile**
* **Story:** As a Recruiter, I want to view a comprehensive candidate profile so that I can assess their suitability for open roles.
* **Acceptance Criteria:**
    * **Given** a Recruiter clicks on a candidate's name, **When** the profile loads, **Then** it displays contact info, parsed experience, education, skills, and links to download their original resume.
    * **Given** the profile is displayed, **When** the Recruiter navigates to the 'Applications' tab, **Then** they can see all jobs the candidate has applied for and their current pipeline stage for each.

**US-3.3: AI Match Scoring**
* **Story:** As a Recruiter, I want to see an AI-generated match score between a candidate and a job so that I can quickly prioritize top applicants.
* **Acceptance Criteria:**
    * **Given** a candidate applies to a job, **When** the application is received, **Then** the system calculates a match score (0-100) based on skills, experience, and job requirements.
    * **Given** a Recruiter is viewing the list of applicants for a job, **When** the list loads, **Then** the AI Match Score is displayed for each candidate and the list can be sorted by this score.

### Epic 4: Application Pipeline

**US-4.1: View Kanban Board**
* **Story:** As a Recruiter, I want to view a Kanban board of all applications for a specific job so that I can visualize the hiring pipeline.
* **Acceptance Criteria:**
    * **Given** a Recruiter selects a job, **When** they navigate to the 'Pipeline' view, **Then** a Kanban board is displayed with columns representing the defined stages for that job (e.g., Applied, Screened, Interview, Offer).
    * **Given** the Kanban board is loaded, **When** there are applications in a stage, **Then** they are displayed as cards showing the candidate's name, match score, and time in current stage.

**US-4.2: Move Candidate Between Stages**
* **Story:** As a Recruiter, I want to drag and drop a candidate's application from one stage to another so that I can easily update their progress.
* **Acceptance Criteria:**
    * **Given** a Recruiter is viewing the Kanban board, **When** they drag a candidate card from 'Applied' to 'Phone Screen', **Then** the application's stage is updated in the database.
    * **Given** an application stage is changed, **When** the database update is successful, **Then** an entry is added to the application's audit log detailing the change, the user who made it, and the timestamp.
    * **Given** an application stage is changed, **When** the new stage has automated actions configured (e.g., send email to candidate), **Then** those actions are triggered asynchronously.

### Epic 5: Interview Management

**US-5.1: Schedule an Interview**
* **Story:** As a Recruiter, I want to schedule an interview with a candidate and assign interviewers so that everyone knows when the meeting will take place.
* **Acceptance Criteria:**
    * **Given** a Recruiter is viewing an application, **When** they click 'Schedule Interview', **Then** a modal opens allowing them to input Date, Time, Duration, Interview Type, Format (Video/On-site), and select Interviewers.
    * **Given** the interview details are submitted, **When** the system saves the record, **Then** calendar invitations are emailed to the candidate and the selected interviewers.
    * **Given** the interview is scheduled, **When** the application is viewed, **Then** the upcoming interview details are visible in the activity timeline.

**US-5.2: AI Interview Question Generation**
* **Story:** As an Interviewer, I want the system to suggest relevant interview questions based on the candidate's profile and the job description so that I can conduct a more effective and targeted interview.
* **Acceptance Criteria:**
    * **Given** an Interviewer is viewing a scheduled interview, **When** they click 'Generate Suggested Questions', **Then** the AI service analyzes the Job Description and the Candidate's Resume.
    * **Given** the AI analysis is complete, **When** the results are returned, **Then** the UI displays 8-12 tailored questions categorized into Technical, Behavioral, and Cultural.

### Epic 6: Technical Assessments

**US-6.1: Create Assessment Template**
* **Story:** As an HR Specialist, I want to create reusable assessment templates containing multiple-choice and coding questions so that I can standardize technical evaluations.
* **Acceptance Criteria:**
    * **Given** the user is in the 'Assessments' module, **When** they create a new template, **Then** they can define a Title, Description, Total Time Limit, and Passing Score.
    * **Given** a template is being created, **When** the user adds questions, **Then** they can specify the question type (MCQ or Coding), the text, the correct answers (for MCQ), and the points assigned.

**US-6.2: Assign Assessment to Candidate**
* **Story:** As a Recruiter, I want to send an assessment link to a candidate so that they can complete the technical screening.
* **Acceptance Criteria:**
    * **Given** a Recruiter views a candidate's application, **When** they select 'Send Assessment' and choose a template, **Then** a unique, time-sensitive link is generated.
    * **Given** the link is generated, **When** the action completes, **Then** an email with the link and instructions is sent to the candidate.

**US-6.3: Auto-Grading MCQ Assessments**
* **Story:** As a Recruiter, I want the system to automatically grade multiple-choice questions so that I get immediate results without manual review.
* **Acceptance Criteria:**
    * **Given** a Candidate submits an assessment, **When** the system processes it, **Then** it compares the selected options for MCQ questions against the defined correct answers.
    * **Given** the grading is complete, **When** the Recruiter views the results, **Then** an 'Auto-Score' is displayed, and the assessment status is updated appropriately.

### Epic 7: Evaluation and Feedback

**US-7.1: Submit Interview Feedback**
* **Story:** As an Interviewer, I want to submit structured feedback and rate a candidate after an interview so that my evaluation is recorded for the hiring manager.
* **Acceptance Criteria:**
    * **Given** an Interviewer has completed a scheduled interview, **When** they access the feedback form, **Then** they must rate the candidate on predefined criteria (1-5 scale) and provide a final recommendation (e.g., Hire, No Hire).
    * **Given** the feedback form is submitted, **When** the system saves it, **Then** the feedback becomes immutable for the Interviewer and visible to the Hiring Manager and Recruiter.

**US-7.2: AI Feedback Summarization**
* **Story:** As a Hiring Manager, I want the system to summarize the feedback from all interviewers so that I can quickly understand the consensus and make a hiring decision.
* **Acceptance Criteria:**
    * **Given** multiple interviewers have submitted feedback for a candidate, **When** the Hiring Manager views the candidate's evaluation summary, **Then** an AI-generated paragraph is displayed that highlights strengths, weaknesses, and the overall consensus recommendation.

### Epic 8: Analytics and Reporting

**US-8.1: View Hiring Dashboard**
* **Story:** As an HR Specialist, I want to view a dashboard with key metrics like time-to-hire and active candidates so that I can monitor the health of the recruitment process.
* **Acceptance Criteria:**
    * **Given** an HR Specialist navigates to the Dashboard, **When** the page loads, **Then** widgets display Total Active Jobs, Total Candidates, Open Applications, and Average Time-to-Hire.
    * **Given** the dashboard is loaded, **When** viewing the 'Pipeline Funnel' chart, **Then** it accurately reflects the conversion rates between pipeline stages for the selected time period.

### Epic 9: AI Integrations

*(Note: AI-specific stories are integrated within the epics above where they provide value to the user flow, such as US-2.3, US-3.1, US-3.3, US-5.2, and US-7.2. This ensures AI is treated as a feature enhancement rather than an isolated module.)*

### Epic 10: Administration and Configuration

**US-10.1: View Audit Logs**
* **Story:** As an Administrator, I want to view system audit logs so that I can monitor user activity and ensure compliance.
* **Acceptance Criteria:**
    * **Given** an Administrator navigates to the 'Audit Logs' page, **When** the page loads, **Then** a paginated table displays all system events, including Timestamp, User, Action performed, Entity affected, and IP Address.
    * **Given** the audit logs are displayed, **When** the Administrator applies filters (e.g., by User or Date Range), **Then** the table updates to show only matching records.
