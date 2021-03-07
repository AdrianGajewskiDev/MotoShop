import { Component, Input, OnInit } from '@angular/core';
import { HealthReportEntryModel } from 'src/app/shared/models/server/health/HealthReportEntry.model';

@Component({
  selector: 'app-server-health-item',
  templateUrl: './server-health-item.component.html',
  styleUrls: ['./server-health-item.component.sass']
})
export class ServerHealthItemComponent implements OnInit {

  @Input()
  model: HealthReportEntryModel;

  constructor() { }

  ngOnInit(): void {
    let splitedWords = this.model.HealthCheckName.split(/(?=[A-Z])/);

    let newHealthCheckName: string = "";

    splitedWords.forEach(element => {
      element = element.replace(",", " ");

      newHealthCheckName += " " + element;
    });

    this.model.HealthCheckName = newHealthCheckName;
  }
}
