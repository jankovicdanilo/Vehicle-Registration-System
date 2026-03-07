import { Routes } from '@angular/router';
import { RegistrationListComponent } from './registration-list/registration-list.component';
import { RegistrationAddComponent } from './registration-add/registration-add.component';
import { RegistrationDetailsComponent } from './registration-details/registration-details.component';

export const REGISTRATION_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'list',
    pathMatch: 'full',
  },
  {
    path: 'list',
    component: RegistrationListComponent
  },
  {
    path: 'add',
    component: RegistrationAddComponent
  },
  {
    path: ':id',
    component: RegistrationDetailsComponent
  }
];