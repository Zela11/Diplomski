import { Component, OnInit } from '@angular/core';
import { EventModel } from '../shared/model/event';
import { ActivatedRoute } from '@angular/router';
import { EventService } from '../services/event/event.service';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.css']
})
export class EventDetailsComponent implements OnInit {
  public event: EventModel | undefined;
  public showReservation: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private eventService: EventService
  ) {}

  ngOnInit(): void {
    this.loadEventDetails();
  }
  private loadEventDetails(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      const eventId = Number(id); 
      this.eventService.getEventById(eventId).subscribe(event => {
        if (event.date && typeof event.date === 'string') {
          event.date = new Date(event.date);
        }
        this.event = event;
        console.log(this.formatDateTime(event.date));
      });
    }
  }
  public formatDateTime(date?: Date): string {
    if (!date) {
      return '';
    }
    const optionsDate: Intl.DateTimeFormatOptions = {
      weekday: 'long', year: 'numeric', month: 'long', day: 'numeric'
    };
    const optionsTime: Intl.DateTimeFormatOptions = {
      hour: '2-digit', minute: '2-digit', hour12: false
    };

    const formattedDate = date.toLocaleDateString('en-US', optionsDate);
    const formattedTime = date.toLocaleTimeString('en-US', optionsTime);

    return `${formattedDate} - ${formattedTime}h`;
  }
  public toggleReservation(): void {
    console.log("trebalo bi da otvori");
    this.showReservation = !this.showReservation;
  }

  public confirmReservation(): void {
    alert('Reservation confirmed!');
  }
  closeModal(): void {
    this.showReservation = false;
  }
}
