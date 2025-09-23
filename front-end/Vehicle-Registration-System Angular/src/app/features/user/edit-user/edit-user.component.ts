import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { AuthService } from "../../../core/services/auth.service";
import { MessageService } from "../../../core/services/message.service";
import { MatCardModule } from "@angular/material/card";
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { UserFormComponent } from '../../../shared/components/user-form/user-form.component';

@Component({
  imports: [
    UserFormComponent,
    MatCardModule,
    HeaderComponent
  ],
  selector: "app-edit-user",
  templateUrl: "./edit-user.component.html",
})
export class EditUserComponent implements OnInit {
  userId: string;
  user: any;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.userId = params["id"];

      this.getUserById();
    });
  }

  getUserById(): void {
    this.authService.getUserDetails(this.userId).subscribe({
      next: (res) => {
        this.user = res.data;
      },
      error: (err) => {
        this.messageService.error(err);
      },
    });
  }

    editUser(userData: any): void {
        if (userData) {
            let roles: string[] = ["Zaposleni"]; // everyone has at least this

            if (userData.role === "Admin") {
                roles.unshift("Admin");
            } else if (userData.role === "SefOdsjeka") {
                roles.unshift("SefOdsjeka");
            }

            const data = {
                id: this.userId,
                username: userData.username,
                email: userData.email,
                password: userData.password || null,
                roles: roles
            };

            this.authService.updateUser(data).subscribe({
                next: () => {
                    this.messageService.success("Korisnik je uspješno izmijenjen");
                    this.router.navigate(["/user/list"]);
                },
                error: (err) => {
                    this.messageService.error(err);
                },
            });
        }
    }
}
