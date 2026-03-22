import { Component, inject } from '@angular/core';
import { RegistrationService } from '../../../core/services/registration.service';
import { MessageService } from '../../../core/services/message.service';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ToastrModule } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { QuestionModalComponent } from '../../../shared/components/question-modal/question-modal.component';
import { EmptyListComponent } from '../../../shared/components/empty-list/empty-list.component';
import { SearchComponent } from '../../../shared/components/search/search.component';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';
import { Registration } from '../../../core/models/registration.model';
import { UserService } from '../../../core/services/user.service';
import { RegistrationListItem } from '../../../core/models/registration-list.model';

@Component({
  selector: 'app-registration-list',
  standalone: true,
  templateUrl: './registration-list.component.html',
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    ToastrModule,
    EmptyListComponent,
    SearchComponent,
    PaginationComponent
  ],
})
export class RegistrationListComponent {

  dialog = inject(MatDialog);

  registrations: RegistrationListItem[] = [];
  searchTerm: string = '';

  totalItems!: number;
  currentPageSize: number = 5;
  currentPage: number = 1;

  displayedColumns: string[] = [
    'vehicle',
    'licensePlate',
    'owner',
    'registrationDate',
    'expirationDate',
    'registrationPrice',
    'insurance',
    'actions'
  ];

  constructor(
    private registrationService: RegistrationService,
    private messageService: MessageService,
    public userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getListOfRegistrations();
  }

  getListOfRegistrations(): void {
    this.registrationService
      .getAllRegistrations(this.searchTerm, this.currentPageSize, this.currentPage)
      .subscribe({
        next: (res) => {
          this.registrations = res.data.items;
          this.totalItems = res.data.totalCount;
        },
        error: (err) => {
          this.messageService.error(err);
        }
      });
  }

  onSearchChange(value: string): void {
    this.searchTerm = value;
    this.getListOfRegistrations();
  }

  navigateToRegistrationCreatePage(): void {
    this.router.navigate(['/registration/add']);
  }

  onEditRegistration(registration: Registration): void {
    this.router.navigate(['/registration', registration.id]);
  }

  onDeleteRegistration(registration: Registration): void {
    this.dialog.open(QuestionModalComponent, {
      width: '400px',
    }).afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        this.deleteRegistration(registration);
      }
    });
  }

  deleteRegistration(registration: Registration): void {
    this.registrationService.deleteRegistration(registration.id).subscribe({
      next: () => {
        this.getListOfRegistrations();
        this.messageService.success('Registration deleted successfully.');
      },
      error: (err) => {
        this.messageService.error(err);
      }
    });
  }

  onPaginationChange(event: { pageNumber: number }): void {
    this.currentPage = event.pageNumber;
    this.getListOfRegistrations();
  }
}