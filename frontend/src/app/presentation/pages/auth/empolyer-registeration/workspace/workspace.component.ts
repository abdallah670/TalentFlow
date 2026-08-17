import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { NgClass } from '@angular/common';
import { StepIndicatorComponent } from '../../../../components/shared/empolyer-step-indicator/step-indicator.component';
import { LayoutComponent } from '../../../../components/shared/empolyer-layout/layout.component';
import { EmployerRegistrationService } from '../../../../../core/services/employer-registration.service';


@Component({
  selector: 'app-workspace',
  standalone: true,
  imports: [ReactiveFormsModule, StepIndicatorComponent, LayoutComponent, NgClass],
  templateUrl: './workspace.component.html',
  styleUrl: './workspace.component.scss',
})
export class WorkspaceComponent implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private employerService = inject(EmployerRegistrationService);

  workspaceForm!: FormGroup;
  hasManuallyEditedUrl = false;

  ngOnInit() {
    const existing = this.employerService.profile();
    this.workspaceForm = this.fb.group({
      name: [existing?.workspaceName || '', Validators.required],
      url: [existing?.workspaceUrl || '', Validators.required],
    });

    this.workspaceForm.get('name')?.valueChanges.subscribe(name => {
      if (name && !this.hasManuallyEditedUrl) {
        const slug = name.toLowerCase().replace(/[^a-z0-9]+/g, '-').replace(/(^-|-$)+/g, '');
        this.workspaceForm.get('url')?.setValue(slug, { emitEvent: false });
      }
    });
  }

  onUrlEdit() {
    this.hasManuallyEditedUrl = true;
  }

  goBack() {
    this.router.navigate(['/register/company-setup']);
  }

  onSubmit() {
    if (this.workspaceForm.valid) {
      const value = this.workspaceForm.getRawValue();
      this.employerService.updateProfile({
        workspaceName: value.name,
        workspaceUrl: value.url,
      });
      this.employerService.setStepValid(4, true);
      this.router.navigate(['/register/subscription']);
    } else {
      this.workspaceForm.markAllAsTouched();
    }
  }
}
