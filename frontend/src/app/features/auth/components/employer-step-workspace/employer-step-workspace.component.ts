import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

import { NgClass } from '@angular/common';
import { EmployerRegistrationService } from '../../services/employer-registration.service';

@Component({
  selector: 'app-employer-step-workspace',
  standalone: true,
  imports: [ReactiveFormsModule, NgClass],
  templateUrl: './employer-step-workspace.component.html',
  styleUrl: './employer-step-workspace.component.scss',
})
export class EmployerStepWorkspaceComponent implements OnInit {
  private fb = inject(FormBuilder);
  private employerService = inject(EmployerRegistrationService);

  workspaceForm!: FormGroup;
  hasManuallyEditedUrl = false;

  ngOnInit() {
    const existing = this.employerService.profile();
    this.workspaceForm = this.fb.group({
      name: [existing?.workspaceName || '', Validators.required],
      url: [existing?.workspaceUrl || '', Validators.required],
    });

    this.workspaceForm.get('name')?.valueChanges.subscribe((name) => {
      if (name && !this.hasManuallyEditedUrl) {
        const slug = name
          .toLowerCase()
          .replace(/[^a-z0-9]+/g, '-')
          .replace(/(^-|-$)+/g, '');
        this.workspaceForm.get('url')?.setValue(slug, { emitEvent: false });
      }
    });
    this.workspaceForm.valueChanges.subscribe((v) => {
      this.employerService.setStepValid(2, this.workspaceForm.valid);
      this.employerService.updateProfile({
        workspaceName: v.name,
        workspaceUrl: v.url,
      });
    });
  }

  onUrlEdit() {
    this.hasManuallyEditedUrl = true;
  }

  field(name: string) {
    return this.workspaceForm.get(name);
  }
}