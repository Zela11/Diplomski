import { Component, OnInit } from '@angular/core';
import { EventModel } from '../shared/model/event';
import { ActivatedRoute } from '@angular/router';
import { EventService } from '../services/event/event.service';
import { Reservation } from '../shared/model/reservation';
import { TokenStorageService } from '../services/user/token.service';
import { ReservationService } from '../services/reservation/reservation.service';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.css']
})
export class EventDetailsComponent implements OnInit {
  public event: EventModel | undefined;
  isReservationOpen: boolean = false;
  selectedTableId: number | null = null;
  highlightedArea: number | null = null;
  eventId: number | undefined;
  reservedTableIds: number[] = [];  // List of reserved table IDs

  tableAreas = [
    { id: 1, shape: 'circle', coords: '90,88,25' },
    { id: 2, shape: 'circle', coords: '161,157,17' },
    { id: 3, shape: 'circle', coords: '271,155,17' },
    { id: 4, shape: 'circle', coords: '271,237.5,17' },
    { id: 5, shape: 'circle', coords: '56,391,17' },
    { id: 6, shape: 'circle', coords: '399,343,17' },
    { id: 7, shape: 'circle', coords: '478,391,17' },
    { id: 8, shape: 'rect', coords: '59,209,63,32' },
    /*
    { id: 9, shape: 'rect', coords: '161,344,32,63' },
    { id: 10, shape: 'rect', coords: '265,344,32,63' },
    { id: 11, shape: 'rect', coords: '201,26,32,63' },*/
  ];
  toggleReservation() {
    this.isReservationOpen = !this.isReservationOpen;
    if (this.isReservationOpen) {
      this.selectedTableId = null;  // Resetuje selektovani sto prilikom otvaranja
      this.highlightedArea = null;  // Resetuje obeleženi sto prilikom otvaranja
    }
  }
  constructor(
    private route: ActivatedRoute,
    private eventService: EventService,
    private tokenStorage: TokenStorageService,
    private reservationService: ReservationService
  ) {}

  ngOnInit(): void {
    this.loadEventDetails();
  }
  private loadEventDetails(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      const eventId = Number(id); 
      this.eventId = eventId;
      this.eventService.getEventById(eventId).subscribe(event => {
        if (event.date && typeof event.date === 'string') {
          event.date = new Date(event.date);
        }
        this.event = event;
        console.log(this.event);
        console.log(this.formatDateTime(event.date));
        this.loadReservations(eventId);
      });
    }
  }
  private loadReservations(eventId: number): void {
    this.reservationService.getReservationsByEventId(eventId).subscribe(reservations => {
      this.reservedTableIds = reservations.map(reservation => reservation.tableId);
    });
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

  public reserveTable(): void {
    console.log(this.selectedTableId);
    console.log(this.event);

    if (this.selectedTableId && this.eventId) {
      const reservation: Reservation = {
        guestId: this.tokenStorage.getUserId(),
        eventId: this.eventId,
        tableId: this.selectedTableId
      };
      this.reservationService.createReservation(reservation).subscribe({
        next: (response) => {
          alert('Reservation confirmed!');
          this.loadEventDetails();
          this.closeModal();
        },
        error: (error) => {
          alert('Reservation failed: ' + error.message);
        }
      });
    }
  }
  closeModal(): void {
    this.isReservationOpen = false;
    this.selectedTableId = null; // Resetuje selektovani sto
    this.highlightedArea = null; // Resetuje obeleženi sto
  }


  public selectArea(areaId: number): void {
    if (this.reservedTableIds.includes(areaId)) {
      alert('This table is already reserved. Please choose another one.');
      return;
    }
    
    if (this.selectedTableId === areaId) {
      // Ako je isti sto već selektovan, poništi selekciju
      this.selectedTableId = null;
      this.highlightedArea = null;
    } else {
      // Inače, postavi kao selektovan
      this.selectedTableId = areaId;
      this.highlightedArea = areaId;
    }
    
    console.log('Selected table: ', this.selectedTableId);
  }
  
  public getOverlayStyle(area: any): any {
    const isReserved = this.reservedTableIds.includes(area.id);
    const isSelected = this.selectedTableId === area.id;
  
    let backgroundColor = '';
  
    if (isReserved) {
      backgroundColor = 'rgba(64, 60, 60, 0.8)'; // Tamno siva boja za rezervisane stolove
    } else if (isSelected) {
      backgroundColor = 'rgba(0, 255, 0, 0.5)'; // Zelena boja za selektovani sto
    } else {
      return {}; // Vraća prazan stil za nerezervisane i neselektovane stolove
    }
  
    const coords = area.coords.split(',').map(Number);
  
    if (area.shape === 'circle') {
      const [x, y, radius] = coords;
      return {
        position: 'absolute',
        borderRadius: '50%',
        backgroundColor: backgroundColor,
        width: `${radius * 2}px`,
        height: `${radius * 2}px`,
        left: `${x - radius}px`,
        top: `${y - radius}px`,
        pointerEvents: 'none',
      };
    }
    else if (area.shape === 'rect') {
      const [centerX, centerY, width, height] = coords;
      return {
        position: 'absolute',
        backgroundColor: backgroundColor,
        width: `${width}px`,
        height: `${height}px`,
        left: `${centerX - (width / 2)}px`,
        top: `${centerY - (height / 2)}px`,
        pointerEvents: 'none',
      };
    }
  
    // Dodajte podršku za druge oblike ako je potrebno
    return {};
  }
  
}