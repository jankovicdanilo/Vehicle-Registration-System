import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { VehicleFormComponent } from '../components/vehicle-form/vehicle-form.component';
import { VehicleService } from '../../../core/services/vehicle.service';
import { Router } from '@angular/router';
import { MessageService } from '../../../core/services/message.service';

@Component({
  selector: 'app-vehicle-add',
  imports: [MatCardModule, VehicleFormComponent],
  templateUrl: './vehicle-add.component.html',
})
export class VehicleAddComponent {

  constructor(private vehicleService: VehicleService, 
              private router: Router,
              private messageService: MessageService) {}
  
  addVehicle(event: any): void {
    this.vehicleService.addNewVehicle(event).subscribe({
      next: () => {
        this.router.navigate(['/vehicle/list']);
      },
      error: (err) => {
        this.messageService.error(err);
      }
    });
  }
}