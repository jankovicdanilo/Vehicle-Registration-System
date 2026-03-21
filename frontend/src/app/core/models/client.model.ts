export class Client {
  id: string;
  firstName: string;
  lastName: string;
  nationalId: string;
  idCardNumber: string;
  email: string;
  phoneNumber: string;
  address: string;
  dateOfBirth: string;

  constructor(client: any) {
    this.id = client.id || undefined;
    this.firstName = client.firstName || undefined;
    this.lastName = client.lastName || undefined;
    this.nationalId = client.nationalId || undefined;
    this.idCardNumber = client.idCardNumber || undefined;
    this.email = client.email || undefined;
    this.phoneNumber = client.phoneNumber || undefined;
    this.address = client.address || undefined;
    this.dateOfBirth = client.dateOfBirth || undefined;
  }
}