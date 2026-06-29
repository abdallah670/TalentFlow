# 09-API-Design.md

# TalentFlow API Design Document

## Document Information

| Field          | Value                     |
| -------------- | ------------------------- |
| Project        | TalentFlow                |
| Version        | 1.0                       |
| API Style      | RESTful API               |
| Authentication | JWT + Refresh Tokens      |
| Architecture   | Clean Architecture + CQRS |
| Base URL       | `/api/v1`                 |

---

# 1. API Design Principles

The API follows:

* RESTful Standards
* Resource-Based Routing
* Stateless Requests
* JWT Authentication
* Role-Based Authorization
* Consistent Response Format
* API Versioning
* Pagination Support
* Filtering Support
* Sorting Support

---

# 2. Standard Response Format

## Success Response

```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": {}
}
```

---

## Validation Error

```json
{
  "success": false,
  "message": "Validation failed",
  "errors": {
    "email": [
      "Email is required"
    ]
  }
}
```

---

## Server Error

```json
{
  "success": false,
  "message": "Unexpected error occurred"
}
```

---

# 3. Authentication APIs

## Login

### Endpoint

```http
POST /api/v1/auth/login
```

### Request

```json
{
  "email": "admin@company.com",
  "password": "Password123"
}
```

### Response

```json
{
  "accessToken": "jwt-token",
  "refreshToken": "refresh-token",
  "expiresAt": "2026-01-01T00:00:00Z"
}
```

---

## Refresh Token

### Endpoint

```http
POST /api/v1/auth/refresh-token
```

### Request

```json
{
  "refreshToken": "token"
}
```

---

## Logout

### Endpoint

```http
POST /api/v1/auth/logout
```

---

## Change Password

### Endpoint

```http
POST /api/v1/auth/change-password
```

---

# 4. Tenant APIs

## Get Current Tenant

```http
GET /api/v1/tenants/current
```

---

## Update Tenant Settings

```http
PUT /api/v1/tenants/settings
```

### Request

```json
{
  "companyName": "ABC Software",
  "timeZone": "UTC",
  "primaryColor": "#1976d2"
}
```

---

# 5. User Management APIs

## Get Users

```http
GET /api/v1/users
```

### Query Parameters

```http
?page=1
&pageSize=20
&search=abdullah
```

---

## Get User By Id

```http
GET /api/v1/users/{id}
```

---

## Create User

```http
POST /api/v1/users
```

### Request

```json
{
  "firstName": "Abdullah",
  "lastName": "Mohammed",
  "email": "abdullah@test.com",
  "roleIds": [1,2]
}
```

---

## Update User

```http
PUT /api/v1/users/{id}
```

---

## Disable User

```http
PATCH /api/v1/users/{id}/disable
```

---

# 6. Role & Permission APIs

## Get Roles

```http
GET /api/v1/roles
```

---

## Create Role

```http
POST /api/v1/roles
```

---

## Assign Permissions

```http
POST /api/v1/roles/{id}/permissions
```

---

# 7. Department APIs

## Get Departments

```http
GET /api/v1/departments
```

---

## Create Department

```http
POST /api/v1/departments
```

---

## Update Department

```http
PUT /api/v1/departments/{id}
```

---

# 8. Job APIs

## Get Jobs

```http
GET /api/v1/jobs
```

### Filtering

```http
?page=1
&pageSize=20
&status=Open
&departmentId=guid
&search=.net
```

---

## Get Job Details

```http
GET /api/v1/jobs/{id}
```

---

## Create Job

```http
POST /api/v1/jobs
```

### Request

```json
{
  "title": ".NET Developer",
  "departmentId": "guid",
  "description": "Job Description",
  "salaryMin": 1000,
  "salaryMax": 3000,
  "experienceLevel": "Mid"
}
```

---

## Update Job

```http
PUT /api/v1/jobs/{id}
```

---

## Publish Job

```http
PATCH /api/v1/jobs/{id}/publish
```

---

## Close Job

```http
PATCH /api/v1/jobs/{id}/close
```

---

# 9. Candidate APIs

## Get Candidates

```http
GET /api/v1/candidates
```

### Filtering

```http
?page=1
&pageSize=20
&skill=CSharp
&experience=3
```

---

## Get Candidate

```http
GET /api/v1/candidates/{id}
```

---

## Create Candidate

```http
POST /api/v1/candidates
```

---

## Update Candidate

```http
PUT /api/v1/candidates/{id}
```

---

## Upload Resume

```http
POST /api/v1/candidates/{id}/resume
```

### Content Type

```http
multipart/form-data
```

---

## Candidate Timeline

```http
GET /api/v1/candidates/{id}/activities
```

---

# 10. Skills APIs

## Get Skills

```http
GET /api/v1/skills
```

---

## Create Skill

```http
POST /api/v1/skills
```

---

# 11. Application APIs

## Apply Candidate

```http
POST /api/v1/applications
```

### Request

```json
{
  "candidateId": "guid",
  "jobId": "guid"
}
```

---

## Get Applications

```http
GET /api/v1/applications
```

---

## Get Application

```http
GET /api/v1/applications/{id}
```

---

## Move Candidate Stage

```http
PATCH /api/v1/applications/{id}/move-stage
```

### Request

```json
{
  "stageId": "guid",
  "notes": "Passed screening"
}
```

---

## Reject Candidate

```http
PATCH /api/v1/applications/{id}/reject
```

---

## Hire Candidate

```http
PATCH /api/v1/applications/{id}/hire
```

---

# 12. Pipeline APIs

## Get Pipelines

```http
GET /api/v1/pipelines
```

---

## Create Pipeline

```http
POST /api/v1/pipelines
```

---

## Add Stage

```http
POST /api/v1/pipelines/{id}/stages
```

---

## Reorder Stages

```http
PATCH /api/v1/pipelines/{id}/stages/reorder
```

---

# 13. Interview APIs

## Schedule Interview

```http
POST /api/v1/interviews
```

### Request

```json
{
  "applicationId": "guid",
  "scheduledAt": "2026-01-01T10:00:00",
  "durationMinutes": 60,
  "participantIds": []
}
```

---

## Get Interviews

```http
GET /api/v1/interviews
```

---

## Get Interview Details

```http
GET /api/v1/interviews/{id}
```

---

## Submit Feedback

```http
POST /api/v1/interviews/{id}/feedback
```

### Request

```json
{
  "recommendation": "Hire",
  "summary": "Strong technical skills",
  "scores": [
    {
      "criteriaId": "guid",
      "score": 5
    }
  ]
}
```

---

# 14. Assessment APIs

## Create Assessment

```http
POST /api/v1/assessments
```

---

## Get Assessments

```http
GET /api/v1/assessments
```

---

## Add Question

```http
POST /api/v1/assessments/{id}/questions
```

---

## Assign Assessment

```http
POST /api/v1/assessment-assignments
```

---

## Submit Answers

```http
POST /api/v1/assessment-submissions
```

---

## Get Results

```http
GET /api/v1/assessment-results/{id}
```

---

# 15. Notification APIs

## Get Notifications

```http
GET /api/v1/notifications
```

---

## Mark As Read

```http
PATCH /api/v1/notifications/{id}/read
```

---

## Mark All As Read

```http
PATCH /api/v1/notifications/read-all
```

---

# 16. Dashboard APIs

## Dashboard Summary

```http
GET /api/v1/dashboard/summary
```

### Response

```json
{
  "openJobs": 25,
  "activeCandidates": 300,
  "scheduledInterviews": 15,
  "offersSent": 8
}
```

---

## Recruitment Funnel

```http
GET /api/v1/dashboard/funnel
```

---

## Hiring Analytics

```http
GET /api/v1/dashboard/analytics
```

---

# 17. Audit APIs

## Get Audit Logs

```http
GET /api/v1/audit-logs
```

### Filters

```http
?entity=Job
?action=Update
&userId=guid
```

---

# 18. Search APIs

## Global Search

```http
GET /api/v1/search
```

### Example

```http
?query=abdullah
```

### Searches

* Jobs
* Candidates
* Interviews
* Assessments

---

# 19. Validation Rules

## User

| Field     | Rule              |
| --------- | ----------------- |
| FirstName | Required, Max 100 |
| LastName  | Required, Max 100 |
| Email     | Valid Email       |
| Password  | Min 8 Characters  |

---

## Job

| Field        | Rule     |
| ------------ | -------- |
| Title        | Required |
| DepartmentId | Required |
| Description  | Required |
| SalaryMin    | >= 0     |

---

## Candidate

| Field     | Rule        |
| --------- | ----------- |
| FirstName | Required    |
| LastName  | Required    |
| Email     | Valid Email |

---

# 20. HTTP Status Codes

| Code | Meaning               |
| ---- | --------------------- |
| 200  | Success               |
| 201  | Created               |
| 204  | No Content            |
| 400  | Validation Error      |
| 401  | Unauthorized          |
| 403  | Forbidden             |
| 404  | Not Found             |
| 409  | Conflict              |
| 500  | Internal Server Error |

---

# 21. Pagination Standard

All list endpoints support:

```http
?page=1
&pageSize=20
```

Response:

```json
{
  "items": [],
  "page": 1,
  "pageSize": 20,
  "totalCount": 100,
  "totalPages": 5
}
```

---

# 22. API Versioning Strategy

Versioning via URL:

```http
/api/v1/jobs
/api/v2/jobs
```

Benefits:

* Backward compatibility
* Safe future upgrades
* Easier client migration

---

# 23. Future APIs

## AI Resume Parsing

```http
POST /api/v1/ai/parse-resume
```

---

## Candidate Ranking

```http
GET /api/v1/ai/rank-candidates/{jobId}
```

---

## Job Matching

```http
GET /api/v1/ai/match-job/{candidateId}
```

---

# API Design Decisions

| Decision           | Reason                 |
| ------------------ | ---------------------- |
| REST APIs          | Industry Standard      |
| JWT Authentication | Stateless Security     |
| URL Versioning     | Compatibility          |
| Pagination         | Performance            |
| Filtering          | Scalability            |
| Standard Responses | Consistency            |
| CQRS Backend       | Separation of Concerns |

This API design provides a scalable, production-grade contract between the Angular frontend and ASP.NET Core backend and supports future SaaS and AI expansion.
