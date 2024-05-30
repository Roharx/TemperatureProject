import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { JwtService } from './jwt.service';
import { environment } from '../../environments/environment';

const TOKEN_KEY = 'auth-token';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  constructor(private cookieService: CookieService, private jwtService: JwtService) {}

  getToken(): string | null {
    const token = this.cookieService.get(TOKEN_KEY);
    return token;
  }

  saveToken(token: string): void {
    const tokenData = this.jwtService.decodeToken(token);
    const expirationTime = tokenData ? tokenData['exp'] : 0;
    const expirationDate = new Date(expirationTime * 1000);

    this.cookieService.set(
      TOKEN_KEY,
      token,
      expirationDate,
      '/', // Path
      environment.domain, // Domain from environment
      window.location.protocol === 'https:', // Secure flag based on protocol
      'Strict' // SameSite flag
    );
  }

  removeToken(): void {
    console.log('Removing token');
    this.cookieService.delete(TOKEN_KEY, '/', environment.domain);
  }
}
