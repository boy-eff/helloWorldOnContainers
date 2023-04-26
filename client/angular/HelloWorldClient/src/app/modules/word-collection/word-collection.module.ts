import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { WordCollectionPageComponent } from './word-collection-page/word-collection-page.component';
import { WordCollectionTestComponent } from './word-collection-test/word-collection-test.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { WordCollectionAddComponent } from './word-collection-add/word-collection-add.component';
import { AddWordModalComponent } from './add-word-modal/add-word-modal.component';

@NgModule({
  declarations: [
    MainComponent,
    WordCollectionPageComponent,
    WordCollectionTestComponent,
    WordCollectionAddComponent,
    AddWordModalComponent,
  ],
  imports: [
    CommonModule,
    InfiniteScrollModule,
    NgbModule,
    RouterModule,
    SharedModule,
    FontAwesomeModule,
  ],
})
export class WordCollectionModule {}
