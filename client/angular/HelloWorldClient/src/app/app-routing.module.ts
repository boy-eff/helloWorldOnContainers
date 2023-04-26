import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './modules/word-collection/main/main.component';
import { WordCollectionPageComponent } from './modules/word-collection/word-collection-page/word-collection-page.component';
import { LoginComponent } from './modules/account/login/login.component';
import { RegisterComponent } from './modules/account/register/register.component';
import { AccountSettingsComponent } from './modules/account/account-settings/account-settings.component';
import { WordCollectionTestComponent } from './modules/word-collection/word-collection-test/word-collection-test.component';
import { DictionaryPageComponent } from './modules/dictionary/dictionary-page/dictionary-page.component';
import { AchievementListComponent } from './modules/achievements/achievement-list/achievement-list.component';
import { WordCollectionAddComponent } from './modules/word-collection/word-collection-add/word-collection-add.component';

const routes: Routes = [
  { path: 'collections', component: MainComponent },
  { path: 'collections/add', component: WordCollectionAddComponent },
  { path: 'collections/:id', component: WordCollectionPageComponent },
  { path: 'collections/:id/test', component: WordCollectionTestComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'account', component: AccountSettingsComponent },
  { path: 'dictionary', component: DictionaryPageComponent },
  { path: 'achievements', component: AchievementListComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
