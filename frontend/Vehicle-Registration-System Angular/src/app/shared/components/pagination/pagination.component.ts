import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatPaginatorIntl, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { getSrPaginatorIntl } from '../../utils/paginator-intl-sr';


@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css'],
  imports: [MatPaginatorModule],
  providers: [
    { provide: MatPaginatorIntl, useFactory: getSrPaginatorIntl }
  ],
})
export class PaginationComponent {
  @Input() totalItems: number;
  @Input() pageSize: number;
  @Input() pageIndex: number;
  @Input() showFirstLastButtons: boolean = true;
  @Input() disabled: boolean = false;

  @Output() paginationChange = new EventEmitter<{ pageNumber: number }>();

  onPageChange(event: PageEvent): void {
    const pageNumber = event.pageIndex + 1;

    this.paginationChange.emit({ pageNumber });
  }
}