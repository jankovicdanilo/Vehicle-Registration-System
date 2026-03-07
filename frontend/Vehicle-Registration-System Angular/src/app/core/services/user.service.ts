import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { jwtDecode } from "jwt-decode";
import { environment } from '../../enviroments/enviroment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  API_URL: string = environment.apiURL;

  constructor(private http: HttpClient) {}

  getCurrentUser() {
    return localStorage.getItem('user');
  }

  /**
 * Check session expiration
 */
  isSessionValid(): boolean {
  const token = localStorage.getItem('token');

  if (!token) {
    return false;
  }

  try {
    const decodedJWT: any = jwtDecode(token);
    const expirationEpoch = decodedJWT.exp;
    const currentEpoch = Math.floor(Date.now() / 1000);

    return currentEpoch < expirationEpoch;
  } catch (e) {
    // In case the token is invalid or corrupt
    console.error('Invalid token:', e);
    return false;
  }
}


  isSignedIn(): boolean {
    return localStorage.getItem('token') !== null && this.isSessionValid();
  }

  isAdmin(): boolean {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    return user?.role?.includes('Admin');
  }

  isSefOdsjeka(): boolean {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    return user?.role?.includes('SefOdsjeka');
  }
}