import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { NgClass } from '@angular/common';
import { StepIndicatorComponent } from '@shared/components/empolyer-step-indicator/step-indicator.component';
import { LayoutComponent } from '@shared/components/empolyer-layout/layout.component';
import { EmployerRegistrationService } from '@features/auth/services/employer-registration.service';

@Component({
  selector: 'app-company-setup',
  standalone: true,
  imports: [ReactiveFormsModule, StepIndicatorComponent, LayoutComponent, NgClass],
  templateUrl: './company-setup.component.html',
  styleUrl: './company-setup.component.scss',
})
export class CompanySetupComponent implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private employerService = inject(EmployerRegistrationService);

  companyForm!: FormGroup;

  ngOnInit() {
    const existing = this.employerService.profile();
    this.companyForm = this.fb.group({
      name: [existing?.companyName || '', Validators.required],
      size: [existing?.companySize || '', Validators.required],
      industry: [existing?.industry || '', Validators.required],
      website: [existing?.websiteUrl || ''],
      linkedin: [existing?.linkedinUrl || ''],
      location: [existing?.officeLocation || '', Validators.required],
    });
  }
  
  goBack() {
    this.router.navigate(['/register/employer']);
  }

  onSubmit() {
    if (this.companyForm.valid) {
      const value = this.companyForm.getRawValue();
      this.employerService.updateProfile({
        companyName: value.name,
        companySize: value.size,
        industry: value.industry,
        websiteUrl: value.website,
        linkedinUrl: value.linkedin,
        officeLocation: value.location,
      });
      this.employerService.setStepValid(3, true);
      this.router.navigate(['/register/workspace']);
    } else {
      this.companyForm.markAllAsTouched();
    }
  }
}
