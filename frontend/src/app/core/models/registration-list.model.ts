export interface RegistrationListItem {
  id: string;
  licensePlate: string;
  registrationDate: string;
  expirationDate: string;
  registrationPrice: number;
  isTemporary: boolean;

  vehicle: string;
  owner: string;
  insurance: string;
}