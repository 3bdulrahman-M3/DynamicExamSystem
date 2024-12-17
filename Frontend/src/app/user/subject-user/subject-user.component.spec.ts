import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubjectUserComponent } from './subject-user.component';

describe('SubjectUserComponent', () => {
  let component: SubjectUserComponent;
  let fixture: ComponentFixture<SubjectUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubjectUserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubjectUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
