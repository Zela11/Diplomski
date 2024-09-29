import { Component, OnInit } from '@angular/core';
import { EventService } from '../services/event/event.service';
import { EventModel } from '../shared/model/event';
import { Router } from '@angular/router';

@Component({
  selector: 'app-events-page',
  templateUrl: './events-page.component.html',
  styleUrls: ['./events-page.component.css']
})
export class EventsPageComponent implements OnInit {
  public upcomingEvents: EventModel[] = [];
  currentPage = 0;
  itemsPerPage = 3;
  totalPages: number = 0; // Initialize with a default value
  constructor(private eventService: EventService, private router: Router) {}
  get currentEvents() {
    const start = this.currentPage * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    return this.upcomingEvents.slice(start, end);
  }
  ngOnInit(): void {
    this.loadUpcomingEvents();
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
  loadUpcomingEvents(): void {

    this.eventService.getEvents().subscribe(events => {
      this.upcomingEvents = events;
      console.log(this.upcomingEvents);
      console.log("duzina", this.upcomingEvents.length);
      this.totalPages = Math.ceil(this.upcomingEvents.length / this.itemsPerPage);
    });
  
  }
 

  navigateToEvent(event: EventModel): void {
    this.eventService.setEvent(event);  // Сачувај објекат у сервис
    this.router.navigate(['/event-details']);
  }
}
