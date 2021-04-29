import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SignalRService } from './shared/services/signalR.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent implements OnInit {
  constructor(
    private signalRService: SignalRService) { }

  title = 'MotoShop-Client';

  ngOnInit() {
    this.signalRService.acquireConnection();
    this.signalRService.startConnection();
    this.signalRService.test().subscribe(res => console.log(res));
  }
}
