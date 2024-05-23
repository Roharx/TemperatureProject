import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ModifyAccountDTO } from '../../models';
import { TokenService } from './token.service';
import { JwtService } from './jwt.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(private http: HttpClient, private tokenService: TokenService, private jwtService: JwtService) {}

  modifyAccount(modifyAccountData: ModifyAccountDTO): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.put(environment.baseURL + '/api/Account/update', modifyAccountData, { headers });
  }
  deleteAccount(): Observable<any> {
    const token = this.tokenService.getToken();
    const decodedToken = this.jwtService.decodeToken(token!);
    const userId = decodedToken?.['id'];
    return this.http.delete(environment.baseURL + `/api/Account/delete/${userId}`);
  }
  getAccountName(): string | null {
    const token = this.tokenService.getToken();
    const decodedToken = this.jwtService.decodeToken(token!);
    return decodedToken?.['name'] || null;
  }
  getAccountId(){
    const token = this.tokenService.getToken();
    const decodedToken = this.jwtService.decodeToken(token!);
    return decodedToken?.['id'];
  }

}
