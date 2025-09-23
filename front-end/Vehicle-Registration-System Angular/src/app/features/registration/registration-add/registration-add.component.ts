import { Component } from '@angular/core';
import { RegistrationService } from '../../../core/services/registration.service';
import { MessageService } from '../../../core/services/message.service';
import { Router } from '@angular/router';
import { RegistrationFormComponent } from '../components/registration-form/registration-form.component';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-registration-add',
  templateUrl: './registration-add.component.html',
  imports: [MatCardModule, RegistrationFormComponent],
})

export class RegistrationAddComponent {

  constructor(private registrationService: RegistrationService,
              private messageService: MessageService,
              private router: Router
  ) {}
  addRegistration(event: any): void {
    this.registrationService.addNewRegistration(event).subscribe({
      next: () => {
        this.router.navigate(['/registration/list']);
      },
      error: (err) => {
        this.messageService.error(err);
      }
    });
  }
}