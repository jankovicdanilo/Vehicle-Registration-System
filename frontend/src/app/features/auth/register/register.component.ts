import { Component } from "@angular/core";
import { AuthService } from "../../../core/services/auth.service";
import { Router, RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MessageService } from "../../../core/services/message.service";
import { HeaderComponent } from "../../../shared/components/header/header.component";
import { UserFormComponent } from "../../../shared/components/user-form/user-form.component";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    MatCardModule,
    HeaderComponent,
    UserFormComponent,
  ],
})
export class RegisterComponent {

  constructor(
    private authService: AuthService,
    private messageService: MessageService,
    private router: Router
  ) {}

  addUser(event: any): void {
    if (!event) return;

    let roles: string[] = ["Employee"];

    if (event.role === "Admin") {
      roles.unshift("Admin");
    } else if (event.role === "Manager") {
      roles.unshift("Manager");
    }

    const data = {
      username: event.username || "",
      email: event.email || "",
      password: event.password || "",
      roles: roles
    };

    console.log("Payload sent:", data);

    this.authService.register(data).subscribe({
      next: () => {
        this.messageService.success("User registered successfully");
        this.router.navigate(["/user/list"]);
      },
      error: (err) => {
        this.messageService.error(err);
      },
    });
  }
}