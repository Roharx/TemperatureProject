import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin } from 'rxjs';
import { environment } from '../../environments/environment';
import { map, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountOfficeService {
  private baseUrl = environment.baseURL;

  constructor(private http: HttpClient) {}

  getOfficesForAccount(accountId: number): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/api/AccountOffice/GetOfficesForAccount/${accountId}`, {});
  }

  linkAccountToOffice(data: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/api/AccountOffice/LinkAccountToOffice`, data);
  }

  getOfficeById(officeId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/api/Office/getById/${officeId}`);
  }

  getRoomsForOffice(officeId: number): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/api/Room/getRoomsForOffice/${officeId}`,{});
  }

  getOfficesWithDetails(accountId: number): Observable<any[]> {
    return this.getOfficesForAccount(accountId).pipe(
      map(response => response.responseData),
      switchMap((officeIds: any[]) => {
        const officeDetails$ = officeIds.map((office: any) =>
          forkJoin({
            office: this.getOfficeById(office.office_id),
            rooms: this.getRoomsForOffice(office.office_id)
          }).pipe(
            map(details => ({
              office: details.office.responseData,
              rooms: details.rooms.responseData
            }))
          )
        );
        return forkJoin(officeDetails$);
      })
    );
  }
}
