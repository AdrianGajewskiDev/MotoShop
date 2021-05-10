import { AfterViewChecked, Component, ElementRef, OnInit, DoCheck, IterableDiffers, IterableDiffer, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Conversation } from 'src/app/shared/models/messages/conversation.model';
import { Message } from 'src/app/shared/models/messages/message.model';
import { NewMessageModel } from 'src/app/shared/models/messages/newMessage.model';
import { ConversationService } from 'src/app/shared/services/conversation.service';
import { IdentityService } from 'src/app/shared/services/identity.service';
import { ServiceLocator } from 'src/app/shared/services/locator.service';


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

  public conversation: Conversation;
  private service: ConversationService;
  private toastr: ToastrService;
  private identityService: IdentityService
  public usernameToDisplay: string;

  constructor(private elementRef: ElementRef,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) {

    this.service = ServiceLocator.injector.get(ConversationService);
    this.toastr = ServiceLocator.injector.get(ToastrService);
    this.identityService = ServiceLocator.injector.get(IdentityService);
  }

  ngOnInit(): void {
    this.service.getConversation(this.data.SenderID, this.data.ReceiverID, this.data.Topic).subscribe((res: Conversation) => {
      this.conversation = res


      for (let msg of res.Messages) {
        if (this.identityService.getUserID === this.conversation.SenderID) {
          msg.Sender = "owner";
          this.usernameToDisplay = this.conversation.ReceiverName;
        }
        else {
          this.usernameToDisplay = this.conversation.SenderName;
          msg.Sender = "recipient";
        }
      }



    }, error => console.log(error));

  }

  ngAfterViewChecked() {
    const msgContainers = this.elementRef.nativeElement.querySelectorAll(".messageItem-container");

    for (let container of msgContainers) {
      let childHeight = (container as HTMLElement).children[0].clientHeight + 10;

      (container as HTMLElement).style.minHeight = `${childHeight}px`;
    }

    this.scrollToBottomOfMessagePanel();
  }

  sendMessage(content) {
    (this.elementRef.nativeElement.querySelector(".msg-input") as HTMLTextAreaElement).value = "";

    let model = new NewMessageModel();

    model.ReceiverID = this.conversation.ReceiverID;
    model.ConversationId = this.conversation.Id;
    model.Content = content;
    this.conversation.Messages.push({ Content: model.Content, Read: false, Sent: new Date(Date.now.toString()), Sender: "owner", Id: 0 });
    this.service.sendMessage(model).subscribe(res => this.toastr.info("Send message"), error => console.log(error))
  }

  scrollToBottomOfMessagePanel() {
    let msgScrollbar = this.elementRef.nativeElement.querySelector(".message-dialog-content") as HTMLElement;
    msgScrollbar.scrollTo({ top: msgScrollbar.scrollHeight });
  }
}
