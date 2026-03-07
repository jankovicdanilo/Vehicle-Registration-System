import { Routes } from '@angular/router';
import { UserDetailsComponent } from './user-details/user-details.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { UserListComponent } from './user-list/user-list.component';

export const USER_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'user',
    pathMatch: 'full',
  },
  {
    path: 'profile',
    component: UserDetailsComponent
  },
  {
    path: 'change-password',
    component: ChangePasswordComponent
  },
  {
    path: 'list',
    component: UserListComponent
  }
];


