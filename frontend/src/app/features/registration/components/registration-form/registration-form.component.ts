import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { VehicleService } from '../../../../core/services/vehicle.service';
import { MessageService } from '../../../../core/services/message.service';
import { ClientService } from '../../../../core/services/client.service';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { InsuranceService } from '../../../../core/services/insurance.service';

import { Registration } from '../../../../core/models/registration.model';
import { Vehicle } from '../../../../core/models/vehicle.model';
import { Client } from '../../../../core/models/client.model';
import { Insurance } from '../../../../core/models/insurance.model';

@Component({
  selector: 'app-registration-form',
  standalone: true,
  templateUrl: './registration-form.component.html',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatSlideToggleModule,
  ],
})
export class RegistrationFormComponent {

  @Input() registrationId!: string;
  @Input() registration!: Registration;

  registrationForm!: FormGroup;

  vehicles: Vehicle[] = [];
  clients: Client[] = [];
  insurances: Insurance[] = [];

  @Output() registrationAdded = new EventEmitter<any>();
  @Output() registrationChanged = new EventEmitter<any>();

  constructor(
    private fb: FormBuilder,
    private vehicleService: VehicleService,
    private clientService: ClientService,
    private messageService: MessageService,
    private insuranceService: InsuranceService
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['registration'] && this.registrationForm && this.registration) {

      const insuranceId = this.registration.insurance?.id || null;

      this.registrationForm.patchValue({
        vehicleId: this.registration.vehicleId,
        clientId: this.registration.clientId,
        registrationDate: this.registration.registrationDate,
        isTemporary: this.registration.isTemporary,
        insuranceId: insuranceId,
        licensePlate: this.registration.licensePlate
      });
    }
  }

  ngOnInit(): void {

    this.registrationForm = this.fb.group({
      vehicleId: ["", Validators.required],
      clientId: ["", Validators.required],
      registrationDate: [new Date(), Validators.required],
      isTemporary: [false, Validators.required],
      insuranceId: ["", Validators.required],
      licensePlate: ["", [Validators.required, this.validateLicensePlate()]],
    });

    this.getListOfVehicles();
    this.getListOfClients();
    this.getListOfInsurances();
  }

  getListOfVehicles(): void {
    this.vehicleService.getAllVehicles().subscribe({
      next: (res) => {
        this.vehicles = res.data.items;
      },
      error: (err) => {
        this.messageService.error(err);
      }
    });
  }

  getListOfClients(): void {
    this.clientService.getAllClients().subscribe({
      next: (res) => {
        this.clients = res.data.items;
      },
      error: (err) => {
        this.messageService.error(err);
      }
    });
  }

  getListOfInsurances(): void {
    this.insuranceService.getAllInsurances().subscribe({
      next: (res) => {
        this.insurances = res.data;
      },
      error: (err) => {
        this.messageService.error(err);
      }
    });
  }

  onSave(): void {
    this.registrationId ? this.onEditRegistration() : this.onAddNewRegistration();
  }

  onEditRegistration(): void {
    this.registrationChanged.emit(this.registrationForm.value);
  }

  onAddNewRegistration(): void {
    if (this.registrationForm.valid) {
      this.registrationAdded.emit(this.registrationForm.value);
    }
  }

  validateLicensePlate(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {

      const value = control.value;

      if (!value) {
        return null;
      }

      const pattern = /^[A-Z]{2}\d{3,5}[A-Z]{2}$/;
      const valid = pattern.test(value);

      return valid ? null : { licensePlateInvalid: true };
    };
  }
}