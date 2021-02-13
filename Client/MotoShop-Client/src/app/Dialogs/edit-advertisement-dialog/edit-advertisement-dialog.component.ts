import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Advertisement } from 'src/app/shared/models/advertisements/advertisement.model';
import { AdvertisementsService } from 'src/app/shared/services/advertisements.service';
import { ServiceLocator } from 'src/app/shared/services/locator.service';

interface DialogData {
  AdvertisementModel: Advertisement;
}

export interface UpdateDataResult {
  Key: string;
  Content: string | number;
}

@Component({
  selector: 'app-edit-advertisement-dialog',
  templateUrl: './edit-advertisement-dialog.component.html',
  styleUrls: ['./edit-advertisement-dialog.component.sass']
})
export class EditAdvertisementDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<EditAdvertisementDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData) {
    this.formBuilder = ServiceLocator.injector.get(FormBuilder);
    this.service = ServiceLocator.injector.get(AdvertisementsService);
  }

  public model: Advertisement;
  public editAdForm: FormGroup;
  public editItemForm: FormGroup;
  public showLoadingSpinner = false;

  private closing: boolean = false;
  private formBuilder: FormBuilder;
  private service: AdvertisementsService;

  ngOnInit(): void {
    this.closing = false;
    this.model = this.data.AdvertisementModel;
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
        "title": new FormControl({ value: this.model.Title, disabled: false }, Validators.required),
        "authorID": new FormControl({ value: this.model.AuthorID, disabled: true }, Validators.required),
        "description": new FormControl({ value: this.model.Description, disabled: false }, Validators.required),
        "placed": new FormControl({ value: this.model.Placed, disabled: true }, Validators.required),
      });
  }

  cancel(): void {
    this.closing = true;
    this.dialogRef.close();
  }

  apply(): void {
    if (this.closing == true)
      return;

    this.showLoadingSpinner = true;

    let updatedAdvert: Advertisement =
    {
      AuthorID: this.model.AuthorID,
      Description: this.editAdForm.get('description').value,
      ID: this.model.ID,
      Placed: this.editAdForm.get('placed').value,
      Title: this.editAdForm.get('title').value,
      ShopItem: {
        ID: this.model.ShopItem.ID,
        ImageUrl: this.editItemForm.get('imageUrl').value,
        ItemType: this.model.ShopItem.ItemType,
        OwnerID: this.model.ShopItem.OwnerID,
        Price: this.editItemForm.get('price').value
      }
    };

    let dataToUpdate = this.determineDataToUpdate(this.model, updatedAdvert);

    this.service.update(dataToUpdate, this.model.ID).subscribe(
      res => {
        this.disableLoadingSpinner();
        this.cancel();
        window.location.reload();
      },
      error => {
        this.disableLoadingSpinner();
        console.log(error)
      });
  }

  disableLoadingSpinner() {
    this.showLoadingSpinner = false;
  }

  determineDataToUpdate(model: Advertisement, newModel: Advertisement): UpdateDataResult[] {
    let oldProperties: Record<number, UpdateDataResult> =
    {
      0: { Key: "title", Content: model.Title },
      1: { Key: "description", Content: model.Description },
      2: { Key: "placed", Content: model.Placed },
      3: { Key: "price", Content: model.ShopItem.Price },
      4: { Key: "imageUrl", Content: model.ShopItem.ImageUrl }
    };

    let newProperties: Record<number, UpdateDataResult> =
    {
      0: { Key: "title", Content: newModel.Title },
      1: { Key: "description", Content: newModel.Description },
      2: { Key: "placed", Content: newModel.Placed },
      3: { Key: "price", Content: newModel.ShopItem.Price },
      4: { Key: "imageUrl", Content: newModel.ShopItem.ImageUrl }
    };

    let results: UpdateDataResult[] = [];

    for (let i = 0; i < 5; i++) {
      if (oldProperties[i].Content != newProperties[i].Content) {
        results.push(
          {
            Key: newProperties[i].Key,
            Content: newProperties[i].Content
          })
      }
      else
        continue;
    }

    return results;
  }
}
