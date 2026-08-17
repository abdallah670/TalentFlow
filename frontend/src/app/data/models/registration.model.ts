export interface Step {
  id: number;
  label: string;
  title: string;
}

export interface CandidateRegistrationRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
  phoneNumber?: string;
  currentJobTitle?: string;
  currentCompany?: string;
  totalYearsOfExperience?: number;
  linkedinUrl?: string;
  portfolioUrl?: string;
  skills?: string[];
  coverLetter?: string;
  preferredLocation?: string;
  remoteOnly?: boolean;
  salaryExpectationMin?: number;
  salaryExpectationMax?: number;
  currency?: string;
  availableFrom?: string;
  workAuthorization?: string;
}

export interface EmployerRegistrationRequest {
  firstName: string;
  lastName: string;
  email: string;
  companyName: string;
  password: string;
  confirmPassword: string;
  companySize?: string;
  industry?: string;
  websiteUrl?: string;
  linkedinUrl?: string;
  officeLocation?: string;
  selectedPlan?: string;
  roleType: string;
  otherRoleDetail: string;
  workspaceName?: string;
  workspaceUrl?: string;
}
export interface TenantInfo {
  id: string;
  name: string;
  logo: string;
  role: string;
  jobCount: number;
}


export interface AcceptInvitationRequest {
  token: string;
  password: string;
  confirmPassword: string;
}