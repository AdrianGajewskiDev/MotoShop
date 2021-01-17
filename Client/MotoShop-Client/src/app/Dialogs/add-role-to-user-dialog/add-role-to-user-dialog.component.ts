import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface DialogData {
  Role: string;
}


@Component({
  selector: 'app-add-role-to-user-dialog',
  templateUrl: './add-role-to-user-dialog.component.html',
  styleUrls: ['./add-role-to-user-dialog.component.sass']
})
export class AddRoleToUserDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<AddRoleToUserDialogComponent>) { }

  public data: DialogData = {
    Role: ''
  }

  ngOnInit(): void {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
