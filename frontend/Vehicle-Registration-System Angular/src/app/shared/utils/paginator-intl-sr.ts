import { MatPaginatorIntl } from '@angular/material/paginator';

export function getSrPaginatorIntl(): MatPaginatorIntl {
  const paginatorIntl = new MatPaginatorIntl();

  paginatorIntl.nextPageLabel = 'Sledeća stranica';
  paginatorIntl.previousPageLabel = 'Prethodna stranica';
  paginatorIntl.firstPageLabel = 'Prva stranica';
  paginatorIntl.lastPageLabel = 'Poslednja stranica';

  paginatorIntl.getRangeLabel = (page: number, pageSize: number, length: number) => {
    if (length === 0 || pageSize === 0) {
      return `0 od ${length}`;
    }
    const startIndex = page * pageSize;
    const endIndex = startIndex < length
      ? Math.min(startIndex + pageSize, length)
      : startIndex + pageSize;

    return `${startIndex + 1} – ${endIndex} od ${length}`;
  };

  return paginatorIntl;
}