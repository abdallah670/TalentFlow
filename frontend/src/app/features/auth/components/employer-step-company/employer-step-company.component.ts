import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

import { NgClass } from '@angular/common';
import { EmployerRegistrationService } from '../../services/employer-registration.service';

@Component({
  selector: 'app-employer-step-company',
  standalone: true,
  imports: [ReactiveFormsModule, NgClass],
  templateUrl: './employer-step-company.component.html',
  styleUrl: './employer-step-company.component.scss',
})
export class EmployerStepCompanyComponent implements OnInit {
  private fb = inject(FormBuilder);
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

    this.companyForm.valueChanges.subscribe((v) => {
      this.employerService.setStepValid(1, this.companyForm.valid);
      this.employerService.updateProfile({
        companyName: v.name,
        companySize: v.size,
        industry: v.industry,
        websiteUrl: v.website,
        linkedinUrl: v.linkedin,
        officeLocation: v.location,
      });
    });
  }

  field(name: string) {
    return this.companyForm.get(name);
  }
}