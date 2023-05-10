import { NgModule } from '@angular/core';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { SharedModule } from 'src/app/shared/shared.module';
import { WordCollectionPageComponent } from './word-collection-page/word-collection-page.component';
import { WordCollectionTestComponent } from './word-collection-test/word-collection-test.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { WordCollectionAddComponent } from './word-collection-add/word-collection-add.component';
import { AddWordModalComponent } from './add-word-modal/add-word-modal.component';
import { WordCollectionListComponent } from './word-collection-list/word-collection-list.component';
import { WordCollectionFormComponent } from './word-collection-form/word-collection-form.component';
import { WordCollectionEditComponent } from './word-collection-edit/word-collection-edit.component';

@NgModule({
  declarations: [
    WordCollectionPageComponent,
    WordCollectionTestComponent,
    WordCollectionAddComponent,
    AddWordModalComponent,
    WordCollectionListComponent,
    WordCollectionFormComponent,
    WordCollectionEditComponent,
  ],
  imports: [SharedModule, InfiniteScrollModule, FontAwesomeModule],
})
export class WordCollectionModule {}
