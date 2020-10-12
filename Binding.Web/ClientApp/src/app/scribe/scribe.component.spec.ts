import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScribeComponent } from './scribe.component';

describe('ScribeComponent', () => {
  let component: ScribeComponent;
  let fixture: ComponentFixture<ScribeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScribeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScribeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
