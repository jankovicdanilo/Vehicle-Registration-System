export class Vehicle {
  id: string;
  brojSasije: string;
  markaVozilaId: string;
  markaVozilaNaziv: string;
  masa: number;
  modelVozilaId: string;
  modelVozilaNaziv: string;
  snagaMotora: number;
  tipVozilaId: string;
  tipVozilaNaziv: string;
  vrstaGoriva: string;
  zapreminaMotora: number;
  datumPrveRegistracije: string;
  godinaProizvodnje: number;

  constructor(vehicle) {
    this.id = vehicle.id || undefined;
    this.brojSasije = vehicle.brojSasije || undefined;
    this.markaVozilaId = vehicle.markaVozilaId || undefined;
    this.markaVozilaNaziv = vehicle.markaVozilaNaziv || undefined;
    this.masa = vehicle.masa || undefined;
    this.modelVozilaId = vehicle.modelVozilaId || undefined;
    this.modelVozilaNaziv = vehicle.modelVozilaNaziv || undefined;
    this.snagaMotora = vehicle.snagaMotora || undefined;
    this.tipVozilaId = vehicle.tipVozilaId || undefined;
    this.tipVozilaNaziv = vehicle.tipVozilaNaziv || undefined;
    this.vrstaGoriva = vehicle.vrstaGoriva || undefined;
    this.zapreminaMotora = vehicle.zapreminaMotora || undefined;
    this.datumPrveRegistracije = vehicle.datumPrveRegistracije || undefined;
    this.godinaProizvodnje = vehicle.godinaProizvodnje || undefined;                           
  }
}