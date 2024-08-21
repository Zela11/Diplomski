import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Reservation } from 'src/app/shared/model/reservation';
import { environment } from 'src/env/enviroment';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private apiUrl = `${environment.apiUrl}/reservations`; // API URL

  constructor(private http: HttpClient) { }

  createReservation(reservation: Reservation) {
    console.log("u res servicu");
    return this.http.post(this.apiUrl, reservation);
  }
  getReservationsByEventId(eventId: number): Observable<Reservation[]> {
    return this.http.get<Reservation[]>(`${this.apiUrl}/event/${eventId}`);
  }
}
