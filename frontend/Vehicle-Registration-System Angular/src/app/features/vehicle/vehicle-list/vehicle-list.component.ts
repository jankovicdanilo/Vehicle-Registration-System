import { Component, inject } from "@angular/core";
import { MatTableModule } from "@angular/material/table";
import { MatCardModule } from "@angular/material/card";
import { VehicleService } from "../../../core/services/vehicle.service";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from "@angular/material/button";
import { Router } from "@angular/router";
import { MessageService } from "../../../core/services/message.service";
import { ToastrModule } from "ngx-toastr";
import { MatDialog } from "@angular/material/dialog";
import { QuestionModalComponent } from "../../../shared/components/question-modal/question-modal.component";
import { CommonModule } from "@angular/common";
import { EmptyListComponent } from "../../../shared/components/empty-list/empty-list.component";
import { SearchComponent } from '../../../shared/components/search/search.component';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';
import { Vehicle } from '../../../core/models/vehicle.model';
import { UserService } from '../../../core/services/user.service';

@Component({
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    ToastrModule,
    EmptyListComponent,
    SearchComponent,
    PaginationComponent
  ],
  selector: "app-vehicle-list",
  templateUrl: "./vehicle-list.component.html",
})
export class VehicleListComponent {
  dialog = inject(MatDialog);
  vehicles: Array<Vehicle> = [];
  searchTerm: string = '';
  totalItems: number;
  currentPageSize: number = 5;
  currentPage: number = 1;

  displayedColumns: string[] = [
    "tip",
    "marka",
    "model",
    "godinaProizvodnje",
    "vrstaGoriva",
    "actions",
  ];

  constructor(
    private vehicleService: VehicleService,
    private messageService: MessageService,
    public userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getListOfVehicles();
  }

  getListOfVehicles(): void {
    this.vehicleService.getAllVehicles(this.searchTerm, this.currentPageSize, this.currentPage).subscribe({
      next: (res) => {
        this.vehicles = [];
        res.data.items.forEach((vehicle) => {
          this.vehicles.push(vehicle);
        });
        this.totalItems = res.data.totalCount;
      },
      error: (err) => {
        this.messageService.error(err);
      },
    });
  }

  onSearchChange(value: string): void {
    this.searchTerm = value;
    this.getListOfVehicles();
  }

  onEditVehicle(vehicle: Vehicle): void {
    this.router.navigate(["/vehicle", vehicle.id]);
  }

  navigateToVehicleCreatePage(): void {
    this.router.navigate(["/vehicle/add"]);
  }

  onDeleteVehicle(vehicle: Vehicle) {
    this.dialog
      .open(QuestionModalComponent, {
        width: "400px",
      })
      .afterClosed()
      .subscribe((confirmed: boolean) => {
        if (confirmed) {
          this.deleteVehicle(vehicle);
        }
      });
  }

  deleteVehicle(vehicle: Vehicle): void {
    this.vehicleService.deleteVehicle(vehicle.id).subscribe({
      next: () => {
        this.getListOfVehicles();
        this.messageService.success("Uspješno ste obrisali vozilo.");
      },
      error: (err) => {
        this.messageService.error(err);
      },
    });
  }

  onPaginationChange(event: { pageNumber: number }) {
    this.currentPage = event.pageNumber;
    this.getListOfVehicles();
  }
}
