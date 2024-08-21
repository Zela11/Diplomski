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
  public selectedWeek: string = this.getCurrentWeek();

  constructor(private eventService: EventService, private router: Router) {}

  ngOnInit(): void {
    this.loadUpcomingEvents();
  }

  loadUpcomingEvents(): void {
    const [startOfWeek, endOfWeek] = this.getWeekRange(this.selectedWeek);

    this.eventService.getEvents().subscribe(events => {
      this.upcomingEvents = events.filter(event => {
        const eventDate = new Date(event.date);
        return eventDate >= startOfWeek && eventDate <= endOfWeek;
      })
      .sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime()); // Sortiranje po datumu
    });
  }

  onWeekChange(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement) {
      this.selectedWeek = inputElement.value;
      this.loadUpcomingEvents();
    }
  }

  private getCurrentWeek(): string {
    const today = new Date();
    return `${today.getFullYear()}-W${this.getWeekNumber(today)}`;
  }

  private getWeekRange(week: string): [Date, Date] {
    const [year, weekNumber] = week.split('-W').map(Number);
    const startOfWeek = this.getDateFromISOWeek(year, weekNumber, 1);
    const endOfWeek = new Date(startOfWeek);
    endOfWeek.setDate(startOfWeek.getDate() + 6);
    return [startOfWeek, endOfWeek];
  }

  private getDateFromISOWeek(year: number, week: number, day: number): Date {
    const simpleDate = new Date(year, 0, 1 + (week - 1) * 7 + (day - 1));
    return new Date(simpleDate.setDate(simpleDate.getDate() - simpleDate.getDay() + 1));
  }

  private getWeekNumber(date: Date): number {
    const jan1 = new Date(date.getFullYear(), 0, 1);
    const days = Math.floor((date.getTime() - jan1.getTime()) / (24 * 60 * 60 * 1000));
    return Math.ceil((days + 1) / 7);
  }
  navigateToEvent(event: EventModel): void {
    this.router.navigate(['/event-details', event.id]);
  }
}
