import { Component, EventEmitter, Input, Output, SimpleChanges } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatNativeDateModule } from "@angular/material/core";
import { CommonModule } from "@angular/common";
import { VehicleService } from "../../../../core/services/vehicle.service";
import { MessageService } from "../../../../core/services/message.service";
import { Vehicle } from "../../../../core/models/vehicle.model";

@Component({
  selector: "app-vehicle-form",
  standalone: true,
  templateUrl: "./vehicle-form.component.html",
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
})
export class VehicleFormComponent {

  @Input() vehicle!: Vehicle;
  @Input() vehicleId!: string;

  vehicleForm!: FormGroup;

  vehicleTypes: { id: string; name: string; category: string }[] = [];
  vehicleBrands: { id: string; name: string; vehicleTypeId: string }[] = [];
  vehicleModels: { id: string; name: string; brandId: string }[] = [];

  fuelTypes: string[] = [
    "Petrol",
    "Diesel",
    "Electric",
    "Hybrid",
    "LPG",
    "CNG"
  ];

  @Output() vehicleAdded = new EventEmitter<any>();
  @Output() vehicleChanged = new EventEmitter<any>();

  constructor(
    private fb: FormBuilder,
    private vehicleService: VehicleService,
    private messageService: MessageService
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["vehicle"] && this.vehicleForm && this.vehicle) {
      this.vehicleForm.patchValue(this.vehicle);
    }
  }

  ngOnInit(): void {

    this.vehicleForm = this.fb.group({
      chassisNumber: ["", Validators.required],
      firstRegistrationDate: ["", Validators.required],
      productionYear: ["", Validators.required],
      vehicleBrandId: ["", Validators.required],
      vehicleModelId: ["", Validators.required],
      vehicleTypeId: ["", Validators.required],
      weight: ["", Validators.required],
      enginePowerKw: ["", Validators.required],
      engineCapacity: ["", Validators.required],
      fuelType: ["", Validators.required],
    });

    this.getVehicleTypes();

    this.vehicleForm.get("vehicleTypeId")?.valueChanges.subscribe(typeId => {
      if (typeId) {
        this.getVehicleBrands(typeId);
      }
    });

    this.vehicleForm.get("vehicleBrandId")?.valueChanges.subscribe(brandId => {
      if (brandId) {
        this.getVehicleModels(brandId);
      }
    });
  }

  getVehicleTypes(): void {
    this.vehicleService.getVehicleTypes().subscribe({
      next: (res) => {
        this.vehicleTypes = res.data;
      },
      error: (err) => {
        this.messageService.error(err);
      },
    });
  }

  getVehicleBrands(vehicleTypeId: string): void {
    this.vehicleService.getVehicleBrands(vehicleTypeId).subscribe({
      next: (res) => {
        this.vehicleBrands = res.data;
      },
      error: (err) => {
        this.messageService.error(err);
      },
    });
  }

  getVehicleModels(brandId: string): void {
    this.vehicleService.getVehicleModels(brandId).subscribe({
      next: (res) => {
        this.vehicleModels = res.data;
      },
      error: (err) => {
        this.messageService.error(err);
      },
    });
  }

  onSave() {
    this.vehicleId ? this.onEditVehicle() : this.onAddNewVehicle();
  }

  onEditVehicle() {
    this.vehicleChanged.emit(this.vehicleForm.value);
  }

  onAddNewVehicle() {
    if (this.vehicleForm.valid) {
      this.vehicleAdded.emit(this.vehicleForm.value);
    }
  }
}