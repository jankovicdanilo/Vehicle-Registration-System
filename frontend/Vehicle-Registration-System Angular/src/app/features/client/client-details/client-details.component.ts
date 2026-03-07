import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientService } from '../../../core/services/client.service';
import { MessageService } from '../../../core/services/message.service';
import { MatCardModule } from '@angular/material/card';
import { ClientFormComponent } from '../components/client-form/client-form.component';
import { Client } from '../../../core/models/client.model';

@Component({
  selector: 'app-client-details',
  templateUrl: './client-details.component.html',
  imports: [MatCardModule, ClientFormComponent]
})

export class ClientDetailsComponent {
  clientId: string;
  client: Client;
  constructor(private route: ActivatedRoute,
              private clienService: ClientService,
              private messageService: MessageService,
              private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.clientId = params['id'];
      this.getClientById();
    });
  }

  getClientById(): void {
    this.clienService.getClientById(this.clientId).subscribe({
      next: (res) => {
        this.client = res.data;
      },
      error: (err) => {
        this.messageService.error(err);
      }
    })
  }

  editClient(clientData: any): void {
    if (clientData) {
      this.clienService.editClient({...clientData, id: this.clientId}).subscribe({
        next: () => {
          this.messageService.success('Klijent je uspješno izmijenjen');
          this.router.navigate(['/client/list']);
        },
        error: (err) => {
          this.messageService.error(err);
        }
      })
    }
  }
}