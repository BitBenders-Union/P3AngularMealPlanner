import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-bookmark',
  templateUrl: './bookmark.component.html',
  styleUrls: ['./bookmark.component.css'],
})
export class BookmarkComponent {
  @Output() isExpandedChange = new EventEmitter<boolean>();

  isHidden = false; // Add this property
  isExpanded = false;
  toggleExpansion() {
    this.isExpanded = !this.isExpanded;
    this.isExpandedChange.emit(this.isExpanded);
  }
}
