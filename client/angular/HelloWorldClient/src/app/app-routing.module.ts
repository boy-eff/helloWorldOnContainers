import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountSettingsComponent } from './modules/account/account-settings/account-settings.component';
import { LoginComponent } from './modules/account/login/login.component';
import { RegisterComponent } from './modules/account/register/register.component';
import { AchievementListComponent } from './modules/achievements/achievement-list/achievement-list.component';
import { WordCollectionAddComponent } from './modules/word-collection/word-collection-add/word-collection-add.component';
import { WordCollectionListComponent } from './modules/word-collection/word-collection-list/word-collection-list.component';
import { WordCollectionPageComponent } from './modules/word-collection/word-collection-page/word-collection-page.component';
import { WordCollectionTestComponent } from './modules/word-collection/word-collection-test/word-collection-test.component';
import { ErrorPageComponent } from './shared/components/error-page/error-page.component';
import { AuthGuard } from './shared/guards/auth.guard';
import { WordCollectionEditComponent } from './modules/word-collection/word-collection-edit/word-collection-edit.component';

const routes: Routes = [
  {
    path: '',
    canActivate: [AuthGuard],
    children: [
      { path: '', component: WordCollectionListComponent },
      { path: 'collections/add', component: WordCollectionAddComponent },
      { path: 'collections/:id', component: WordCollectionPageComponent },
      { path: 'collections/:id/edit', component: WordCollectionEditComponent },
      { path: 'collections/:id/test', component: WordCollectionTestComponent },
      { path: 'account', component: AccountSettingsComponent },
      { path: 'achievements', component: AchievementListComponent },
      { path: 'error', component: ErrorPageComponent },
    ],
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
