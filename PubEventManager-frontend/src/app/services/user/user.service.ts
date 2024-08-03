import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/env/enviroment';
import { TokenStorageService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  

  constructor(private http: HttpClient, private tokenStorage: TokenStorageService) { }
  private apiUrl = `${environment.apiUrl}/users`; // API URL

  login(email: string, password: string) : Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { email, password });
 
  }
  handleLoginResponse(response: any): void {
    this.tokenStorage.saveToken(response.token, response.userId);
  }
  logout() {
    this.tokenStorage.clear();
  }
}
