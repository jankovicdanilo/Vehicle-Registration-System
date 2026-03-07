import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../enviroments/enviroment';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  API_URL: string = environment.apiURL;

  constructor(private http: HttpClient) {}

  getAllClients(search?: string, pageSize?: number, pageNumber?: number): Observable<any> {
    let params = new HttpParams();

    if (search) params = params.append('searchQuery', search);
    if (pageSize) params = params.append('pageSize', pageSize);
    if (pageNumber) params = params.append('pageNumber', pageNumber);

    return this.http.get(`${this.API_URL}/api/Client`, { params });
  }

  getClientById(clientId: string): Observable<any> {
    return this.http.get(`${this.API_URL}/api/Client/${clientId}`);
  }

  addNewClient(client: any): Observable<any> {
    return this.http.post(`${this.API_URL}/api/Client`, client);
  }

  editClient(client: any): Observable<any> {
    return this.http.put(`${this.API_URL}/api/Client`, client);
  }

  deleteClient(id: any): Observable<any> {
    return this.http.delete(`${this.API_URL}/api/Client`, {
      params: { id }
    });
  }
}