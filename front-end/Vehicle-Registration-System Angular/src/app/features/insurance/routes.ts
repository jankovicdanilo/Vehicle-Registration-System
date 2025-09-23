import { Routes } from '@angular/router';
import { InsuranceListComponent } from './insurance-list/insurance-list.component';
import { InsuranceAddComponent } from './insurance-add/insurance-add.component';
import { InsuranceDetailsComponent } from './insurance-details/insurance-details.component';

export const INSURANCE_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'list',
    pathMatch: 'full',
  },
  {
    path: 'list',
    component: InsuranceListComponent
  },
  {
    path: 'add',
    component: InsuranceAddComponent
  },
  {
    path: ':id',
    component: InsuranceDetailsComponent
  }
];