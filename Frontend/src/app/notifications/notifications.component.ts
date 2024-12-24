import { Component, OnInit } from '@angular/core';
import { NotificationService } from './signalr.service'; // Update path accordingly
import { CommonModule, DatePipe, NgIf } from '@angular/common';

@Component({
  selector: 'app-notification',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css'],
  imports: [DatePipe, NgIf, CommonModule],
})
export class NotificationComponent implements OnInit {
  showNotification: boolean = false;
  notification: Test = {
    Message: '',
    ExamId: '',
    StudentId: '',
    SubmissionTime: new Date(),
  };

  constructor(private notificationService: NotificationService) {}

  ngOnInit(): void {
    this.notificationService.notification$.subscribe((data: any) => {
      if (data) {
        // Map received keys to match the interface
        this.notification = {
          Message: data.message,
          ExamId: String(data.examId), // Ensure it's a string
          StudentId: data.studentId,
          SubmissionTime: new Date(data.submissionTime), // Convert to Date object
        };
        this.showNotification = true;
        setTimeout(() => {
          this.showNotification = false;
        }, 5000); // Hide notification after 5 seconds
      }
    });
  }
}

export interface Test {
  Message: string;
  ExamId: string;
  StudentId: string;
  SubmissionTime: Date;
}
