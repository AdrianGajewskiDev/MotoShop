import { trigger, state, style, transition, animate } from '@angular/animations';

export const slideInOutAnimation = trigger('slideInOut', [
      state("slideIn", style({
        transform: 'translateX(0)' 
      })),
      state("slideOut", style({
        transform: 'translateX(-300%)' 
      })),
      transition("* => slideIn", animate("0.3s")),
      transition("* => slideOut", animate("0.3s")),
]);