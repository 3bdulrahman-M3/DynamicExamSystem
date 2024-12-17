import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AppComponent } from './app.component';
import { HomepageComponent } from './homepage/homepage.component';
import { RegiterationComponent } from './regiteration/regiteration.component';
import { RoleGuard } from './role.guard';
import { UserDashboardComponent } from './user/user-dashboard/user-dashboard.component';
import { AdminComponent } from './admin/admin.component';
import { AdminDashboardComponent } from './admin/admin-dashboard/admin-dashboard.component';
import { SubjectComponent } from './admin/subject/subject.component';
import { SubjectUserComponent } from './user/subject-user/subject-user.component';
import { UserComponent } from './user/user.component';
import { StartExamComponent } from './user/start-exam/start-exam.component';
import { ExamComponent } from './admin/exam/exam.component';
import { ManageExamComponent } from './admin/manage-exam/manage-exam.component';
import { ReportsComponent } from './user/reports/reports.component';
import { ExamReportsComponent } from './admin/exam-reports/exam-reports.component';
import { StudentComponent } from './admin/student/student.component';
import { ProfileComponent } from './profile/profile.component';
export const routes: Routes = [
  {
    path: '',
    component: HomepageComponent,
  },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'register',
    component: RegiterationComponent,
  },

  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [RoleGuard],
    children: [
      { path: 'dashboard', component: AdminDashboardComponent },
      { path: 'subject', component: SubjectComponent },
      { path: 'exam', component: ExamComponent },
      { path: 'exam/manage-exam/:id', component: ManageExamComponent },
      { path: 'reports', component: ExamReportsComponent },
      { path: 'students', component: StudentComponent },
      { path: 'profile', component: ProfileComponent },
    ],
  },
  {
    path: 'user',
    component: UserComponent,
    canActivate: [RoleGuard],
    children: [
      { path: 'dashboard', component: UserDashboardComponent },
      { path: 'subject', component: SubjectUserComponent },
      { path: 'start-exam/:examId', component: StartExamComponent },
      { path: 'reports', component: ReportsComponent },
      { path: 'profile', component: ProfileComponent },
    ],
  },
];
