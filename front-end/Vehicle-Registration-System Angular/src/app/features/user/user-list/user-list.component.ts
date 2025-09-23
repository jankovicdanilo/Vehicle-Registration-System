import { Component, inject } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../core/models/user.model';
import { MessageService } from '../../../core/services/message.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ToastrModule } from 'ngx-toastr';
import { EmptyListComponent } from '../../../shared/components/empty-list/empty-list.component';
import { QuestionModalComponent } from '../../../shared/components/question-modal/question-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from '../../../core/services/user.service';

@Component({
   imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    ToastrModule,
    EmptyListComponent,
  ],
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
})
export class UserListComponent {
   dialog = inject(MatDialog);
  users: Array<any> = [];

  displayedColumns: string[] = [
    'Korisničko ime',
    'email',
    'role',
    'actions'
  ];

  constructor(private authService: AuthService,
              private messageService: MessageService,
              public userService: UserService,
              private router: Router
  ) {}

  ngOnInit(): void {
    this.getListOfUsers();
  }

  getListOfUsers(): void {
    this.authService.getUsers().subscribe({
      next: (res) => {
        this.users = [];
        res.forEach((user) => {
          this.users.push(user);
        });
      },
      error: (err) => {
        this.messageService.error(err);
      }
    })
  }

    formatRole(role: string): string {
        if (role === 'SefOdsjeka') return 'Šef odsjeka';
        return role;
    }


    onEditUser(user: User): void {
    this.router.navigate(['auth/register', user.id]);
  }

  navigateToUserCreatePage(): void {
    this.router.navigate(['/auth/register']);
  }

  onDeleteUser(user: User): void {
    this.dialog.open(QuestionModalComponent, {
      width: '400px',
    }).afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        this.deleteUser(user);
      }
    });
  }

  deleteUser(user: User): void {
    this.authService.deleteUser(user.id).subscribe({
      next: () => {
        this.getListOfUsers();
        this.messageService.success('Uspješno ste obrisali korisnika.');
      },
      error: (err) => {
        this.messageService.error(err);
      }
    })
  }
}