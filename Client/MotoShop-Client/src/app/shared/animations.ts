import { trigger, state, style, transition, animate, keyframes } from '@angular/animations';

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

export const removeWatchlistItemAnimation = trigger("removeWatchlistItemAnimation", [
  transition("* => activate", animate("0.7s", keyframes([
    style({ transform: 'translateY(0)', opacity: 1, offset: 0 }),
    style({ transform: 'translateY(-10%)', opacity: 0.8, offset: 0.1 }),
    style({ transform: 'translateY(300%)', opacity: 0, offset: 1 }),
  ])))
]);