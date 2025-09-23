import { Component, EventEmitter, Input, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import {MatOption, MatSelect} from '@angular/material/select';


@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatCardModule,
        MatSelect,
        MatOption,
    ],
})
export class UserFormComponent implements OnInit {
  @Input() user: any;
  @Input() userId: string;
  userForm: FormGroup;
  @Output() userAdded = new EventEmitter<void>();
  @Output() userChanged = new EventEmitter<void>();
  
  constructor(private fb: FormBuilder) {}

    ngOnChanges(changes: SimpleChanges): void {
        if (changes['user'] && this.userForm && this.user) {
            this.userForm.patchValue(this.user);

            if (this.user.roles.includes('Admin')) {
                this.userForm.get('role')?.setValue('Admin');
            } else if (this.user.roles.includes('SefOdsjeka')) {
                this.userForm.get('role')?.setValue('SefOdsjeka');
            } else {
                this.userForm.get('role')?.setValue('Zaposleni');
            }
        }
    }

  ngOnInit(): void {
    this.userForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      role:[''],
    });
  }

  onSave() {
    this.userId ? this.onEditUser() : this.onAddNewUser();
  }

  onEditUser() {
    this.userChanged.emit(this.userForm.value);
  }

  onAddNewUser() {
    if (this.userForm.valid) {
      this.userAdded.emit(this.userForm.value);
    }
  }

  get register() {
    return this.userForm.controls;
  }
}