import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';


@Component({
  selector: 'app-question-modal',
  templateUrl: './question-modal.component.html',
  styleUrls: ['./question-modal.component.css'],
  imports: [CommonModule, MatDialogModule, MatButtonModule],
})
export class QuestionModalComponent {
  dialogRef = inject(MatDialogRef<QuestionModalComponent>, { optional: true });
  data = inject(MAT_DIALOG_DATA, { optional: true });
  constructor(
  ) {}

  onYes(): void {
    this.dialogRef.close(true);
  }

  onNo(): void {
    this.dialogRef.close(false);
  }
}