import { Component, EventEmitter, Input, Output, SimpleChanges } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatNativeDateModule } from "@angular/material/core";
import { CommonModule } from "@angular/common";
import { Client } from '../../../../core/models/client.model';

@Component({
  selector: "app-client-form",
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
  ],
  templateUrl: "./client-form.component.html",
})
export class ClientFormComponent {

  @Input() clientId!: string;
  @Input() client!: Client;

  clientForm!: FormGroup;

  @Output() clientAdded = new EventEmitter<any>();
  @Output() clientChanged = new EventEmitter<any>();

  constructor(private fb: FormBuilder) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['client'] && this.clientForm && this.client) {
      this.clientForm.patchValue(this.client);
    }
  }

  ngOnInit(): void {
    this.clientForm = this.fb.group({
      firstName: ["", Validators.required],
      lastName: ["", Validators.required],
      nationalId: ["", [Validators.required, Validators.pattern(/^\d{13}$/)]],
      address: ["", Validators.required],
      phoneNumber: [""],
      email: ["", Validators.email],
      dateOfBirth: [""],
      idCardNumber: ["", [Validators.required, Validators.pattern(/^\d{6,9}$/)]],
    });
  }

  onSave(): void {
    this.clientId ? this.onEditClient() : this.onAddNewClient();
  }

  onEditClient(): void {
    this.clientChanged.emit(this.clientForm.value);
  }

  onAddNewClient(): void {
    if (this.clientForm.valid) {
      this.clientAdded.emit(this.clientForm.value);
    }
  }
}