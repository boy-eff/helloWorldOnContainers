import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { WordCollectionPageComponent } from './word-collection-page/word-collection-page.component';

@NgModule({
  declarations: [MainComponent, WordCollectionPageComponent],
  imports: [
    CommonModule,
    InfiniteScrollModule,
    NgbModule,
    RouterModule,
    SharedModule,
  ],
})
export class WordCollectionModule {}
