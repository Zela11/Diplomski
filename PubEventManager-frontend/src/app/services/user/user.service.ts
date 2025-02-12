import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/env/enviroment';
import { TokenStorageService } from './token.service';
import { User } from 'src/app/shared/model/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private currentUserSubject: BehaviorSubject<User | null>;
  public currentUser: Observable<User | null>;

  constructor(private http: HttpClient, private tokenStorage: TokenStorageService) {
    const userId = this.tokenStorage.getUserId();
    this.currentUserSubject = new BehaviorSubject<any>(userId);
    this.currentUser = this.currentUserSubject.asObservable();
    if (userId) {
      this.getById(userId).subscribe(
        (user: User) => {
          this.currentUserSubject.next(user);
        },
        (error) => {
          console.error('Error fetching user', error);
        }
      );
    }
  }
  private apiUrl = `${environment.apiUrl}/users`; // API URL


  public get currentUserValue(): any {
    return this.currentUserSubject.value;
  }

  login(email: string, password: string) : Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, { email, password });
 
  }
  handleLoginResponse(response: any): void {
    console.log(response);
    this.tokenStorage.saveToken(response.token, response.userId);
    this.getById(response.userId).subscribe(
      (user: User) => {
        this.currentUserSubject.next(user);
      },
      (error) => {
        console.error('Error fetching user after login', error);
      }
    );
  }
  logout() {
    this.tokenStorage.clear();
    this.currentUserSubject.next(null);
  }

  register(user: User): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }
  getById(id: number) : Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }
  update(id: number, user: User): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, user);
  }
}
