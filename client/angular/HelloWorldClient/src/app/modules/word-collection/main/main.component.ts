import { EnglishLevel } from './../../../shared/enums/EnglishLevel';
import { Component, OnInit } from '@angular/core';
import { WordCollectionsService } from 'src/app/services/word-collections.service';
import { WordCollectionModel } from 'src/app/shared/contracts/wordCollectionModel';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent implements OnInit {
  collections: WordCollectionModel[] = [];
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
