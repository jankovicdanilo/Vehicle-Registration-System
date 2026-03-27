import { Injectable } from "@angular/core";
import { ToastrService } from "ngx-toastr";

@Injectable({
  providedIn: "root",
})
export class MessageService {
  constructor(private toast: ToastrService) {}

  warning(message: string): void {
    this.toast.warning(message);
  }

  success(message: string): void {
    this.toast.success(message);
  }

  info(message: string): void {
    this.toast.info(message);
  }

  error(error: any): void {
    let errorMessage: string = "An unexpected error occurred";

    if (error && error.error) {
      const code = error.error.errorCode;

      switch (code) {
        case "INVALID_USERNAME":
          errorMessage = "User with the provided username does not exist";
          break;
        case "INVALID_EMAIL":
          errorMessage = "User with the provided email does not exist";
          break;
        case "INVALID_PASSWORD":
          errorMessage = "Incorrect password";
          break;
        case "CHANGE_PASSWORD_FAILED":
          errorMessage = "Failed to change password";
          break;
        case "RESET_PASSWORD_FAILED":
          errorMessage = "Failed to reset password";
          break;
        case "USER_NOT_FOUND":
          errorMessage = "User not found";
          break;

        case "JMBG_EXISTS":
          errorMessage = "A user with this national ID already exists";
          break;
        case "EMAIL_EXISTS":
          errorMessage = "A user with this email already exists";
          break;
        case "ID_CARD_EXISTS":
          errorMessage = "A user with this ID card number already exists";
          break;

        case "CLIENT_NOT_FOUND":
          errorMessage = "Client not found";
          break;
        case "CLIENT_REGISTRATION_EXISTS":
          errorMessage = "Client cannot be deleted while having an active registration";
          break;

        case "VEHICLE_REGISTRATION_EXISTS":
          errorMessage = "Vehicle cannot be deleted while having an active registration";
          break;
        case "VEHICLE_BRAND_EXISTS":
          errorMessage = "Vehicle brand already exists";
          break;
        case "VEHICLE_TYPE_NOT_FOUND":
          errorMessage = "Vehicle type not found";
          break;
        case "VEHICLE_BRAND_HAS_MODELS":
          errorMessage = "Vehicle brand has models and cannot be deleted";
          break;
        case "VEHICLE_BRAND_NOT_FOUND":
          errorMessage = "Vehicle brand not found";
          break;
        case "VEHICLE_BRAND_IN_USE":
          errorMessage = "Vehicle brand is in use and cannot be deleted";
          break;

        case "VEHICLE_MODEL_NOT_FOUND":
          errorMessage = "Vehicle model not found";
          break;
        case "VEHICLE_MODEL_IN_USE":
          errorMessage = "Vehicle model is in use and cannot be deleted";
          break;
        case "VEHICLE_MODEL_EXISTS":
          errorMessage = "Vehicle model already exists";
          break;

        case "VEHICLE_ALREADY_REGISTERED":
          errorMessage = "Vehicle is already registered";
          break;

        case "REGISTRATION_INVALID_DATE":
          errorMessage = "Invalid registration date. Future dates are not allowed";
          break;
        case "REGISTRATION_INVALID_PRICE":
          errorMessage = "Invalid registration price. Must be greater than 0";
          break;

        case "REGISTRATION_VEHICLE_INVALID_ID":
          errorMessage = "Vehicle not found";
          break;
        case "REGISTRATION_CLIENT_INVALID_ID":
          errorMessage = "Client not found";
          break;
        case "REGISTRATION_INVALID_ID":
          errorMessage = "Registration not found";
          break;

        case "TIP_NOT_FOUND":
          errorMessage = "Vehicle type not found";
          break;
        case "MARKA_NOT_FOUND":
          errorMessage = "Vehicle brand not found";
          break;
        case "MODEL_NOT_FOUND":
          errorMessage = "Vehicle model not found";
          break;

        case "INVALID_COMBINATION":
          errorMessage = "Selected model does not belong to the selected brand";
          break;

        case "PLATE_EXISTS":
          errorMessage = "A registration with this license plate already exists";
          break;

        case "CHASSIS_NUMBER_EXISTS":
          errorMessage = "A vehicle with this chassis number already exists";
          break;

        case "INVALID_YEAR":
          errorMessage = "Invalid production year";
          break;

        case "DATE_MISMATCH":
          errorMessage = "First registration date must be before the current registration date";
          break;

        case "INVALID_ENGINE_POWER":
          errorMessage = "Engine power must be greater than 0";
          break;

        case "PRODUCTION_YEAR_AFTER_FIRST_REGISTRATION":
        case "PRODUCTION_DATE_AFTER_FIRST_REGISTRATION":
          errorMessage = "Production year must be before the first registration date";
          break;

        case "VEHICLE_TYPE_CATEGORY_EXISTS":
          errorMessage = "Vehicle type with this category already exists";
          break;

        case "VEHICLE_TYPE_NAME_EXISTS":
          errorMessage = "Vehicle type with this name already exists";
          break;

        case "TYPE_HAS_BRANDS":
          errorMessage = "Vehicle type has associated brands and cannot be deleted";
          break;

        default:
          errorMessage = "An error occurred";
      }
    }

    this.toast.error(errorMessage);
  }
}