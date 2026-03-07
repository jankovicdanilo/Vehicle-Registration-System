import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../enviroments/enviroment';

@Injectable({
  providedIn: 'root',
})
export class RegistrationService {
  API_URL: string = environment.apiURL

  constructor(private http: HttpClient) {}

  getAllRegistartions(search?: string, pageSize?: number, pageNumber?: number): Observable<any> {
    let params = new HttpParams();

    if (search) params = params.append('searchQuery', search);
    if (pageSize) params = params.append('pageSize', pageSize);
    if (pageNumber) params = params.append('pageNumber', pageNumber);
    
    return this.http.get(`${this.API_URL}/api/RegistrationVehicle`, { params });
  }

  getRegistrationById(registrationId: string): Observable<any> {
    return this.http.get(`${this.API_URL}/api/RegistrationVehicle/${registrationId}`);
  }

  addNewRegistration(registration: any): Observable<any> {
    return this.http.post(`${this.API_URL}/api/RegistrationVehicle`, registration);
  }

  editRegistration(registration: any): Observable<any> {
    return this.http.put(`${this.API_URL}/api/RegistrationVehicle`, registration);
  }

  deleteRegistration(id: any): Observable<any> {
    return this.http.delete(`${this.API_URL}/api/RegistrationVehicle`, {
      params: { id }
    });
  }
}