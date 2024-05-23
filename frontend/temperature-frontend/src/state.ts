import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class State {
  activeCategorySubject = new BehaviorSubject<string | null>(null);
  private selectedOptionSubject = new BehaviorSubject<string | null>(null);

  activeCategory$ = this.activeCategorySubject.asObservable();
  selectedOption$ = this.selectedOptionSubject.asObservable();

  setActiveCategory(category: string | null) {
    this.activeCategorySubject.next(category);
  }

  setSelectedOption(option: string | null) {
    this.selectedOptionSubject.next(option);
  }
}
