import { Routes } from '@angular/router';
import { ClientListComponent } from './client-list/client-list.component';
import { ClientAddComponent } from './client-add/client-add.component';
import { ClientDetailsComponent } from './client-details/client-details.component';

export const CLIENT_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'list',
    pathMatch: 'full',
  },
  {
    path: 'list',
    component: ClientListComponent
  },
  {
    path: 'add',
    component: ClientAddComponent
  },
  {
    path: ':id',
    component: ClientDetailsComponent
  }
];