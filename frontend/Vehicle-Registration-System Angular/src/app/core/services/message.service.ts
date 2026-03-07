import {Injectable} from "@angular/core";
import {ToastrService} from "ngx-toastr";

@Injectable({
    providedIn: "root",
})
export class MessageService {
    constructor(private toast: ToastrService) {
    }

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
        let errorMessage: string = "Dogodila se greška";

        if (error && error.error) {
            const code = error.error.errorCode;

            switch (code) {
                case 'INVALID_USERNAME':
                    errorMessage = "Korisnik sa unijetim korisničkim imenom ne postoji";
                    break;
                case 'INVALID_EMAIL':
                    errorMessage = "Korisnik sa unijetom email adresom ne postoji";
                    break;
                case 'INVALID_PASSWORD':
                    errorMessage = "Pogrešna lozinka";
                    break;
                case 'CHANGE_PASSWORD_FAILED':
                    errorMessage = "Dogodila se greška pri promjeni lozinke";
                    break;
                case 'RESET_PASSWORD_FAILED':
                    errorMessage = "Dogodila se greška pri resetovanju lozinke";
                    break;
                case 'USER_NOT_FOUND':
                    errorMessage = "Korisnik nije pronađen";
                    break;
                case "JMBG_EXISTS":
                    errorMessage = "Korisnik sa ovim matičnim brojem već postoji";
                    break;
                case "EMAIL_EXISTS":
                    errorMessage = "Korisnik sa ovom email adresom već postoji";
                    break;
                case "ID_CARD_EXISTS":
                    errorMessage = "Korisnik sa ovim brojem lične karte već postoji";
                    break;
                case 'CLIENT_NOT_FOUND':
                    errorMessage = "Klijent nije pronađen";
                    break;
                case 'CLIENT_REGISTRATION_EXISTS':
                    errorMessage = "Klijent ne može biti obrisan dok ima aktivnu registraciju";
                    break;
                case 'VEHICLE_REGISTRATION_EXISTS':
                    errorMessage = "Vozilo ne moze biti obrisano dok ima aktivnu registraciju";
                    break;
                case 'VEHICLE_BRAND_EXISTS':
                    errorMessage = "Vozilo sa ovom markom već postoji";
                    break;
                case 'VEHICLE_TYPE_NOT_FOUND':
                    errorMessage = "Tip vozila nije pronađen";
                    break;
                case 'VEHICLE_BRAND_HAS_MODELS':
                    errorMessage = "Vozilo sa ovom markom ima modele i ne može biti obrisano";
                    break;
                case 'VEHICLE_BRAND_NOT_FOUND':
                    errorMessage = "Vozilo sa ovom markom ne postoji";
                    break;
                case 'VEHICLE_BRAND_IN_USE':
                    errorMessage = "Vozilo sa ovom markom je u upotrebi i ne može biti obrisano";
                    break;
                case 'VEHICLE_MODEL_NOT_FOUND':
                    errorMessage = "Vozilo sa ovim modelom ne postoji";
                    break;
                case 'VEHICLE_MODEL_IN_USE':
                    errorMessage = "Vozilo sa ovim modelom je u upotrebi i ne može biti obrisano";
                    break;
                case 'VEHICLE_MODEL_EXISTS':
                    errorMessage = "Vozilo sa ovim modelom već postoji";
                    break;
                case 'VEHICLE_ALREADY_REGISTERED':
                    errorMessage = "Vozilo je već registrovano";
                    break;
                case 'REGISTRATION_INVALID_DATE':
                    errorMessage = "Datum registracije nije validan. Ne može biti izabran datum u budućnosti.";
                    break;
                case 'REGISTRATION_INVALID_PRICE':
                    errorMessage = "Cijena registracije nije validna. Morate unijeti broj veći od 0.";
                    break;
                case 'REGISTRATION_VEHICLE_INVALID_ID':
                    errorMessage = "Vozilo nije pronađeno";
                    break;
                case 'REGISTRATION_CLIENT_INVALID_ID':
                    errorMessage = "Klijent nije pronađen";
                    break;
                case 'REGISTRATION_INVALID_ID':
                    errorMessage = "Registracija nije pronađena";
                    break;
                case 'TIP_NOT_FOUND':
                    errorMessage = "Tip vozila nije pronađen";
                    break;
                case 'MARKA_NOT_FOUND':
                    errorMessage = "Marka vozila nije pronađena";
                    break;
                case 'MODEL_NOT_FOUND':
                    errorMessage = "Model vozila nije pronađen";
                    break;
                case 'INVALID_COMBINATION':
                    errorMessage = "Izabrani model vozila ne odgovara izabranoj marki vozila";
                    break;
                case 'PLATE_EXISTS':
                    errorMessage = "Registracija sa ovom registarskom oznakom već postoji";
                    break;
                case 'CHASSIS_NUMBER_EXISTS':
                    errorMessage = "Vozilo sa ovim brojem šasije već postoji";
                    break;
                case 'INVALID_YEAR':
                    errorMessage = "Godina proizvodnje vozila nije validna";
                    break;
                case 'DATE_MISMATCH':
                    errorMessage = "Datum prve registracije mora biti prije datuma trenutne registracije";
                    break;
                case 'INVALID_ENGINE_POWER':
                    errorMessage = "Snaga motora mora biti veća od 0";
                    break;
                case 'PRODUCTION_YEAR_AFTER_FIRST_REGISTRATION':
                    errorMessage = "Godina proizvodnje mora biti prije datuma prve registracije";
                    break;
                case 'PRODUCTION_DATE_AFTER_FIRST_REGISTRATION':
                    errorMessage = "Godina proizvodnje mora biti prije datuma prve registracije";
                    break;
                case 'VEHICLE_TYPE_CATEGORY_EXISTS':
                    errorMessage = "Tip vozila sa ovom kategorijom već postoji";
                    break;
                case 'VEHICLE_TYPE_NAME_EXISTS':
                    errorMessage = "Tip vozila sa ovim nazivom već postoji";
                    break;
                case 'TYPE_HAS_BRANDS':
                    errorMessage = "Tip vozila ima marku i ne može biti obrisan";
                    break;
                default:
                    errorMessage = "Greška";
            }
        }
        this.toast.error(errorMessage);
    }
}
