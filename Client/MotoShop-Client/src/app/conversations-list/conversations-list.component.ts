import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ConversationDialogComponent } from '../Dialogs/conversation-dialog/conversation-dialog.component';
import { ConversationListItemModel } from '../shared/models/messages/conversationListItem.model';
import { ConversationsListModel } from '../shared/models/messages/conversationsList.model';
import { ConversationService } from '../shared/services/conversation.service';
import { IdentityService } from '../shared/services/identity.service';

@Component({
  selector: 'app-conversations-list',
  templateUrl: './conversations-list.component.html',
  styleUrls: ['./conversations-list.component.sass']
})
export class ConversationsListComponent implements OnInit {

  constructor(private service: ConversationService,
    private identityService: IdentityService,
    private dialogRef: MatDialog) { }

  public model: ConversationsListModel;
  public userNameToDisplay;

  ngOnInit(): void {
    this.service.getConversationsList().subscribe((res: any) => {
      this.model = res.ConversationsList;
      for (let conv of this.model.Conversations) {
        var date = new Date(conv.LastMsgSentTime);
        conv.FormatedSentTime = `${date.getHours()}:${date.getMinutes()}`

        conv.UserNameToDisplay = (conv.SenderID == this.identityService.getUserID) ? conv.ReceiverName : conv.SenderName;
      }
    },
      (error) => console.log(error));
  }

  getConversationDetails(data: ConversationListItemModel) {
    this.dialogRef.open(ConversationDialogComponent, {
      panelClass: 'custom-dialog-container', data:
      {
        SenderID: data.SenderID,
        ReceiverID: data.ReceiverID,
        Topic: data.Topic
      }
    });
  }

}
