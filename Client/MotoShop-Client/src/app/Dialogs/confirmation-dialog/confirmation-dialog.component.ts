import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

interface Data {
  func: (s: boolean) => void;
  run: boolean;
  message: string;
}

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.sass']
})
export class ConfirmationDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<ConfirmationDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: Data) { }

  ngOnInit(): void {

  }

  excute() {
    this.data.run = false;
    this.data.func(this.data.run);
    this.dialogRef.close();
  }

  cancel() {
    this.dialogRef.close();
  }

}
