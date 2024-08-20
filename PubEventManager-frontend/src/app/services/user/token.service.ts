import { Injectable } from '@angular/core';
import { ACCESS_TOKEN, USER } from 'src/app/shared/constants';
@Injectable({
  providedIn: 'root',
})
export class TokenStorageService {
  constructor() {}

  saveToken(token: string, userId: number): void {
    console.log("u token storageu");
    console.log(token);
    console.log(userId);
    sessionStorage.removeItem(ACCESS_TOKEN);
    sessionStorage.removeItem(USER);
    sessionStorage.setItem(ACCESS_TOKEN, token);
    sessionStorage.setItem(USER, userId.toString());
  }

  getToken(): string | null {
    return sessionStorage.getItem(ACCESS_TOKEN);
  }

  getUserId(): number {
    const userIdString = sessionStorage.getItem(USER);
    return userIdString ? parseInt(userIdString, 10) : 0;
  }

  clear(): void {
    sessionStorage.removeItem(ACCESS_TOKEN);
    sessionStorage.removeItem(USER);
  }
}
