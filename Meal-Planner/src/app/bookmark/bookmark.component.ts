import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-bookmark',
  templateUrl: './bookmark.component.html',
  styleUrls: ['./bookmark.component.css'],
})
export class BookmarkComponent {

  // emits the isExpanded property to the parent component (dashboard)
  @Output() isExpandedChange = new EventEmitter<boolean>();

  // properties to control the expansion of the bookmark component
  // by expansion, we mean the bookmark component is expanded to show the recipe-card components
  isHidden = false; 
  isExpanded = false;


  // triggers on click of the arrow icon
  // then emits the isExpanded property to the parent component (dashboard)
  toggleExpansion() {
    this.isExpanded = !this.isExpanded;
    this.isExpandedChange.emit(this.isExpanded); // I don't think this actually emits anything yet, it just tells the output to emit
  }
}
