import { Component } from '@angular/core';
import { ClientService } from '../../../core/services/client.service';
import { MessageService } from '../../../core/services/message.service';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { ClientFormComponent } from '../components/client-form/client-form.component';

@Component({
  selector: 'app-client-add',
  templateUrl: './client-add.component.html',
  imports: [MatCardModule, ClientFormComponent],
})

export class ClientAddComponent {
  constructor(private clientService: ClientService,
              private messageService: MessageService,
              private router: Router
  ) {}

  addClient(event: any): void {
    this.clientService.addNewClient(event).subscribe({
      next: () => {
        this.router.navigate(['/client/list']);
      },
      error: (err) => {
        this.messageService.error(err);
      }
    });
  }
}