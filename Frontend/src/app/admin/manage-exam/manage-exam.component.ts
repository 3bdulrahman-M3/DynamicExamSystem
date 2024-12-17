import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common'; // Import CommonModule for directives like ngIf, ngFor
import { FormsModule } from '@angular/forms'; // Import FormsModule for ngModel

@Component({
  selector: 'app-manage-exam',
  standalone: true,
  imports: [CommonModule, FormsModule], // Add FormsModule here
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
  newQuestionText: string = ''; // New question text model

  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  ngOnInit(): void {
    // Retrieve the exam ID from the route parameter
    this.examId = +this.route.snapshot.paramMap.get('id')!;
    this.fetchQuestions();
  }

  fetchQuestions(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.http
      .get<any[]>(`http://localhost:5063/api/Exam/${this.examId}/questions`)
      .subscribe({
        next: (data) => {
          this.questions = data; // Update the questions list
          this.isLoading = false;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to load questions. Please try again.';
          this.isLoading = false;
        },
      });
  }

  addQuestion(): void {
    if (!this.newQuestionText.trim()) {
      this.errorMessage = 'Please enter a question text.';
      return;
    }

    const newQuestion = {
      text: this.newQuestionText,
    };

    this.isLoading = true;
    this.errorMessage = '';

    this.http
      .post<any>(
        `http://localhost:5063/api/Exam/${this.examId}/questions`,
        newQuestion
      )
      .subscribe({
        next: () => {
          this.fetchQuestions(); // Refresh questions after adding the question
          this.newQuestionText = ''; // Clear input field
          this.isLoading = false;
          setTimeout(() => {
            const element = document.getElementById(
              `question-${this.questions.length}`
            );
            if (element) {
              element.scrollIntoView({ behavior: 'smooth' });
            }
          }, 1000); // Scroll after loading
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to add the question. Please try again.';
          this.isLoading = false;
        },
      });
  }

  addAnswer(questionId: number): void {
    const answerText = this.newAnswers[questionId];
    const isCorrect = this.correctAnswers[questionId];

    if (!answerText.trim()) {
      this.errorMessage = 'Please enter an answer text.';
      return;
    }

    // Check if the question already has a correct answer
    const question = this.questions.find((q) => q.id === questionId);
    if (question && isCorrect) {
      // If the question already has a correct answer, prevent adding another one
      const hasCorrectAnswer = question.answers.some(
        (answer: any) => answer.isCorrect
      );
      if (hasCorrectAnswer) {
        this.errorMessage =
          'This question already has a correct answer. You cannot add another correct answer.';
        return;
      }
    }

    const newAnswer = {
      text: answerText,
      isCorrect: isCorrect,
    };

    this.isLoading = true;
    this.errorMessage = '';

    this.http
      .post<string>(
        `http://localhost:5063/api/Questions/${questionId}/Answer`,
        newAnswer,
        {
          responseType: 'text' as 'json',
        }
      )
      .subscribe({
        next: () => {
          alert('Answer added successfully!');
          this.fetchQuestions(); // Refresh questions after adding the answer
          this.newAnswers[questionId] = ''; // Clear input field
          this.isLoading = false;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to add the answer. Please try again.';
          this.isLoading = false;
        },
      });
  }

  deleteAnswer(questionId: number, answerId: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.http
      .delete(`http://localhost:5063/api/Questions/${answerId}`, {
        responseType: 'text',
      })
      .subscribe({
        next: (response: string) => {
          alert(response || 'Answer deleted successfully!');
          this.fetchQuestions(); // Refresh questions after deleting the answer
          this.isLoading = false;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Failed to delete the answer. Please try again.';
          this.isLoading = false;
        },
      });
  }
}
