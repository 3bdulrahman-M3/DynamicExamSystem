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
  selectedAnswers: { [questionId: number]: number } = {};
  isLoading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  examTitle = '';
  private baseApiUrl = 'http://localhost:5063/api';
  timer: string = '05:00';
  interval: any;
  currentQuestionIndex: number = 0;
  unansweredQuestions: number[] = [];

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.examId = +this.route.snapshot.paramMap.get('examId')!;
    this.fetchQuestions();
    this.route.queryParams.subscribe((params) => {
      this.examTitle = params['title'] || 'Exam';
    });

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
    this.selectedAnswers[questionId] = answerId;
    this.checkUnansweredQuestions();
  }

  checkUnansweredQuestions(): void {
    this.unansweredQuestions = this.questions
      .filter((question) => !this.selectedAnswers[question.id])
      .map((question) => question.id);
  }

  refreshComponentState(): void {
    this.selectedAnswers = {};
    this.unansweredQuestions = [];
    this.successMessage = '';
    this.errorMessage = '';
    this.fetchQuestions();
  }

  submitExam(): void {
    this.checkUnansweredQuestions();
    if (this.unansweredQuestions.length > 0) {
      this.errorMessage = `You have unanswered questions: ${this.unansweredQuestions.join(
        ', '
      )}. Please answer all questions before submitting.`;
      return;
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
        next: () => {
          this.successMessage = 'Exam submitted successfully!';
          this.refreshComponentState(); 
          setTimeout(() => {
            this.router.navigate(['user/dashboard']); 
          }, 500); 
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to submit the exam. Please try again.';
        },
      });
  }

  startTimer(): void {
    let minutes = 5;
    let seconds = 0;

    this.interval = setInterval(() => {
      if (seconds === 0) {
        if (minutes === 0) {
          clearInterval(this.interval);
          this.saveAnswersAndSubmit(); 
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
    }, 1000);
  }

  saveAnswersAndSubmit(): void {
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
        next: () => {
          this.successMessage = 'Time is up! Exam submitted successfully.';
          this.refreshComponentState();
          setTimeout(() => {
            this.router.navigate(['user/dashboard']); 
          }, 500);
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to submit the exam. Please try again.';
        },
      });
  }
}
