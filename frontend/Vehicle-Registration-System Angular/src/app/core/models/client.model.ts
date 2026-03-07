export class Client {
  id: string;
  ime: string;
  prezime: string;
  jmbg: string;
  adresa: string;
  brojLicneKarte: string;
  brojTelefona: string;
  datumRodjenja: string;
  email: string;

  constructor(client) {
    this.id = client.id || undefined;
    this.ime = client.ime || undefined;
    this.prezime = client.prezime || undefined;
    this.jmbg = client.jmbg || undefined;
    this.adresa = client.adresa || undefined;
    this.brojLicneKarte = client.brojLicneKarte || undefined;
    this.brojTelefona = client.brojTelefona || undefined;
    this.datumRodjenja = client.datumRodjenja || undefined;
    this.email = client.email || undefined;                           
  }
}