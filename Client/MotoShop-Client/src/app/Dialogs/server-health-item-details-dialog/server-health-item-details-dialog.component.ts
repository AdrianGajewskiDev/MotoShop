import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HealthReportEntryModel } from 'src/app/shared/models/server/health/HealthReportEntry.model';

interface DialogData {
  model: HealthReportEntryModel;
}
@Component({
  selector: 'app-server-health-item-details-dialog',
  templateUrl: './server-health-item-details-dialog.component.html',
  styleUrls: ['./server-health-item-details-dialog.component.sass']
})
export class ServerHealthItemDetailsDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<ServerHealthItemDetailsDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: DialogData) { }

  public description: string = "";
  ngOnInit(): void {
    this.description = this.data.model.Status == 2 ? "No problems were detected" : this.data.model?.Description;
  }

}
