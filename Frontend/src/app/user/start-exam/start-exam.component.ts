import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-start-exam',
  templateUrl: './start-exam.component.html',
  styleUrls: ['./start-exam.component.css'],
  imports: [NgIf, NgFor],
})
export class StartExamComponent implements OnInit, OnDestroy {
  examId!: number;
  questions: any[] = [];
  selectedAnswers: { [questionId: number]: number } = {}; // Tracks user-selected answers
  isLoading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  examTitle = '';
  private baseApiUrl = 'http://localhost:5063/api';
  timer: string = '01:00'; // Timer set to 1 minute initially
  interval: any;
  currentQuestionIndex: number = 0; // Tracks the current question number
  unansweredQuestions: number[] = []; // Tracks unanswered questions

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router // Injecting Router to handle navigation
  ) {}

  ngOnInit(): void {
    this.examId = +this.route.snapshot.paramMap.get('examId')!;
    this.fetchQuestions();
    this.route.queryParams.subscribe((params) => {
      this.examTitle = params['title'] || 'Exam'; // Get the title from the query parameters
    });

    // Start the timer countdown
    this.startTimer();
  }

  ngOnDestroy(): void {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }

  fetchQuestions(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.http
      .get<any[]>(`${this.baseApiUrl}/Exam/${this.examId}/questions`)
      .subscribe({
        next: (data) => {
          this.questions = data;
          this.isLoading = false;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to load questions. Please try again.';
          this.isLoading = false;
        },
      });
  }

  selectAnswer(questionId: number, answerId: number): void {
    this.selectedAnswers[questionId] = answerId; // Update selected answer for a question
    this.checkUnansweredQuestions(); // Recheck unanswered questions whenever a selection is made
  }

  checkUnansweredQuestions(): void {
    // Update unanswered questions list based on selected answers
    this.unansweredQuestions = this.questions
      .filter((question) => !this.selectedAnswers[question.id])
      .map((question) => question.id);
  }

  submitExam(): void {
    // Ensure all questions have been answered before submitting
     this.router.navigate(['user/dashboard']);
    this.checkUnansweredQuestions(); // Recheck unanswered questions before submission
    if (this.unansweredQuestions.length > 0) {
      this.errorMessage = `You have unanswered questions: ${this.unansweredQuestions.join(
        ', '
      )}. Please answer all questions before submitting.`;
      return; // Stop submission if there are unanswered questions
    }

    const answersPayload = Object.keys(this.selectedAnswers).map(
      (questionId) => ({
        questionId: +questionId,
        answerId: this.selectedAnswers[+questionId],
      })
    );

    this.http
      .post(
        `${this.baseApiUrl}/Exam/exam/${this.examId}/submit`,
        answersPayload
      )
      .subscribe({
        next: (response: any) => {
          this.successMessage = 'Exam submitted successfully!';
          // Redirect to the dashboard after successful submission
          this.router.navigate(['user/dashboard']);
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to submit the exam. Please try again.';
        },
      });
  }

  startTimer(): void {
    let minutes = 5; // Timer set to 5 minutes initially
    let seconds = 0;

    this.interval = setInterval(() => {
      if (seconds === 0) {
        if (minutes === 0) {
          clearInterval(this.interval);
          // Check if all questions are answered before submitting when timer ends
          this.submitExam(); // Automatically submit after timer ends
        } else {
          minutes--;
          seconds = 59;
        }
      } else {
        seconds--;
      }

      this.timer = `${minutes < 10 ? '0' + minutes : minutes}:${
        seconds < 10 ? '0' + seconds : seconds
      }`;
    }, 1000); // Update every second
  }
}
