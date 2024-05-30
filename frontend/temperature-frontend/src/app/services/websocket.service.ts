import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WebSocketService {
  private socket?: WebSocket;
  private messageSubject: Subject<any> = new Subject<any>();

  connect(): Observable<any> {
    const wsUrl = environment.wsUrl;
    if (!this.socket || this.socket.readyState !== WebSocket.OPEN) {
      this.socket = new WebSocket(wsUrl);
      this.socket.onmessage = (event) => this.messageSubject.next(event.data);
      this.socket.onerror = (event) => this.messageSubject.error(event);
      this.socket.onclose = () => this.messageSubject.complete();
    }
    return this.messageSubject.asObservable();
  }

  sendMessage(message: string): void {
    if (this.socket && this.socket.readyState === WebSocket.OPEN) {
      this.socket.send(message);
    }
  }
}
