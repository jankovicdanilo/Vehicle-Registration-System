import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-insurance-form',
  templateUrl: './insurance-form.component.html',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
})

export class InsuranceFormComponent {
  @Input() insurance: { id: string; naziv: string };
  @Input() insuranceId: string;
  insuranceForm: FormGroup;
  @Output() insuranceAdded = new EventEmitter<void>();
  @Output() insuranceChanged = new EventEmitter<void>();

  constructor(
    private fb: FormBuilder
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['insurance'] && this.insuranceForm && this.insurance) {
      this.insuranceForm.patchValue(this.insurance);
    }
  }

  ngOnInit(): void {
    this.insuranceForm = this.fb.group({
      naziv: ["", Validators.required],
    });
  }

  onSave() {
    this.insuranceId ? this.onEditInsurance() : this.onAddNewInsurance();
  }

  onEditInsurance() {
    this.insuranceChanged.emit(this.insuranceForm.value);
  }

  onAddNewInsurance() {
    if (this.insuranceForm.valid) {
      this.insuranceAdded.emit(this.insuranceForm.value);
    }
  }
}