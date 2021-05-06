import { AfterViewChecked, Component, ElementRef, OnInit, DoCheck, IterableDiffers, IterableDiffer, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgImageSliderComponent } from 'ng-image-slider';
import { ConversationService } from 'src/app/shared/services/conversation.service';


interface DialogData {
  SenderID: string;
  ReceiverID: string;
  Topic: string;
}

@Component({
  selector: 'app-conversation-dialog',
  templateUrl: './conversation-dialog.component.html',
  styleUrls: ['./conversation-dialog.component.sass']
})
export class ConversationDialogComponent implements OnInit, AfterViewChecked {


  constructor(private elementRef: ElementRef,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private service: ConversationService) { }

  ngOnInit(): void {
    this.service.getConversation(this.data.SenderID, this.data.ReceiverID, this.data.Topic).subscribe((res) => console.log(res), error => console.log(error));
  }

  ngAfterViewChecked() {
    const msgContainers = this.elementRef.nativeElement.querySelectorAll(".messageItem-container");

    for (let container of msgContainers) {
      let childHeight = (container as HTMLElement).children[0].clientHeight + 10;

      (container as HTMLElement).style.minHeight = `${childHeight}px`;
    }
  }

  sendMessage(content) {
  }
}
