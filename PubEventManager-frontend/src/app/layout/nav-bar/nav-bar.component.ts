import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenStorageService } from 'src/app/services/user/token.service';
import { UserService } from 'src/app/services/user/user.service';
import { User } from 'src/app/shared/model/user';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  isLoggedIn: boolean = false;
  currentUser: User | null = null;
  userType: number | null = null;

  constructor(private userService: UserService, private tokenStorage: TokenStorageService, private router: Router) {}
  ngOnInit(): void {
    this.checkLoginStatus();
    }


    checkLoginStatus(): void {
      this.userService.currentUser.subscribe(user => {
        this.isLoggedIn = !!user;
        this.currentUser = user;
        this.userType = user ? user.userType : null;
        console.log(user);
      });
      
    }

  logout(): void {
    this.userService.logout();
    this.isLoggedIn = false;
    this.router.navigate(['home']);
  }
}
