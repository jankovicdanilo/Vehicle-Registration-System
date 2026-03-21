import { Client } from './client.model';
import { Vehicle } from './vehicle.model';
import { Insurance } from './insurance.model';

export class Registration {
  id: string;
  licensePlate: string;

  registrationDate: string;
  expirationDate: string;

  registrationPrice: number;
  isTemporary: boolean;

  clientId: string;
  client: Client;

  vehicleId: string;
  vehicle: Vehicle;

  insuranceId: string;
  insurance: Insurance;

  constructor(registration: any) {
    this.id = registration.id || undefined;
    this.licensePlate = registration.licensePlate || undefined;

    this.registrationDate = registration.registrationDate || undefined;
    this.expirationDate = registration.expirationDate || undefined;

    this.registrationPrice = registration.registrationPrice || undefined;
    this.isTemporary = registration.isTemporary || undefined;

    this.clientId = registration.clientId || undefined;
    this.client = registration.client || undefined;

    this.vehicleId = registration.vehicleId || undefined;
    this.vehicle = registration.vehicle || undefined;

    this.insuranceId = registration.insuranceId || undefined;
    this.insurance = registration.insurance || undefined;
  }
}

