import { Component, OnInit } from '@angular/core';
import { EventService } from '../services/event/event.service';
import { Event } from '../shared/model/event';

@Component({
  selector: 'app-events-page',
  templateUrl: './events-page.component.html',
  styleUrls: ['./events-page.component.css']
})
export class EventsPageComponent implements OnInit {
  public upcomingEvents: Event[] = [];
  constructor(private eventService: EventService) {}
  ngOnInit(): void {
    this.loadUpcomingEvents();
  }
  loadUpcomingEvents(): void {
    const today = new Date();
    const startOfWeek = this.getStartOfWeek(today);
    const endOfWeek = this.getEndOfWeek(today);

    this.eventService.getEvents().subscribe(events => {
      this.upcomingEvents = events.filter(event => {
        const eventDate = new Date(event.date);
        return eventDate >= startOfWeek && eventDate <= endOfWeek;
      });
    });
  }

  private getStartOfWeek(date: Date): Date {
    const day = date.getDay();
    const diff = date.getDate() - day + (day === 0 ? -6 : 1); 
    return new Date(date.setDate(diff));
  }

  private getEndOfWeek(date: Date): Date {
    const startOfWeek = this.getStartOfWeek(date);
    return new Date(startOfWeek.getFullYear(), startOfWeek.getMonth(), startOfWeek.getDate() + 6);
  }
}
