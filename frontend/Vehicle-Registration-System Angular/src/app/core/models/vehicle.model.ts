export class Vehicle {
  id: string;
  vehicleTypeId: string;
  vehicleBrandId: string;
  vehicleModelId: string;

  vehicleTypeName: string;
  vehicleBrandName: string;
  vehicleModelName: string;

  productionYear: number;
  engineCapacity: number;
  fuelType: string;
  weight: number;
  enginePowerKw: number;

  chassisNumber: string;
  firstRegistrationDate: string;

  constructor(vehicle: any) {
    this.id = vehicle.id || undefined;
    this.vehicleTypeId = vehicle.vehicleTypeId || undefined;
    this.vehicleBrandId = vehicle.vehicleBrandId || undefined;
    this.vehicleModelId = vehicle.vehicleModelId || undefined;

    this.vehicleTypeName = vehicle.vehicleTypeName || undefined;
    this.vehicleBrandName = vehicle.vehicleBrandName || undefined;
    this.vehicleModelName = vehicle.vehicleModelName || undefined;

    this.productionYear = vehicle.productionYear || undefined;
    this.engineCapacity = vehicle.engineCapacity || undefined;
    this.fuelType = vehicle.fuelType || undefined;
    this.weight = vehicle.weight || undefined;
    this.enginePowerKw = vehicle.enginePowerKw || undefined;

    this.chassisNumber = vehicle.chassisNumber || undefined;
    this.firstRegistrationDate = vehicle.firstRegistrationDate || undefined;
  }
}