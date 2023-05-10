import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { faEdit, faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WordCollectionsService } from 'src/app/services/word-collections.service';
import { englishLevels } from 'src/app/shared/contracts/englishLevelOption';
import { WordCollection } from 'src/app/shared/contracts/wordCollection';
import { EnglishLevel } from 'src/app/shared/enums/englishLevel';

@Component({
  selector: 'app-word-collection-list',
  templateUrl: './word-collection-list.component.html',
  styleUrls: ['./word-collection-list.component.scss'],
})
export class WordCollectionListComponent {
  readonly filteringAllValue: number = -1;
  collections: WordCollection[];
  faPlus = faPlus;
  faTrash = faTrash;
  faEdit = faEdit;
  pageNumber = 1;
  pageSize = 18;
  totalCount = 0;
  loading = false;
  englishLevels = englishLevels;
  englishLevelsEnum = EnglishLevel;
  userId: number;

  constructor(
    private collectionService: WordCollectionsService,
    private authService: AuthenticationService,
    public router: Router
  ) {}

  ngOnInit(): void {
    let userId = this.authService.getUser()?.id;
    if (userId) {
      this.userId = userId;
    }
    this.loadCollections();
  }

  loadCollections(englishLevel: EnglishLevel | null = null): void {
    if (this.loading) {
      return;
    }
    this.loading = true;
    this.collections = [];
    this.collectionService
      .getWordCollections(this.pageNumber, this.pageSize, englishLevel)
      .subscribe((result) => {
        if (result.value) {
          this.collections.push(...result.value);
        }
        this.totalCount = result.totalCount;
        this.loading = false;
      });
  }

  onEnglishLevelChange(event: Event) {
    const value = parseInt((event.target as HTMLSelectElement).value);
    const filteringParams = value === this.filteringAllValue ? null : value;
    this.loadCollections(filteringParams);
  }

  deleteWordCollection(id: number) {
    this.collectionService.deleteWordCollection(id).subscribe((id) => {
      let index = this.collections.findIndex((x) => x.id == id);
      if (index !== -1) {
        this.collections.splice(index, 1);
      }
    });
  }

  onScroll(): void {
    if (this.collections.length < this.totalCount && !this.loading) {
      this.pageNumber++;
      this.loadCollections();
    }
  }

  navigateToAddCollection() {
    this.router.navigate(['collections', 'add']);
  }
}
