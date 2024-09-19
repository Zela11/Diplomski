import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';
import { TokenStorageService } from 'src/app/services/user/token.service';
import { User } from 'src/app/shared/model/user';
import { NgForm } from '@angular/forms';
import { EventModel } from '../shared/model/event';
import { EventService } from '../services/event/event.service';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http'; // Import za rad sa greškama HTTP zahteva


@Component({
  selector: 'app-manager-profile',
  templateUrl: './manager-profile.component.html',
  styleUrls: ['./manager-profile.component.css']
})
export class ManagerProfileComponent implements OnInit {
  isEditing: boolean = false; 
  currentUser: User | null = null;
  newEmployee: User = {
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    userType: 2,  
  };
  password: string = '';
  confirmPassword: string = '';
  eventTime: string = '';  // Dodajemo varijablu za čuvanje izabranog vremena
  newEvent: EventModel = {
    id: 0,
    name: '',
    description: '',
    date: new Date(),
    managerId: 0,
  }
  showPopup: boolean = false;
  showSecondPopup: boolean = false;

  constructor(
    private userService: UserService, 
    private tokenStorage: TokenStorageService,
    private eventService: EventService
  ) {}

  ngOnInit(): void {
    const userId = this.tokenStorage.getUserId();
    this.userService.getById(userId).subscribe(user => {
      this.currentUser = user;
    });
  }
  openPopup(): void {
    this.showPopup = true;
  }

  closePopup(): void {
    this.showPopup = false;
  }
  openSecondPopup(): void {
    this.showSecondPopup = true;
  }

  closeSecondPopup(): void {
    this.showSecondPopup = false;
  }
  onRegisterEmployee(form: NgForm): void {
    if (form.valid && this.password === this.confirmPassword) {
      this.newEmployee.password = this.password;
      this.userService.register(this.newEmployee).subscribe(
        response => {
          console.log('Employee registration successful', response.message);
          form.reset();
        },
        error => {
          console.error('Employee registration error', error);
        }
      );
    } else {
      console.log('Form is invalid or passwords do not match');
    }
  }

  onCreateEvent(form: NgForm): void {
    if (form.valid) {
      this.newEvent.managerId = this.tokenStorage.getUserId();
  
      if (!(this.newEvent.date instanceof Date)) {
        this.newEvent.date = new Date(this.newEvent.date);
      }
  
      if (this.newEvent.date && this.eventTime) {
        this.newEvent.date = this.combineDateAndTime(this.newEvent.date, this.eventTime);
      }
  
      const formattedDate = this.getFormattedDateForBackend(this.newEvent.date);
      this.newEvent.date = new Date(formattedDate);
  
      this.eventService.createEvent(this.newEvent).subscribe(
        (response) => {
          console.log("Successfully created new event", response);
          form.reset();
        },
        (error: HttpErrorResponse) => {
          if (error.status === 400) {
            alert('An event already exists on the selected day. Please choose different date.');
          } else {
            console.error('An error occurred:', error);
          }
        }
      );
    } else {
      console.log('Form is invalid');
    }
  }
  toggleEdit(): void {
    if (this.isEditing && this.currentUser) {
      this.userService.update( this.tokenStorage.getUserId(), this.currentUser).subscribe(
        (response) => {
          console.log('User updated successfully', response);
          this.isEditing = false; // Zaključavamo polja ponovo
        },
        (error) => {
          console.error('Error updating user', error);
        }
      );
    } else {
      // Ako nije u modu uređivanja, omogućavamo uređivanje
      this.isEditing = true;
    }
  }
  
  private getFormattedDateForBackend(date: Date): string {
    const padZero = (num: number): string => (num < 10 ? '0' : '') + num;
  
    const year = date.getFullYear();
    const month = padZero(date.getMonth() + 1);
    const day = padZero(date.getDate());
    const hours = padZero(date.getHours());
    const minutes = padZero(date.getMinutes());
    const seconds = padZero(date.getSeconds());
  
    const timezoneOffset = -date.getTimezoneOffset();
    const sign = timezoneOffset >= 0 ? '+' : '-';
    const absOffset = Math.abs(timezoneOffset);
    const hoursOffset = padZero(Math.floor(absOffset / 60));
    const minutesOffset = padZero(absOffset % 60);
  
    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}${sign}${hoursOffset}:${minutesOffset}`;
  }
   private combineDateAndTime(date: Date, time: string): Date {
    const [hours, minutes] = time.split(':').map(Number); // Razdvojimo sate i minute
    const newDate = new Date(date);

    newDate.setHours(hours, minutes, 0, 0); // Dodamo vreme na datum
    return newDate;
  }
  
}
