import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-start-exam',
  standalone: true,
  templateUrl: './start-exam.component.html',
  styleUrls: ['./start-exam.component.css'],
  imports: [NgFor, NgIf],
})
export class StartExamComponent implements OnInit {
  examId!: number;
  questions: any[] = [];
  selectedAnswers: { [questionId: number]: number } = {}; // Tracks user-selected answers
  isLoading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';
  examTitle='';
  private baseApiUrl = 'http://localhost:5063/api';

  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  ngOnInit(): void {
    this.examId = +this.route.snapshot.paramMap.get('examId')!;
    this.fetchQuestions();
    this.examId = +this.route.snapshot.paramMap.get('examId')!;
    this.route.queryParams.subscribe((params) => {
      this.examTitle = params['title'] || 'Exam'; // Get the title from the query parameters
    });
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
  }

  submitExam(): void {
    // Prepare the payload for submission
    console.log(this.selectedAnswers);
    const answersPayload = Object.keys(this.selectedAnswers).map(
      (questionId) => ({
        questionId: +questionId,
        answerId: this.selectedAnswers[+questionId],
      })
    );
    console.log(answersPayload);

    // this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';
    console.log(answersPayload);
    this.http
      .post(
        `${this.baseApiUrl}/Exam/exam/${this.examId}/submit`,
        answersPayload
      )
      .subscribe({
        next: (response: any) => {
          this.successMessage = 'Exam submitted successfully!';
          this.isLoading = false;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to submit the exam. Please try again.';
          this.isLoading = false;
        },
      });
  }
}
