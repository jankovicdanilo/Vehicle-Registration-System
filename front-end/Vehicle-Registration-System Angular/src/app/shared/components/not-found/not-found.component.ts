import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css'],
  imports: [MatIconModule, MatButtonModule, RouterModule],
})

export class NotFoundComponent {
  constructor(private router: Router) {}

  logout() {
    localStorage.clear();
    this.router.navigate(["/"]);
  }
}