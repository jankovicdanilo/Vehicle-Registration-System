import { Component } from '@angular/core';
import { UserSidebarComponent } from '../components/user-sidebar/user-sidebar.component';
import { MatCardModule } from '@angular/material/card';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../../core/services/auth.service';
import { MessageService } from '../../../core/services/message.service';
import { UserService } from '../../../core/services/user.service';
import { User } from '../../../core/models/user.model';

@Component({
  selector: 'app-user-change-password',
  standalone: true,
  templateUrl: './change-password.component.html',
  imports: [
    UserSidebarComponent,
    MatCardModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ]
})
export class ChangePasswordComponent {

  passwordForm!: FormGroup;
  currentUser!: User;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private messageService: MessageService,
    private userService: UserService
  ) {}

  ngOnInit(): void {

    this.currentUser = JSON.parse(this.userService.getCurrentUser() || '{}');

    this.passwordForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', Validators.required],
    });
  }

  changePassword(): void {

    if (this.passwordForm.invalid) return;

    this.authService.changePassword(this.passwordForm.value).subscribe({
      next: () => {
        this.messageService.success('Password successfully changed.');
        this.passwordForm.reset();
      },
      error: (err) => {
        this.messageService.error(err);
      }
    });
  }
}