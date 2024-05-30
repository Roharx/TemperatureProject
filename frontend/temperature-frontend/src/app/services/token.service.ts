import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { JwtService } from './jwt.service';

const TOKEN_KEY = 'auth-token';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  constructor(private cookieService: CookieService, private jwtService: JwtService) {}

  getToken(): string | null {
    const token = this.cookieService.get(TOKEN_KEY);
    console.log('Retrieved token:', token);
    return token;
  }

  saveToken(token: string): void {
    const tokenData = this.jwtService.decodeToken(token);
    const expirationTime = tokenData ? tokenData['exp'] : 0;
    const expirationDate = new Date(expirationTime * 1000);

    console.log('Saving token with expiration:', expirationDate);

    this.cookieService.set(
      TOKEN_KEY,
      token,
      expirationDate,
      '/', // Path
      '161.97.92.174', // Domain
      window.location.protocol === 'https:', // Secure flag based on protocol
      'Strict' // SameSite flag
    );

    // Log the current cookies for debugging
    console.log('Current cookies:', document.cookie);
  }

  removeToken(): void {
    console.log('Removing token');
    this.cookieService.delete(TOKEN_KEY, '/', '161.97.92.174');
  }
}
