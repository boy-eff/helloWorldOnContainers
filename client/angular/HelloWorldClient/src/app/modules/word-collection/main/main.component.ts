import { EnglishLevel } from '../../../shared/enums/englishLevel';
import { Component, OnInit } from '@angular/core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { WordCollectionsService } from 'src/app/services/word-collections.service';
import { WordCollection } from 'src/app/shared/contracts/wordCollection';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent implements OnInit {
  collections: WordCollection[] = [];
  faPlus = faPlus;
  pageNumber = 1;
  pageSize = 18;
  totalCount = 0;
  loading = false;
  englishLevel = EnglishLevel;

  constructor(private collectionService: WordCollectionsService) {}

  ngOnInit(): void {
    this.loadCollections();
  }

  loadCollections(): void {
    if (this.loading) {
      return;
    }
    this.loading = true;

    this.collectionService
      .getWordCollections(this.pageNumber, this.pageSize)
      .subscribe((result) => {
        if (result.value) {
          this.collections.push(...result.value);
        }
        this.totalCount = result.totalCount;
        this.loading = false;
      });
  }

  onScroll(): void {
    if (this.collections.length < this.totalCount && !this.loading) {
      this.pageNumber++;
      this.loadCollections();
    }
  }
}
