import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private tokenService: TokenService, private router: Router) {}

  logout() {
    this.tokenService.removeToken();
    this.router.navigate(['/login']);
  }
}


