import { Component, inject } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { MessageService } from '../../../core/services/message.service';
import { ToastrModule } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { QuestionModalComponent } from '../../../shared/components/question-modal/question-modal.component';
import { ClientService } from '../../../core/services/client.service';
import { CommonModule } from '@angular/common';
import { EmptyListComponent } from '../../../shared/components/empty-list/empty-list.component';
import { SearchComponent } from '../../../shared/components/search/search.component';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';
import { Client } from '../../../core/models/client.model';
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
    SearchComponent,
    PaginationComponent
  ],
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
})
export class ClientListComponent {
  dialog = inject(MatDialog);
  clients: Array<Client> = [];
  searchTerm: string = '';
  totalItems: number;
  currentPageSize: number = 5;
  currentPage: number = 1;

  displayedColumns: string[] = [
    'ime',
    'prezime',
    'adresa',
    'jmbg',
    'datumRodjenja',
    'brojTelefona',
    'email',
    'actions'
  ];

  constructor(private clientService: ClientService,
              private messageService: MessageService,
              public userService: UserService,
              private router: Router
  ) {}

  ngOnInit(): void {
    this.getListOfClients();
  }

  getListOfClients(): void {
    this.clientService.getAllClients(this.searchTerm,  this.currentPageSize, this.currentPage).subscribe({
      next: (res) => {
        this.clients = [];
        res.data.items.forEach((client) => {
          this.clients.push(client);
        });
        this.totalItems = res.data.totalCount;
      },
      error: (err) => {
        this.messageService.error(err);
      }
    })
  }

  onSearchChange(value: string): void {
    this.searchTerm = value;
    this.getListOfClients();
  }

  onEditClient(client: Client): void {
    this.router.navigate(['/client', client.id]);
  }

  navigateToClientCreatePage(): void {
    this.router.navigate(['/client/add']);
  }

  onDeleteClient(client: Client): void {
    this.dialog.open(QuestionModalComponent, {
      width: '400px',
    }).afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        this.deleteClient(client);
      }
    });
  }

  deleteClient(client: Client): void {
    this.clientService.deleteClient(client.id).subscribe({
      next: () => {
        this.getListOfClients();
        this.messageService.success('Uspješno ste obrisali klijenta.');
      },
      error: (err) => {
        this.messageService.error(err);
      }
    })
  }

  onPaginationChange(event: { pageNumber: number }): void {
    this.currentPage = event.pageNumber;
    this.getListOfClients();
  }
}