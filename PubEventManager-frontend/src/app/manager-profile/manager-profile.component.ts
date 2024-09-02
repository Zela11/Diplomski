import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';
import { TokenStorageService } from 'src/app/services/user/token.service';
import { User } from 'src/app/shared/model/user';
import { NgForm } from '@angular/forms';

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
    userType: 2,  // Default to Server
  };
  password: string = '';
  confirmPassword: string = '';
  constructor(private userService: UserService, private tokenStorage: TokenStorageService) {}

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
}
