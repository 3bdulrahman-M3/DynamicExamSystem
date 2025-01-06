import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-exam',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './manage-exam.component.html',
  styleUrls: ['./manage-exam.component.css'],
})
export class ManageExamComponent implements OnInit {
  examId!: number;
  questions: any[] = [];
  errorMessage: string = '';
  isLoading: boolean = false;
  newAnswers: { [key: number]: string } = {};
  correctAnswers: { [key: number]: boolean } = {};
  newQuestionText: string = '';
  updatedQuestionText: { [key: number]: string } = {};

  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  ngOnInit(): void {
    this.examId = +this.route.snapshot.paramMap.get('id')!;
    this.fetchQuestions();
  }

  // Fetch questions
  fetchQuestions(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.http
      .get<any[]>(`http://localhost:5063/api/Exam/${this.examId}/questions`)
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

  // Refresh component state
  refreshComponent(): void {
    this.fetchQuestions(); // Re-fetch questions
    // Reset any temporary UI states here if needed
  }

  // Add a new question
  addQuestion(): void {
    if (!this.newQuestionText.trim()) {
      this.errorMessage = 'Please enter a question text.';
      return;
    }

    const newQuestion = { text: this.newQuestionText };

    this.isLoading = true;
    this.errorMessage = '';

    this.http
      .post<any>(
        `http://localhost:5063/api/Exam/${this.examId}/questions`,
        newQuestion
      )
      .subscribe({
        next: () => {
          this.newQuestionText = ''; // Clear input
          this.refreshComponent(); // Refresh component
          this.isLoading = false;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to add the question. Please try again.';
          this.isLoading = false;
        },
      });
  }

  // Update an existing question
  updateQuestion(questionId: number): void {
    const updatedText = this.updatedQuestionText[questionId];

    if (!updatedText.trim()) {
      this.errorMessage = 'Please enter a question text.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.http
      .put(
        `http://localhost:5063/api/Exam/${this.examId}/questions/${questionId}`,
        { text: updatedText },
        { responseType: 'text' }
      )
      .subscribe({
        next: () => {
          this.updatedQuestionText[questionId] = ''; // Clear input
          this.refreshComponent(); // Refresh component
          this.isLoading = false;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage =
            'Failed to update the question. Please try again.';
          this.isLoading = false;
        },
      });
  }

  // Delete a question
  deleteQuestion(questionId: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.http
      .delete(`http://localhost:5063/api/Exam/DeleteQuestion/${questionId}`, {
        responseType: 'text',
      })
      .subscribe({
        next: () => {
          this.refreshComponent(); // Refresh component
          this.isLoading = false;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage =
            'Failed to delete the question. Please try again.';
          this.isLoading = false;
        },
      });
  }

  // Add an answer to a question
  addAnswer(questionId: number): void {
    const answerText = this.newAnswers[questionId];
    const isCorrect = this.correctAnswers[questionId];

    if (!answerText.trim()) {
      this.errorMessage = 'Please enter an answer text.';
      return;
    }

    const question = this.questions.find((q) => q.id === questionId);
    if (question && isCorrect) {
      const hasCorrectAnswer = question.answers.some(
        (answer: any) => answer.isCorrect
      );
      if (hasCorrectAnswer) {
        this.errorMessage =
          'This question already has a correct answer. You cannot add another correct answer.';
        return;
      }
    }

    const newAnswer = { text: answerText, isCorrect: isCorrect };

    this.isLoading = true;
    this.errorMessage = '';

    this.http
      .post<string>(
        `http://localhost:5063/api/Questions/${questionId}/Answer`,
        newAnswer,
        { responseType: 'text' as 'json' }
      )
      .subscribe({
        next: () => {
          this.newAnswers[questionId] = ''; // Clear input
          this.refreshComponent(); // Refresh component
          this.isLoading = false;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to add the answer. Please try again.';
          this.isLoading = false;
        },
      });
  }

  // Delete an answer
  deleteAnswer(questionId: number, answerId: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.http
      .delete(`http://localhost:5063/api/Questions/${answerId}`, {
        responseType: 'text',
      })
      .subscribe({
        next: () => {
          this.refreshComponent(); // Refresh component
          this.isLoading = false;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to delete the answer. Please try again.';
          this.isLoading = false;
        },
      });
  }

  // Start editing a question
  startEditingQuestion(questionId: number, questionText: string): void {
    this.updatedQuestionText[questionId] = questionText;
  }
}
