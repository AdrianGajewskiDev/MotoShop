import { AfterViewChecked, Component, ElementRef, OnInit, DoCheck, IterableDiffers, IterableDiffer } from '@angular/core';
import { NgImageSliderComponent } from 'ng-image-slider';

@Component({
  selector: 'app-conversation-dialog',
  templateUrl: './conversation-dialog.component.html',
  styleUrls: ['./conversation-dialog.component.sass']
})
export class ConversationDialogComponent implements OnInit, AfterViewChecked {


  constructor(private elementRef: ElementRef) { }

  ngOnInit(): void {
  }

  ngAfterViewChecked() {
    const msgContainers = this.elementRef.nativeElement.querySelectorAll(".messageItem-container");
    console.log(msgContainers);

    for (let container of msgContainers) {
      let childHeight = (container as HTMLElement).children[0].clientHeight + 10;

      (container as HTMLElement).style.minHeight = `${childHeight}px`;
    }
  }

  sampleMessages: Message[] = [
    {
      content: "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Eum, ex!",
      sender: "owner"
    },
    {
      content: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque adipisci quibusdam est esse quas mollitia dolorum vitae nostrum soluta exercitationem?",
      sender: "recipient"
    },
    {
      content: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque adipisci quibusdam est esse quas mollitia dolorum v",
      sender: "owner"
    },
    {
      content: "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Eum, ex! Lorem ipsum, dolor sit amet consectetur adipisicing elit. Eum, ex! Lorem ipsum, dolor sit amet consectetur adipisicing elit. Eum, ex!",
      sender: "recipient"
    },
    {
      content: "Lorem ipsum, dolor sit amet consectetur adipisicing elit. Eum, ex!",
      sender: "owner"
    },
    {
      content: "Lorem ipsum dolor Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque adipisci quibusdam est esse quas mollitia dolorum v Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque adipisci quibusdam est esse quas mollitia dolorum v Lorem ipsum dolor sit amet consectetur  Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque adipisci quibusdam est esse quas mollitia dolorum v Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque adipisci quibusdam est esse quas mollitia dolorum v Lorem ipsum dolor sit amet consectetur  sit amet consectetur adipisicing elit. Atque adipisci quibusdam est esse quas mollitia dolorum vitae nostrum soluta exercitationem?",
      sender: "recipient"
    },
    {
      content: "Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque adipisci quibusdam est esse quas mollitia dolorum v Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque adipisci quibusdam est esse quas mollitia dolorum v Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque adipisci quibusdam est esse quas mollitia dolorum v",
      sender: "owner"
    },
    {
      content: "Lorem ipsum, Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque adipisci quibusdam est esse quas mollitia dolorum v Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque adipisci quibusdam est esse quas mollitia dolorum v Lorem ipsum dolor sit amet consectetur  dolor sit amet consectetur adipisicing elit. Eum, ex! Lorem ipsum, dolor sit amet consectetur adipisicing elit. Eum, ex! Lorem ipsum, dolor sit amet consectetur adipisicing elit. Eum, ex!",
      sender: "recipient"
    }
  ]


  sendMessage(content) {
    this.sampleMessages.push({ sender: "owner", content: content })
  }
}

class Message {
  content: string;
  sender: string
}
