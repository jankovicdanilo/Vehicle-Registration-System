import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from '../../../core/services/message.service';
import { InsuranceService } from '../../../core/services/insurance.service';
import { QuestionModalComponent } from '../../../shared/components/question-modal/question-modal.component';
import { MatDialog } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ToastrModule } from 'ngx-toastr';
import { EmptyListComponent } from '../../../shared/components/empty-list/empty-list.component';
import { UserService } from '../../../core/services/user.service';
import { Insurance } from '../../../core/models/insurance.model';

@Component({
  selector: 'app-insurance-list',
  standalone: true,
  templateUrl: './insurance-list.component.html',
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    ToastrModule,
    EmptyListComponent
  ],
})
export class InsuranceListComponent {

  dialog = inject(MatDialog);

  insurances: Insurance[] = [];

  displayedColumns: string[] = [
    'name',
    'actions'
  ];

  constructor(
    private insuranceService: InsuranceService,
    private messageService: MessageService,
    public userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getListOfInsurances();
  }

  getListOfInsurances(): void {
    this.insuranceService.getAllInsurances().subscribe({
      next: (res) => {
        this.insurances = res.data;
      },
      error: (err) => {
        this.messageService.error(err);
      }
    });
  }

  onEditInsurance(insurance: Insurance): void {
    this.router.navigate(['/insurance', insurance.id]);
  }

  navigateToInsuranceCreatePage(): void {
    this.router.navigate(['/insurance/add']);
  }

  onDeleteInsurance(insurance: Insurance): void {
    this.dialog.open(QuestionModalComponent, {
      width: '400px',
    }).afterClosed().subscribe((confirmed: boolean) => {
      if (confirmed) {
        this.deleteInsurance(insurance);
      }
    });
  }

  deleteInsurance(insurance: Insurance): void {
    this.insuranceService.deleteInsurance(insurance.id).subscribe({
      next: () => {
        this.getListOfInsurances();
        this.messageService.success('Insurance deleted successfully.');
      },
      error: (err) => {
        this.messageService.error(err);
      }
    });
  }
}