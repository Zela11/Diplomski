import { Component } from '@angular/core';
import { UserService } from '../services/user/user.service';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../shared/model/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  user: User = {
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    userType: 0,
  }
  password: string = '';
  confirmPassword: string = '';

  constructor(private userService: UserService, private router: Router) {}


  onRegister(form: NgForm): void {
    if (form.valid && this.password === this.confirmPassword) {
      this.user.password = this.password;
      this.userService.register(this.user).subscribe(
        response => {
          console.log('Registration successful', response.message);
          this.router.navigate(['login']);
        },
        error => {
          console.error('Registration error', error);
        }
      );
    } else {
      console.log('Form is invalid or passwords do not match');
    }
  }
}