import { Routes } from '@angular/router';
import { VehicleListComponent } from './vehicle-list/vehicle-list.component';
import { VehicleAddComponent } from './vehicle-add/vehicle-add.component';
import { VehicleDetailsComponent } from './vehicle-details/vehicle-details.component';

export const VEHICLE_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'list',
    pathMatch: 'full',
  },
  {
    path: 'list',
    component: VehicleListComponent
  },
  {
    path: 'add',
    component: VehicleAddComponent
  },
  {
    path: ':id',
    component: VehicleDetailsComponent
  }
];