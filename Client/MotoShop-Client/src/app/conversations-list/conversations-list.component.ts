import { Component, OnInit } from '@angular/core';
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
    private identityService: IdentityService) { }

  public model: ConversationsListModel;
  public userNameToDisplay;

  ngOnInit(): void {
    this.service.getConversationsList().subscribe((res: any) => {
      this.model = res.ConversationsList;
      console.log(this.model);

      for (let conv of this.model.Conversations) {
        var date = new Date(conv.LastMsgSentTime);
        conv.FormatedSentTime = `${date.getHours()}:${date.getMinutes()}`

        conv.UserNameToDisplay = (conv.SenderID == this.identityService.getUserID) ? conv.ReceiverName : conv.SenderName;
      }
    },
      (error) => console.log(error));
  }

}
