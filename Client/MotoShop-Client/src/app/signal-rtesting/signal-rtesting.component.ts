import { Component, OnInit } from '@angular/core';
import { SignalRService } from '../shared/services/signalR.service';

@Component({
  selector: 'app-signal-rtesting',
  templateUrl: './signal-rtesting.component.html',
  styleUrls: ['./signal-rtesting.component.sass']
})
export class SignalRTestingComponent implements OnInit {

  constructor(private service: SignalRService) { }

  ngOnInit(): void {
    this.service.startConnection();
  }

}
