import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OfficeService {
  constructor(private http: HttpClient) {}

  createOffice(officeData: { name: string; location: string }): Observable<any> {
    return this.http.post<any>(`${environment.baseURL}/api/Office/create`, officeData);
  }

  updateOffice(officeData: { id: number; name: string; location: string }): Observable<any> {
    return this.http.put<any>(`${environment.baseURL}/api/Office/update`, officeData);
  }
}
