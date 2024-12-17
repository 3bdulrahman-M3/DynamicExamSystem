import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegiterationComponent } from './regiteration.component';

describe('RegiterationComponent', () => {
  let component: RegiterationComponent;
  let fixture: ComponentFixture<RegiterationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegiterationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegiterationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
