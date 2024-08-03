import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TokenStorageService } from '../services/user/token.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  forecasts: any[] = [];

  private apiUrl = 'https://localhost:7035/WeatherForecast'; // URL API-a

  constructor(private http: HttpClient, private tokenStorage: TokenStorageService) { }

  ngOnInit(): void {
    this.http.get<any[]>(this.apiUrl).subscribe(data => {
      this.forecasts = data;
    });
    const userId = this.tokenStorage.getUserId();
    console.log(userId);
  }
}
