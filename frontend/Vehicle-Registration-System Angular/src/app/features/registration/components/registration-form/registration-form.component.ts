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

@Component({
  selector: 'app-registration-form',
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
  @Input() registrationId: string;
  @Input() registration: Registration;
  registrationForm: FormGroup;
  vehicles: Array<Vehicle> = [];
  clients: Array<Client> = [];
  insurances: Array<{ id: string; naziv: string }> = [];

  @Output() registrationAdded = new EventEmitter<void>();
  @Output() registrationChanged = new EventEmitter<void>();

  constructor(private fb: FormBuilder,
              private vehicleService: VehicleService,
              private clientService: ClientService,
              private messageService: MessageService,
              private insuranceService: InsuranceService,
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['registration'] && this.registrationForm && this.registration) {
      const osiguranjeId = this.registration.osiguranje?.id || null;
  
      this.insurances = this.insurances.map(i => ({
        ...i,
        id: i.id.toLowerCase()
      }));
  
      this.registrationForm.patchValue({
        voziloId: this.registration.voziloId,
        klijentId: this.registration.klijentId,
        datumRegistracije: this.registration.datumRegistracije,
        privremenaRegistracija: this.registration.privremenaRegistracija,
        osiguranjeId: osiguranjeId,
        registarskaOznaka: this.registration.registarskaOznaka
      });
    }
  }  

  ngOnInit(): void {
    this.registrationForm = this.fb.group({
      voziloId: ["", Validators.required],
      klijentId: ["", Validators.required],
      datumRegistracije: [new Date(), Validators.required],
      privremenaRegistracija: [false, Validators.required],
      osiguranjeId: ["", [Validators.required]],
      registarskaOznaka: ["", [Validators.required, this.validateRegistrarskaOznaka()]],
    });

    this.getListOfVehicles();
    this.getListOfClients();
    this.getListOfInsurances();
  }


  getListOfVehicles(): void {
    this.vehicleService.getAllVehicles().subscribe({
      next: (res) => {
        this.vehicles = [];
        res.data.items.forEach((vehicle) => {
          this.vehicles.push(vehicle);
        });

      },
      error: (err) => {
        this.messageService.error(err);
      }
    })
  }

  getListOfClients(): void {
    this.clientService.getAllClients().subscribe({
      next: (res) => {
        this.clients = [];
        res.data.items.forEach((vehicle) => {
          this.clients.push(vehicle);
        });

      },
      error: (err) => {
        this.messageService.error(err);
      }
    })
  }

  getListOfInsurances(): void {
    this.insuranceService.getAllInsurances().subscribe({
      next: (res) => {
        res.data.forEach((insurance) => {
          this.insurances.push(insurance);
        })
      },
      error: (err) => {
        this.messageService.error(err);
      }
    })
  }

  onSave() {
    this.registrationId ? this.onEditRegistration() : this.onAddNewRegistration();
  }

  onEditRegistration() {
    this.registrationChanged.emit(this.registrationForm.value);
  }


  onAddNewRegistration() {
    if (this.registrationForm.valid) {
      this.registrationAdded.emit(this.registrationForm.value);
    }
  }

  validateRegistrarskaOznaka(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
  
      if (!value) {
        return null;
      }
  
      const pattern = /^[A-Z]{2}\d{3,5}[A-Z]{2}$/;
      const valid = pattern.test(value);
  
      return valid ? null : { registarskaOznakaInvalid: true };
    };
  }
}