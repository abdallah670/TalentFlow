# TalentFlow – Design-to-Angular Component Generation Plan

> Master checklist. Work top-to-bottom: **Candidates → Employer/Recruiter → Admin → Shared → Global setup**.
> Source designs: `design/<folder>/code.html` (+ `screen.png` reference).
> Target app: `frontend/src/app/presentation/`.

## Rules for every generated component

1. **TS (`<name>.component.ts`)** – minimal only, NO logic:

   ```ts
   import { Component } from '@angular/core';

   @Component({
     selector: 'app-<kebab-name>',
     standalone: true,
   templateUrl: './<name>.component.html',
     styleUrl: './<name>.component.scss',
   })
   export class <Name>Component {}
   ```

2. **HTML (`<name>.component.html`)** – markup taken from the design's `<body>` inner content:
   - Replace Tailwind utility classes with semantic custom classes (e.g. `tf-page`, `tf-card`, `job-card__title`).
   - Use Bootstrap utility classes where convenient (spacing/flex/grid).
   - Keep Material Symbols icons (`<span class="material-symbols-outlined">icon_name</span>`).
3. **SCSS (`<name>.component.scss`)** – ALL styles here, real SCSS (nesting allowed), using design tokens from `_variables.scss`. No inline `style=""` unless trivially dynamic.
4. **Shared blocks** that repeat across designs (navbar, sidebars, modals, footers, step indicators) go in `presentation/components/shared/<name>/`.
5. Skip duplicates: `_standardized*`, `_mobile`, `_animated`, `_1/_2/_3` variants of the same screen.
6. No routes, no services, no forms/logic in this pass.

---

## Phase 0 – Global setup (do first)
- [x] `frontend/src/app/presentation/components/shared/_variables.scss` – design tokens:
  `$primary:#24389c; $primary-container:#3f51b5; $secondary:#565c84; $surface:#fbf8ff;`
  `$on-surface:#1a1b22; $outline:#757684; $outline-variant:#c5c5d4; $error:#ba1a1a;` etc.
- [x] `styles.scss` – already has full MD3 token set (`:root` CSS variables) – no change needed.
- [x] `index.html` – already links Material Symbols Outlined (+ Roboto loaded via styles.scss).

## Phase 1 – Candidate portal pages (`presentation/pages/candidate/`) — FIRST PRIORITY
| Done | Design folder | Component path |
|---|---|---|
| [x] | account_verified_talentflow | candidate/account-verified |
| [x] | assessment_results_arthur_pendragon_talentflow | candidate/assessment-results |
| [x] | technical_assessment_senior_backend_engineer_talentflow | candidate/technical-assessment |
| [ ] | candidate_profile_arthur_pendragon_talentflow | candidate/profile |
| [ ] | candidate_comparison_matrix_talentflow | candidate/comparison-matrix |
| [ ] | candidate_help_center_talentflow | candidate/help-center |
| [ ] | candidate_interview_prep_guide_talentflow | candidate/interview-prep-guide |
| [ ] | interview_preparation_guide_talentflow_candidate_portal (dup pair – pick one) | candidate/interview-preparation |
| [ ] | interview_hub_talentflow_candidate_portal | candidate/interview-hub |
| [ ] | schedule_your_interview_talentflow_candidate_portal | candidate/schedule-interview |
| [ ] | manage_availability / manage_my_availability (dup pair – pick one) | candidate/manage-availability |
| [ ] | candidate_onboarding_checklist_talentflow | candidate/onboarding-checklist |

## Phase 2 – Employer / Recruiter pages

### Dashboard & jobs
| Done | Design folder | Component |
|---|---|---|
| [ ] | dashboard_talentflow | dashboard/dashboard (**reference implementation – split fully html+scss**) |
| [ ] | recruiter_productivity_dashboard_talentflow | dashboard/recruiter-productivity |
| [ ] | hiring_manager_action_center_talentflow | dashboard/hiring-manager-action-center |
| [ ] | jobs_talentflow | jobs/jobs-list |
| [ ] | create_new_job_talentflow | jobs/create-job |
| [ ] | pipeline_senior_software_engineer_talentflow | jobs/pipeline |
| [ ] | pipeline_template_manager_talentflow | jobs/pipeline-template-manager |
| [ ] | talent_pool_talentflow | candidates/talent-pool |
| [ ] | offers_talentflow | offers/offers-list |
| [ ] | analytics_talentflow_1/_2 (pick one) | analytics/analytics |

### Candidates ops / interviews / assessments (`presentation/pages/`)
| Done | Design folder | Component |
|---|---|---|
| [ ] | import_candidates_talentflow | candidates/import-candidates |
| [ ] | move_candidate_talentflow | candidates/move-candidate |
| [ ] | batch_actions_talentflow_recruiter_portal | candidates/batch-actions |
| [ ] | candidate_crm_re_engagement_talentflow | candidates/crm-re-engagement |
| [ ] | interview_feedback_talentflow | interviews/interview-feedback |
| [ ] | interview_feedback_comparison_talentflow | interviews/feedback-comparison |
| [ ] | schedule_interview_talentflow | interviews/schedule-interview |
| [ ] | assessment_builder_talentflow | assessments/assessment-builder |

### Offers, referrals, help, misc employer pages
| Done | Design folder | Component |
|---|---|---|
| [ ] | offer_detail_arthur_pendragon_talentflow | offers/offer-detail |
| [ ] | offer_negotiation_arthur_pendragon_talentflow | offers/offer-negotiation |
| [ ] | employee_referral_leaderboard_talentflow | referrals/referral-leaderboard |
| [ ] | employee_referral_portal_talentflow | referrals/referral-portal |
| [ ] | internal_mobility_portal_talentflow | internal-mobility/portal |
| [ ] | hiring_manager_resource_center_talentflow | help/hiring-manager-resource-center |
| [ ] | resource_library_talentflow_recruiter_portal | help/resource-library |
| [ ] | eeo_compliance_reporting_talentflow | compliance/eeo-reporting |
| [ ] | global_compliance_calendar_talentflow | compliance/compliance-calendar |
| [ ] | select_subscription_plan_talentflow | billing/select-subscription-plan |
| [ ] | system_status_talentflow | system/system-status |
| [ ] | settings_talentflow | settings/settings |

## Phase 3 – Admin pages (`presentation/pages/admin/`) — LAST PRIORITY
| Done | Design folder | Component |
|---|---|---|
| [ ] | user_management_talentflow | admin/user-management |
| [ ] | audit_log_talentflow | admin/audit-log |
| [ ] | security_permissions_talentflow_admin | admin/security-permissions |
| [ ] | billing_subscription_talentflow_admin | admin/billing-subscription |
| [ ] | agency_management_portal_talentflow_admin | admin/agency-management |
| [ ] | vendor_agency_portal_talentflow | admin/vendor-agency-portal |
| [ ] | vendor_management_system_vms_talentflow_admin | admin/vendor-management-vms |
| [ ] | vendor_portal_configuration_talentflow_admin | admin/vendor-portal-config |
| [ ] | integration_marketplace_talentflow_admin_1/_2 (one) | admin/integration-marketplace |
| [ ] | integration_health_monitor_talentflow_admin | admin/integration-health |
| [ ] | communication_integrations_talentflow_admin | admin/communication-integrations |
| [ ] | hiring_team_integrations_talentflow_admin | admin/hiring-team-integrations |
| [ ] | advanced_talent_analytics_talentflow_intelligence | admin/talent-analytics |
| [ ] | executive_reporting_suite_talentflow_intelligence | admin/executive-reporting |
| [ ] | custom_report_builder_talentflow_admin | admin/custom-report-builder |
| [ ] | rejection_analytics_talentflow_intelligence | admin/rejection-analytics |
| [ ] | global_talent_benchmarking_talentflow_intelligence | admin/talent-benchmarking |
| [ ] | dei_reporting_dashboard_talentflow_admin_1/_2 (one) | admin/dei-reporting |
| [ ] | employee_referral_management_talentflow_admin | admin/referral-management |
| [ ] | referral_payouts_talentflow_admin | admin/referral-payouts |
| [ ] | candidate_experience_surveys_talentflow_admin | admin/experience-surveys |
| [ ] | candidate_onboarding_status_talentflow_admin | admin/onboarding-status |
| [ ] | employee_offboarding_transitions_talentflow_admin | admin/offboarding |
| [ ] | recruiter_capacity_planner_talentflow_admin | admin/recruiter-capacity |
| [ ] | hiring_team_manager_talentflow_admin | admin/hiring-team-manager |
| [ ] | holiday_leave_configurator_talentflow_admin | admin/holiday-leave-config |
| [ ] | global_payroll_integration_talentflow_admin | admin/payroll-integration |
| [ ] | global_multi_entity_settings_talentflow_admin | admin/multi-entity-settings |
| [ ] | privacy_compliance_center_talentflow_admin | admin/privacy-compliance |
| [ ] | question_bank_talentflow_admin | admin/question-bank |
| [ ] | offer_approval_workflow_talentflow_admin | admin/offer-approval-workflow |
| [ ] | offer_letter_preview_finalization_talentflow_admin | admin/offer-letter-preview |
| [ ] | offer_letter_template_builder_talentflow_admin | admin/offer-letter-builder |
| [ ] | support_help_desk_talentflow_admin | admin/support-help-desk |
| [ ] | system_settings_talentflow_admin | admin/system-settings |
| [ ] | system_status_health_talentflow_admin | admin/system-health |
| [ ] | internal_audit_permission_review_talentflow_admin | admin/audit-permission-review |
| [ ] | internal_mobility_settings_talentflow_admin | admin/internal-mobility-settings |
| [ ] | career_site_editor_talentflow_admin | admin/career-site-editor |
| [ ] | developer_api_documentation_talentflow_tech | admin/api-documentation |
| [ ] | employer_welcome_dashboard_talentflow | admin/welcome-dashboard |

## Phase 4 – Shared components (`presentation/components/shared/`)
| Done | Source pattern | Component |
|---|---|---|
| [ ] | repeated top bar across portal designs | navbar |
| [ ] | dashboard/jobs/admin left menu | sidebar |
| [ ] | move-candidate / confirm dialogs | modal |
| [ ] | registration wizards | step-indicator |
| [ ] | repeated footer | footer |

## Phase 5 – Emails, auth extras & system states
| Done | Design folder | Component / action |
|---|---|---|
| [ ] | user_invite_email_talentflow | email-templates/user-invite |
| [ ] | forgot_password_talentflow | exists (`pages/auth/password/forgot-password`) – merge styles only if needed |
| [ ] | password_updated_talentflow | auth/password-updated |
| [ ] | set_up_your_account_talentflow | exists (`auth/setup-account`) |
| [ ] | select_workspace_talentflow | exists (`auth/select-workspace`) |
| [ ] | system_states_404_empty_state_talentflow | shared/not-found |
| [ ] | talentflow_enterprise_recruitment_platform | pages/home (landing) |

| [ ] | careers_in_emea_talentflow_global | career-site/careers-emea |
| [ ] | public_career_page_talentflow | career-site/public-career-page |
| [ ] | employer_registration_company_details / company_setup_1 / create_workspace_2/_3 / review / select_subscription_4 (unique steps only) | auth/employer-registration/step-1..4 |

| [ ] | offer_acceptance_portal_talentflow | candidate/offer-acceptance |
| [ ] | candidate_travel_expense_reimbursement_talentflow | candidate/travel-expense |
| [ ] | verify_email / verify_your_email (dup pair – pick one) | candidate/verify-email |
| [ ] | candidate_registration_step_1 (+standardized) | candidate/registration/step-personal-info |
| [ ] | candidate_registration_step_2 | candidate/registration/step-education |
| [ ] | candidate_registration_step_3 (+variants) | candidate/registration/step-skills |
| [ ] | candidate_registration_step_4 (+standardized) | candidate/registration/step-review |

