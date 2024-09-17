import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';
import { TokenStorageService } from 'src/app/services/user/token.service';
import { User } from 'src/app/shared/model/user';
import { NgForm } from '@angular/forms';
import { EventModel } from '../shared/model/event';
import { EventService } from '../services/event/event.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manager-profile',
  templateUrl: './manager-profile.component.html',
  styleUrls: ['./manager-profile.component.css']
})
export class ManagerProfileComponent implements OnInit {

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
  newEvent: EventModel = {
    id: 0,
    name: '',
    description: '',
    date: new Date(),
    managerId: 0,
  }
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
    if(form.valid) {
      this.newEvent.managerId = this.tokenStorage.getUserId();
      if (!(this.newEvent.date instanceof Date)) {
        this.newEvent.date = new Date(this.newEvent.date);
      }
  
      const formattedDate = this.getFormattedDateForBackend(this.newEvent.date);
      this.newEvent.date =  new Date(formattedDate);
      this.eventService.createEvent(this.newEvent).subscribe(
        (response) => {
          console.log("Successfully created new event", response);
          form.reset();
        }
      );
    } else {
      console.log('Form is invalid');

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
  
  
}
