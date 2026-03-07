import { Client } from './client.model';
import { Vehicle } from './vehicle.model';

export class Registration {
  id: string;
  cijenaRegistracije: number;
  datumIstekaRegistracije: string;
  datumRegistracije: string;
  klijentId: string;
  osiguranje: Osiguranje;
  osiguranjeId: string;
  privremenaRegistracija: boolean;
  registarskaOznaka: string;
  voziloId: string;
  vlasnik: Client;
  vozilo: Vehicle;

  constructor(registration) {   
    this.id = registration.id || undefined;
    this.cijenaRegistracije = registration.cijenaRegistracije || undefined;
    this.datumIstekaRegistracije = registration.datumIstekaRegistracije || undefined;
    this.datumRegistracije = registration.datumRegistracije || undefined;
    this.klijentId = registration.klijentId || undefined;
    this.osiguranje = registration.osiguranje || undefined;      
    this.osiguranjeId = registration.osiguranjeId || undefined;
    this.privremenaRegistracija = registration.privremenaRegistracija || undefined;
    this.registarskaOznaka = registration.registarskaOznaka || undefined;
    this.voziloId = registration.voziloId || undefined;
    this.vlasnik = registration.vlasnik || undefined;
    this.vozilo = registration.vozilo || undefined;              
  }
}

class Osiguranje {
  id: string;
  naziv: string;
}