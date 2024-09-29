import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { EventService } from '../services/event/event.service';
import { EventModel } from '../shared/model/event';
import { Router } from '@angular/router';

@Component({
  selector: 'app-events-page',
  templateUrl: './events-page.component.html',
  styleUrls: ['./events-page.component.css']
})
export class EventsPageComponent implements OnInit {
  public allEvents: EventModel[] = [];  
  public upcomingEvents: EventModel[] = [];
  currentPage = 0;
  itemsPerPage = 3;
  totalPages: number = 0;
  selectedDate: string = this.formatDate(new Date());
  weekRange: string = '';  
  isPopupVisible: boolean = false; // Control the popup visibility


  constructor(private eventService: EventService, private router: Router) {}

  ngOnInit(): void {
    const today = new Date();
  
    // If today is Sunday (getDay() returns 0 for Sunday), set the selectedDate to the next Monday
    if (today.getDay() === 0) {
      // Set the date to the next Monday
      today.setDate(today.getDate() + 1); // Moves to next Monday
    }
  
    this.selectedDate = this.formatDate(today); // Set the selected date to today (or next Monday if it's Sunday)
    
    this.loadAllEvents();
    this.updateWeekRange(today); // Initialize week range
  }
  
  openPopup(): void {
    this.isPopupVisible = true; // Show the popup
  }
  confirmWeekSelection(): void {
    // Close the popup after confirming
    this.closePopup();
  }
  closePopup(): void {
    this.isPopupVisible = false; // Hide the popup
  }
  get currentEvents() {
    const start = this.currentPage * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    return this.upcomingEvents.slice(start, end);
  }

  nextPage() {
    if (this.currentPage < this.totalPages - 1) {
      this.currentPage++;
    }
  }

  prevPage() {
    if (this.currentPage > 0) {
      this.currentPage--;
    }
  }

  loadAllEvents(): void {
    this.eventService.getEvents().subscribe(events => {
      this.allEvents = events;
      this.filterEventsByWeek(this.selectedDate);
    });
  }

  setWeekRange(date: Date): void {
    const startOfWeek = this.getStartOfWeek(date);
    const endOfWeek = new Date(startOfWeek);
    endOfWeek.setDate(endOfWeek.getDate() + 6);  

    this.weekRange = `Mon ${this.formatDate(startOfWeek)} - Sun ${this.formatDate(endOfWeek)}`;
    this.filterEventsByWeek(this.selectedDate);
  }

  filterEventsByWeek(selectedDate: string): void {
    const date = new Date(selectedDate);
    const startOfWeek = this.getStartOfWeek(date);
    const endOfWeek = new Date(startOfWeek);
    endOfWeek.setDate(endOfWeek.getDate() + 6);  

    this.upcomingEvents = this.allEvents.filter(event => {
      const eventDate = new Date(event.date);
      return eventDate >= startOfWeek && eventDate <= endOfWeek;
    });

    this.totalPages = Math.ceil(this.upcomingEvents.length / this.itemsPerPage);
    this.currentPage = 0;  
  }

  getStartOfWeek(date: Date): Date {
    const day = date.getDay();
    const diff = date.getDate() - day + (day === 0 ? -6 : 1);  
    return new Date(date.setDate(diff));
  }

  onDateChange(event: any): void {
    const newDate = event.target.value;
    this.selectedDate = newDate;
    this.updateWeekRange(new Date(newDate)); // Update the week range when the date changes
    this.closePopup();
  }

  updateWeekRange(date: Date): void {
    const startOfWeek = this.getStartOfWeek(date);
    const endOfWeek = new Date(startOfWeek);
    endOfWeek.setDate(endOfWeek.getDate() + 6);  // End of the selected week

    this.weekRange = `Mon ${this.formatDate(startOfWeek)} - Sun ${this.formatDate(endOfWeek)}`;
    this.filterEventsByWeek(this.selectedDate); // Filter events based on new week
  }

  

  formatDate(date: Date): string {
    return date.toISOString().split('T')[0];
  }

  navigateToEvent(event: EventModel): void {
    this.eventService.setEvent(event);
    this.router.navigate(['/event-details']);
  }
}
