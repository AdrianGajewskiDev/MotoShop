import { Component, OnInit } from '@angular/core';
import { AbstractControl, ControlContainer, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { carBrands } from "../shared/Constants/carBrands"
import { fuels } from '../shared/Constants/fuels';
import { gearboxes } from '../shared/Constants/gearboxes';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.sass']
})
export class HomeComponent implements OnInit {

  constructor(private fb: FormBuilder) { }

  public brands = [];
  public models = [];
  public gearboxes = gearboxes;
  public fuels = fuels;

  public carFilterForm: FormGroup;

  ngOnInit(): void {

    this.carFilterForm = this.fb.group(
      {
        "brand": new FormControl({ value: "" }, [Validators.required]),
        "model": new FormControl({ value: "", disabled: true }, [Validators.required]),
        "gearbox": new FormControl({ value: "", disabled: true }, [Validators.required]),
        "fromPrice": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "toPrice": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "fromProductionYear": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "toProductionYear": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "fuelType": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "fromMileage": new FormControl({ value: "", disabled: false }, [Validators.required]),
        "toMileage": new FormControl({ value: "", disabled: false }, [Validators.required]),
      });
    this.brands = carBrands.map(x => x.brand);

  }

  onSelection(control: string) {

    let res = this.carFilterForm.get(control)?.value;

    switch (control) {
      case "brand": {
        this.onSelectInputChange('model', res)
      } break;
      case "model": {
        this.onSelectInputChange('gearbox', res)
      }
    }
  }

  search() {

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
      case 'gearbox': {
        this.enableFormInput(control);
      }
        break;
    }
  }

  clearForm() {
    this.models = [];
    this.disableAll();
  }
  isDefaultValue(value): boolean {
    return value == "Select Brand" || value == "Select Model";
  }

  enableFormInput(formControl: string) {
    this.carFilterForm.get(formControl).enable();
  }

  disableAll() {
    Object.keys(this.carFilterForm.controls).forEach(key => {
      let currentControl = this.carFilterForm.controls[key];
      let currentControlName = this.getName(currentControl);

      if (currentControlName && currentControlName !== "brand") {
        currentControl.disable();
      }
    });
  }

  getName(control: AbstractControl): string | null {
    let group = <FormGroup>control.parent;

    if (!group) {
      return null;
    }

    let name: string;

    Object.keys(group.controls).forEach(key => {
      let childControl = group.get(key);

      if (childControl !== control) {
        return;
      }

      name = key;
    });

    return name;
  }
}
