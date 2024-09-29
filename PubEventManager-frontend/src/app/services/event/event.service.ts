import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { EventModel } from 'src/app/shared/model/event';
import { environment } from 'src/env/enviroment';

@Injectable({
  providedIn: 'root'
})
export class EventService {

  private selectedEvent: EventModel | null = null;

  private apiUrl = `${environment.apiUrl}/events`; // API URL

  constructor(private http: HttpClient) { }

  getEvents(): Observable<EventModel[]> {
    return this.http.get<EventModel[]>(this.apiUrl);
  }
  getEventById(id: number): Observable<EventModel> {
    return this.http.get<EventModel>(`${this.apiUrl}/${id}`);
  }
  createEvent(formData: FormData): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}`, formData);
  }
  generateEventReport(eventId: number) {
    const url = `${this.apiUrl}/${eventId}/report`;
    return this.http.get(url, { responseType: 'blob' });  // Vrati PDF kao blob
  }
  setEvent(event: EventModel): void {
    this.selectedEvent = event;
  }

  getEvent(): EventModel | null {
    return this.selectedEvent;
  }
}
