import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VehicleService } from '../../../core/services/vehicle.service';
import { MessageService } from '../../../core/services/message.service';
import { VehicleFormComponent } from '../components/vehicle-form/vehicle-form.component';
import { MatCardModule } from '@angular/material/card';
import { Vehicle } from '../../../core/models/vehicle.model';

@Component({
  selector: 'app-vehicle-details',
  templateUrl: './vehicle-details.component.html',
  imports: [MatCardModule, VehicleFormComponent],
})
export class VehicleDetailsComponent {
  vehicleId: string;
  vehicle: Vehicle;
  constructor(private route: ActivatedRoute,
              private vehicleService: VehicleService,
              private messageService: MessageService,
              private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.vehicleId = params['id'];
      this.getVehicleById();
    });
  }

  getVehicleById(): void {
    this.vehicleService.getVehicleById(this.vehicleId).subscribe({
      next: (res) => {
        this.vehicle = res.data;
      },
      error: (err) => {
        this.messageService.error(err);
      }
    })
  }

  editVehicle(vehicleData: any): void {
    if (vehicleData) {
      this.vehicleService.editVehicle({...vehicleData, id: this.vehicleId}).subscribe({
        next: (res) => {
          this.messageService.success('Vozilo je uspješno izmijenjeno');
          this.router.navigate(['/vehicle/list']);
        },
        error: (err) => {
          this.messageService.error(err);
        }
      })
    }
  }
}