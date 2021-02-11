import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Advertisement } from 'src/app/shared/models/advertisements/advertisement.model';
import { ServiceLocator } from 'src/app/shared/services/locator.service';

interface DialogData {
  AdvertisementModel: Advertisement;
}

@Component({
  selector: 'app-edit-advertisement-dialog',
  templateUrl: './edit-advertisement-dialog.component.html',
  styleUrls: ['./edit-advertisement-dialog.component.sass']
})
export class EditAdvertisementDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<EditAdvertisementDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData) {
    this.formBuilder = ServiceLocator.injector.get(FormBuilder);
  }

  public model: Advertisement;
  public editAdForm: FormGroup;
  public editItemForm: FormGroup;

  private formBuilder: FormBuilder;

  ngOnInit(): void {

    this.model = this.data.AdvertisementModel;

    console.log(this.model);

    this.editItemForm = this.formBuilder.group(
      {
        "itemID": new FormControl({ value: this.model.ShopItem.ID, disabled: true }),
        "price": new FormControl({ value: this.model.ShopItem.Price, disabled: false }),
        "itemType": new FormControl({ value: this.model.ShopItem.ItemType, disabled: true }),
        "imageUrl": new FormControl({ value: this.model.ShopItem.ImageUrl, disabled: false }),
      });

    this.editAdForm = this.formBuilder.group(
      {
        "id": new FormControl({ value: this.model.ID, disabled: true }),
        "authorID": new FormControl({ value: this.model.AuthorID, disabled: true }, Validators.required),
        "title": new FormControl({ value: this.model.Title, disabled: false }, Validators.required),
        "description": new FormControl({ value: this.model.Description, disabled: false }, Validators.required),
        "placed": new FormControl({ value: this.model.Placed, disabled: false }, Validators.required),
      });
  }

}
