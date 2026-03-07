import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { InsuranceService } from '../../../core/services/insurance.service';
import { MessageService } from '../../../core/services/message.service';
import { MatCardModule } from '@angular/material/card';
import { InsuranceFormComponent } from '../components/insurance-form/insurance-form.component';

@Component({
  selector: 'app-insurance-details',
  templateUrl: './insurance-details.component.html',
  imports: [MatCardModule, InsuranceFormComponent],
})

export class InsuranceDetailsComponent {
  insuranceId: string;
  insurance: { id: string; naziv: string };
  constructor(private route: ActivatedRoute,
              private insuranceService: InsuranceService,
              private messageService: MessageService,
              private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.insuranceId = params['id'];
      this.getInsuranceById();
    });
  }

  getInsuranceById(): void {
    this.insuranceService.getInsuranceById(this.insuranceId).subscribe({
      next: (res) => {
        this.insurance = res.data;
      },
      error: (err) => {
        this.messageService.error(err);
      }
    })
  }

  editInsurance(insuranceData: any): void {
    if (insuranceData) {
      this.insuranceService.editInsurance({...insuranceData, id: this.insuranceId}).subscribe({
        next: (res) => {
          this.messageService.success('Osiguranje je uspješno izmijenjeno');
          this.router.navigate(['/insurance/list']);
        },
        error: (err) => {
          this.messageService.error(err);
        }
      })
    }
  }
}