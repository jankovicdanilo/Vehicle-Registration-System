import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from "@angular/forms";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from "@angular/material/input";

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule
  ],
})
export class SearchComponent {
  @Input() placeholder: string;
  @Input() label: string;
  searchTerm: string;
  @Output() searchChange = new EventEmitter<string>();

  onChange(value: string): void {
    this.searchChange.emit(value);
  }

  clear(): void {
    this.searchTerm = '';
    this.searchChange.emit('');
  }
}