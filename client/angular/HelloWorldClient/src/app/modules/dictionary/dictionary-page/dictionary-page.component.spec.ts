import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DictionaryPageComponent } from './dictionary-page.component';

describe('DictionaryPageComponent', () => {
  let component: DictionaryPageComponent;
  let fixture: ComponentFixture<DictionaryPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DictionaryPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DictionaryPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
