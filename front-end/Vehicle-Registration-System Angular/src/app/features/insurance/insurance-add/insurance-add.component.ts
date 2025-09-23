import { Component } from "@angular/core";
import { MatCardModule } from "@angular/material/card";
import { InsuranceFormComponent } from "../components/insurance-form/insurance-form.component";
import { InsuranceService } from "../../../core/services/insurance.service";
import { Router } from "@angular/router";
import { MessageService } from "../../../core/services/message.service";

@Component({
  selector: "app-insurance-add",
  templateUrl: "./insurance-add.component.html",
  imports: [MatCardModule, InsuranceFormComponent],
})
export class InsuranceAddComponent {
  constructor(
    private insuranceService: InsuranceService,
    private messageService: MessageService,
    private router: Router
  ) {}
  
  addInsurance(event: any): void {
    if (event) {
      this.insuranceService.addNewInsurance(event).subscribe({
        next: () => {
          this.router.navigate(["/insurance/list"]);
        },
        error: (err) => {
          this.messageService.error(err);
        },
      });
    }
  }
}
