import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WebSocketService {
  private socket?: WebSocket;
  private messageSubject: Subject<any> = new Subject<any>();
  private wsUrl: string = environment.wsUrl;
  private reconnectAttempts: number = 0;
  private maxReconnectAttempts: number = 5;
  private reconnectInterval: number = 5000; // 5 seconds

  connect(): Observable<any> {
    if (!this.socket || this.socket.readyState !== WebSocket.OPEN) {
      this.socket = new WebSocket(this.wsUrl);

      this.socket.onmessage = (event) => {
        this.messageSubject.next(event.data);
      };

      this.socket.onerror = (event) => {
        this.messageSubject.error(event);
        this.reconnect();
      };

      this.socket.onclose = () => {
        this.messageSubject.complete();
        this.reconnect();
      };
    }

    return this.messageSubject.asObservable();
  }

  sendMessage(message: string): void {
    if (this.socket && this.socket.readyState === WebSocket.OPEN) {
      this.socket.send(message);
    } else {
      this.reconnect();
    }
  }

  private reconnect(): void {
    if (this.reconnectAttempts < this.maxReconnectAttempts) {
      setTimeout(() => {
        console.log(`WebSocket reconnect attempt #${this.reconnectAttempts + 1}`);
        this.reconnectAttempts++;
        this.connect();
      }, this.reconnectInterval);
    } else {
      console.error('Max reconnect attempts reached');
    }
  }
}
