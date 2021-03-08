import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ServerHealthItemDetailsDialogComponent } from 'src/app/Dialogs/server-health-item-details-dialog/server-health-item-details-dialog.component';
import { HealthReportEntryModel } from 'src/app/shared/models/server/health/HealthReportEntry.model';
import { ServiceLocator } from 'src/app/shared/services/locator.service';

@Component({
  selector: 'app-server-health-item',
  templateUrl: './server-health-item.component.html',
  styleUrls: ['./server-health-item.component.sass']
})
export class ServerHealthItemComponent implements OnInit {

  @Input()
  model: HealthReportEntryModel;

  private dialog: MatDialog;

  constructor() {
    this.dialog = ServiceLocator.injector.get(MatDialog);
  }

  ngOnInit(): void {
    let splitedWords = this.model.HealthCheckName.split(/(?=[A-Z])/);

    let newHealthCheckName: string = "";

    splitedWords.forEach(element => {
      element = element.replace(",", " ");

      newHealthCheckName += " " + element;
    });

    this.model.HealthCheckName = newHealthCheckName;
  }

  goToDetails(): void {
    this.dialog.open(ServerHealthItemDetailsDialogComponent,
      {
        width: "300px",
        height: "200px",
        data: {
          model: this.model
        }

      });
  }
}
