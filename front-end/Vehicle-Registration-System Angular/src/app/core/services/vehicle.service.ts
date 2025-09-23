import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../enviroments/enviroment';

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  API_URL: string = environment.apiURL;

  constructor(private http: HttpClient) {}

  getAllVehicles(search?: string, pageSize?: number, pageNumber?: number): Observable<any> {
    let params = new HttpParams();

    if (search) params = params.append('searchQuery', search);
    if (pageSize) params = params.append('pageSize', pageSize);
    if (pageNumber) params = params.append('pageNumber', pageNumber);

    return this.http.get(`${this.API_URL}/api/Vehicle`, { params });
  }

  addNewVehicle(vehicle: any): Observable<any> {
    return this.http.post(`${this.API_URL}/api/Vehicle`, vehicle);
  }

  getVehicleById(vehicleId: string): Observable<any> {
    return this.http.get(`${this.API_URL}/api/Vehicle/${vehicleId}`);
  }

  getVehicleTypes(): Observable<any> {
    return this.http.get(`${this.API_URL}/api/VehicleType`);
  }

  getVehicleBrands(vehicleTypeId: string): Observable<any> {
    return this.http.get(`${this.API_URL}/api/VehicleBrand/ListById/${vehicleTypeId}`);
  }

  getVehicleModels(brandId: string): Observable<any> {
    return this.http.get(`${this.API_URL}/api/VehicleModel/list/${brandId}`);
  }

  editVehicle(vehicle: any): Observable<any> {
    return this.http.put(`${this.API_URL}/api/Vehicle`, vehicle);
  }

  deleteVehicle(id: any): Observable<any> {
    return this.http.delete(`${this.API_URL}/api/Vehicle`, {
      params: { id }
    });
  }
}