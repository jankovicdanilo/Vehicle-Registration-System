import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RegistrationService } from '../../../core/services/registration.service';
import { MessageService } from '../../../core/services/message.service';
import { MatCardModule } from '@angular/material/card';
import { RegistrationFormComponent } from '../components/registration-form/registration-form.component';
import { Registration } from '../../../core/models/registration.model';

@Component({
  selector: 'app-registration-details',
  templateUrl: './registration-details.component.html',
  imports: [MatCardModule, RegistrationFormComponent],
})
export class RegistrationDetailsComponent {
  registrationId: string;
  registration: Registration;

  constructor(private route: ActivatedRoute,
              private registrationService: RegistrationService,
              private messageService: MessageService,
              private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.registrationId = params['id'];
      this.getRegistrationById();
    });
  }

  getRegistrationById(): void {
    this.registrationService.getRegistrationById(this.registrationId).subscribe({
      next: (res) => {
        this.registration = res.data;
      },
      error: (err) => {
        this.messageService.error(err);
      }
    })
  }

  editRegistration(registrationData: any): void {
    if (registrationData) {
      this.registrationService.editRegistration({...registrationData, id: this.registrationId}).subscribe({
        next: (res) => {
          this.messageService.success('Podaci o registraciji su uspješno izmijenjeni');
          this.router.navigate(['/registration/list']);
        },
        error: (err) => {
          this.messageService.error(err);
        }
      })
    }
  }
}