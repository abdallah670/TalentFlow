export interface AuthRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  id: string;
  userName: string;
  email: string;
  token: string;
  refreshToken?: string;
}

export interface RegistrationRequest {
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  password: string;
}

export interface RegistrationResponse {
  userId: string;
  token?: string;
  refreshToken?: string;
  email: string;
  userName: string;
  requiresEmailVerification?: boolean;
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
}

export interface VerifyEmailRequest {
  email: string;
  token: string;
}

export interface ResendVerificationRequest {
  email: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  email: string;
  token: string;
  newPassword: string;
  confirmPassword: string;
}

export interface AcceptInvitationRequest {
  token: string;
  password: string;
  confirmPassword: string;
}

export interface TenantInfo {
  id: string;
  name: string;
  logo: string;
  role: string;
  jobCount: number;
}
