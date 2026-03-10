import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule, MatPaginatorModule],
  template: `
    <mat-paginator
      [length]="totalItems"
      [pageSize]="pageSize"
      [pageSizeOptions]="[5,10,20]"
      (page)="onPageChange($event)">
    </mat-paginator>
  `
})
export class PaginationComponent {

  @Input() totalItems!: number;
  @Input() pageSize!: number;

  @Output() paginationChange = new EventEmitter<{ pageNumber: number }>();

  onPageChange(event: PageEvent) {
    this.paginationChange.emit({
      pageNumber: event.pageIndex + 1
    });
  }

}