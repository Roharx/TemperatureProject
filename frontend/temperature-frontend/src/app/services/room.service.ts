import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RoomService {
  private baseUrl: string = environment.baseURL;

  constructor(private http: HttpClient) { }

  createRoom(roomData: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/api/Room/create`, roomData);
  }

  updateRoom(roomData: any): Observable<any> {
    const headers = new HttpHeaders({
      'id': roomData.id.toString()
    });
    const { id, ...payload } = roomData;
    return this.http.put<any>(`${this.baseUrl}/api/Room/update`, payload, { headers });
  }

  deleteRoom(roomId: number): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/api/Room/delete/${roomId}`);
  }

  getRoomById(roomId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/api/Room/getById/${roomId}`);
  }
}
