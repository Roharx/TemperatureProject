import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WebSocketService {
  private socket?: WebSocket;
  private messageSubject: Subject<any> = new Subject<any>();

  connect(url: string): Observable<any> {
    if (!this.socket || this.socket.readyState !== WebSocket.OPEN) {
      this.socket = new WebSocket(url);
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
