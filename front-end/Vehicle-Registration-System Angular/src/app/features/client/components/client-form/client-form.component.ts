import { Component, EventEmitter, Input, Output, SimpleChanges } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatNativeDateModule } from "@angular/material/core";
import { CommonModule } from "@angular/common";
import { Client } from '../../../../core/models/client.model';

@Component({
  selector: "app-client-form",
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
  ],
  templateUrl: "./client-form.component.html",
})
export class ClientFormComponent {
  @Input() clientId: string;
  @Input() client: Client;
  clientForm: FormGroup;
  @Output() clientAdded = new EventEmitter<void>();
  @Output() clientChanged = new EventEmitter<void>();

  constructor(
    private fb: FormBuilder,
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['client'] && this.clientForm && this.client) {
      this.clientForm.patchValue(this.client);
    }
  }


  ngOnInit(): void {
    this.clientForm = this.fb.group({
      ime: ["", Validators.required],
      prezime: ["", Validators.required],
      jmbg: ["", [Validators.required, Validators.pattern(/^\d{13}$/)]],
      adresa: ["", Validators.required],
      brojTelefona: [""],
      email: ["", Validators.email],
      datumRodjenja: [""],
      brojLicneKarte: ["", [Validators.required, Validators.pattern(/^\d{9}$/)]],
    });
  }

  onSave(): void {
    this.clientId ? this.onEditClient() : this.onAddNewClient();
  }

  onEditClient(): void {
    this.clientChanged.emit(this.clientForm.value);
  }

  onAddNewClient() {
    if (this.clientForm.valid) {
      this.clientAdded.emit(this.clientForm.value);
    }
  }
}