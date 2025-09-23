import { Component } from "@angular/core";
import { Router, RouterOutlet } from "@angular/router";
import { CommonModule } from "@angular/common";
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { HeaderComponent } from './shared/components/header/header.component';

@Component({
  selector: "app-root",
  standalone: true,
  imports: [
    RouterOutlet,
    CommonModule,
    LoadingBarHttpClientModule,
    HeaderComponent
  ],
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
})
export class AppComponent {
  title = "vehicle-registration-system";

  constructor(public router: Router) {}
}
