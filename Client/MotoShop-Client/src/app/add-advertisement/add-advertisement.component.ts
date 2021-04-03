import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { bodyTypes } from '../shared/Constants/carBodyTypes';
import { carBrands } from '../shared/Constants/carBrands';
import { fuels } from '../shared/Constants/fuels';
import { gearboxes } from '../shared/Constants/gearboxes';
import { Car } from '../shared/models/advertisements/Items/car.model';
import { NewCarAdvertisementModel } from '../shared/models/advertisements/newCarAdvertisementModel';
import { AdvertisementsService } from '../shared/services/advertisements.service';
import { IdentityService } from '../shared/services/identity.service';

@Component({
  selector: 'app-add-advertisement',
  templateUrl: './add-advertisement.component.html',
  styleUrls: ['./add-advertisement.component.sass']
})
export class AddAdvertisementComponent implements OnInit {

  constructor(private fb: FormBuilder,
    private identityService: IdentityService,
    private adService: AdvertisementsService,
    private router: Router,
    private toastr: ToastrService) { }

  public brands = [];
  public models = [];
  public fuels = fuels;
  public gearboxes = gearboxes;
  public bodyTypes = bodyTypes;
  public baseInfoForm: FormGroup;
  public carFilterForm: FormGroup;

  public showLoadingSpinner: boolean = false;

  ngOnInit(): void {
    this.brands = carBrands.map(x => x.brand);

    this.baseInfoForm = this.fb.group({
      "title": ["", Validators.required],
      'description': ["", Validators.required],
    })

    this.carFilterForm = this.fb.group(
      {
        "brand": new FormControl({ value: "" }, [Validators.required]),
        "model": new FormControl({ value: "", disabled: true }, [Validators.required]),
        "gearbox": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "bodyType": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "price": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "productionYear": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "fuelType": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "hp": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "cubicCapacity": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "fuelConsumption": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "length": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "width": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "acceleration": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "numberOfDoors": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "numberOfSeats": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "mileage": new FormControl({ value: "", disabled: false }, [Validators.required]),
      });
  }

  getFile() {
    const fileInput = document.getElementById("fileInput");
    fileInput.click();
  }


  submit(itemType: string) {
    this.showLoadingSpinner = true;
    switch (itemType) {
      case 'car': {
        this.proceedCarAdvertisement();
      } break;
    }
  }

  proceedCarAdvertisement() {
    let model: NewCarAdvertisementModel =
    {
      Description: this.baseInfoForm.get("description").value,
      Price: this.carFilterForm.get("price").value,
      Title: this.baseInfoForm.get("title").value,
      Car: new Car
    };
    model.Car = {
      Acceleration: this.carFilterForm.get('acceleration').value,
      CarType: this.carFilterForm.get('bodyType').value,
      CarBrand: this.carFilterForm.get('brand').value,
      CarModel: this.carFilterForm.get('model').value,
      CubicCapacity: this.carFilterForm.get('cubicCapacity').value,
      FuelConsumption: this.carFilterForm.get('fuelConsumption').value,
      Fuel: this.carFilterForm.get('fuelType').value,
      Gearbox: this.carFilterForm.get('gearbox').value,
      HorsePower: this.carFilterForm.get('hp').value,
      Lenght: this.carFilterForm.get('length').value,
      NumberOfDoors: this.carFilterForm.get('numberOfDoors').value,
      NumberOfSeats: this.carFilterForm.get('numberOfSeats').value,
      Price: this.carFilterForm.get('price').value,
      Width: this.carFilterForm.get('width').value,
      YearOfProduction: this.carFilterForm.get('productionYear').value,
      ItemType: "Car",
      ImageUrl: "wwwroot/resources/images/CarImage.jpg",
      ID: 0,
      OwnerID: this.identityService.getUserID,
      Mileage: this.carFilterForm.get('mileage').value,
    }
    this.adService.addAdvertisement(model).subscribe(res => {
      this.showLoadingSpinner = false;
      this.toastr.success("Successfully added your advertisement");
      this.router.navigateByUrl("/home");
    }
      , error => this.errorCallback(error));
  }

  errorCallback(error) {
    this.showLoadingSpinner = false;
    console.log(error);
    this.toastr.error("Something went wrong while trying to add your advertisement. Try again.")
  }

  checkErrorForSelectInput(control: string, defaultValue = ""): boolean {
    if (defaultValue === "")
      return this.carFilterForm.get(control).touched;

    return this.carFilterForm.get(control).touched && this.carFilterForm.get(control).value === defaultValue;
  }


  isCarFilterFormValid(): boolean {
    let flag = false;

    //TODO: refactor this
    if (this.carFilterForm.valid) {
      if (this.carFilterForm.get("brand").value == "Select Brand" || this.carFilterForm.get("model").value == "Select Model"
        || this.carFilterForm.get("gearbox").value == "Gearbox" || this.carFilterForm.get("fuelType").value == "Fuel Type"
        || this.carFilterForm.get("bodyType").value == "Body Type") {

        return false;
      }
      flag = true;
    }
    return flag;
  }

  onSelection(control: string) {

    let res = this.carFilterForm.get(control)?.value;

    switch (control) {
      case "brand": {
        this.onSelectInputChange('model', res)
      } break;
    }
  }
  onSelectInputChange(control, valueToSet) {

    if (this.isDefaultValue(valueToSet)) {
      this.clearForm();
      return;
    }

    switch (control) {
      case 'model': {
        let selectedCarObj = carBrands.filter(x => x.brand == valueToSet)[0];
        this.models = selectedCarObj.models;
        this.enableFormInput(control);
      }
        break;
    }
  }
  isDefaultValue(value): boolean {
    return value == "Select Brand" || value == "Select Model";
  }
  enableFormInput(formControl: string) {
    this.carFilterForm.get(formControl).enable();
  }
  clearForm() {
    this.models = [];
    this.disableModelsControl();
  }
  disableModelsControl() {
    this.carFilterForm.get('model').disable();
  }
}
