import { trigger, transition, style, animate } from '@angular/animations';

export const fader = trigger('routeFadeAnimation', [
  transition('* <=> *', [
    style({ opacity: 0 }), // Start with opacity 0
    animate('0.5s', style({ opacity: 1 })), // Animate to opacity 1
  ]),
]);
 