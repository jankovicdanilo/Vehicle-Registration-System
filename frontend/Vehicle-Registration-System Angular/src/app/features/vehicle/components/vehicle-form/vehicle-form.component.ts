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
import { MessageService } from '../../../../core/services/message.service';
import { Vehicle } from '../../../../core/models/vehicle.model';

@Component({
  selector: "app-vehicle-form",
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
  templateUrl: "./vehicle-form.component.html",
})
export class VehicleFormComponent {
  @Input() vehicle: Vehicle;
  @Input() vehicleId: string;
  vehicleForm: FormGroup;
  vehicleTypes: Array<{ id: string; naziv: string; kategorija: string }> = []; // change type
  vehicleBrands: Array<{ id: string; naziv: string; tipVozilaId: string }> = [];
  vehicleBrandModels: Array<{ id: string; naziv: string; markaId: string }> =
    [];
  vehicleFuelTypes: Array<string> = [
    "Benzin",
    "Dizel",
    "Električni pogon",
    "Hibrid",
    "Tečni naftni gas",
    "Kompresovani prirodni gas",
  ];
  @Output() vehicleAdded = new EventEmitter<void>();
  @Output() vehicleChanged = new EventEmitter<void>();

  constructor(
    private fb: FormBuilder,
    private vehicleService: VehicleService,
    private messageService: MessageService,
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['vehicle'] && this.vehicleForm && this.vehicle) {
      this.vehicleForm.patchValue(this.vehicle);
    }
  }

  ngOnInit(): void {
    this.vehicleForm = this.fb.group({
      brojSasije: ["", Validators.required],
      datumPrveRegistracije: ["", Validators.required],
      godinaProizvodnje: ["", Validators.required],
      markaVozilaId: ["", Validators.required],
      modelVozilaId: ["", Validators.required],
      tipVozilaId: ["", Validators.required],
      masa: ["", Validators.required],
      snagaMotora: ["", Validators.required],
      zapreminaMotora: ["", Validators.required],
      vrstaGoriva: ["", Validators.required],
    });
    
    this.getVehicleTypes();

    this.vehicleForm.get("tipVozilaId")?.valueChanges.subscribe((tip) => {
      if (tip) {
        this.getVehicleBrands(tip);
      }
    });

    this.vehicleForm.get("markaVozilaId")?.valueChanges.subscribe((marka) => {
      if (marka) {
        this.getVehicleModels(marka);
      }
    });
  }

  getVehicleTypes(): void {
    this.vehicleTypes = [];
    this.vehicleService.getVehicleTypes().subscribe({
      next: (res) => {
        res.data.forEach((vehicleType) => {
          this.vehicleTypes.push(vehicleType);
        });
      },
      error: (err) => {
        this.messageService.error(err);
      },
    });
  }

  getVehicleBrands(vehicleTypeId: string): void {
    this.vehicleBrands = [];
    this.vehicleService.getVehicleBrands(vehicleTypeId).subscribe({
      next: (res) => {
        res.data.forEach((vehicleBrand) => {
          this.vehicleBrands.push(vehicleBrand);
        });
      },
      error: (err) => {
        this.messageService.error(err);
      },
    });
  }

  getVehicleModels(brandId: string): void {
    this.vehicleBrandModels = [];
    this.vehicleService.getVehicleModels(brandId).subscribe({
      next: (res) => {
        res.data.forEach((vehicleBrandModel) => {
          this.vehicleBrandModels.push(vehicleBrandModel);
        });
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
