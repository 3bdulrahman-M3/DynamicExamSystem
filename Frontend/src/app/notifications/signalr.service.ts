import { Injectable, OnDestroy } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
  HubConnectionState,
} from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

export interface Notification {
  Message: string;
  ExamId: string;
  StudentId: string;
  SubmissionTime: string; 
}

@Injectable({
  providedIn: 'root',
})
export class NotificationService implements OnDestroy {
  private hubConnection: HubConnection;
  private notificationSubject = new BehaviorSubject<Notification | null>(null);

  public notification$ = this.notificationSubject.asObservable();

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5063/notification') 
      .withAutomaticReconnect([0, 2000, 10000, 30000]) 
      .configureLogging(LogLevel.Information)
      .build();

    this.setupListeners();
    this.startConnection();
  }

  public startConnection() {
    if (this.hubConnection.state === HubConnectionState.Disconnected) {
      this.hubConnection
        .start()
        .then(() => console.log('SignalR Connected'))
        .catch((err) => console.error('SignalR Connection Error: ', err));
    } else {
      console.log('SignalR connection is already in progress or connected');
    }
  }

  public setupListeners() {
    this.hubConnection.on('ExamSubmittedByStudent', (data: Notification) => {
      console.log('Received notification:', data);
      this.notificationSubject.next(data);
    });

    this.hubConnection.onreconnecting((error) => {
      console.warn('SignalR attempting to reconnect...', error);
    });

    this.hubConnection.onreconnected((connectionId) => {
      console.log('SignalR reconnected successfully:', connectionId);
    });

    this.hubConnection.onclose((error) => {
      console.error('SignalR connection closed. Reconnecting...', error);
      this.startConnection();
    });
  }

  public stopConnection() {
    if (this.hubConnection.state !== HubConnectionState.Disconnected) {
      this.hubConnection
        .stop()
        .then(() => console.log('SignalR Connection Stopped'))
        .catch((err) =>
          console.error('Error stopping SignalR connection: ', err)
        );
    }
  }

  ngOnDestroy(): void {
    this.stopConnection();
  }
}
