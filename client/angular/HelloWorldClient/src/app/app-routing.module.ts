import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './modules/word-collection/main/main.component';
import { WordCollectionPageComponent } from './modules/word-collection/word-collection-page/word-collection-page.component';
import { LoginComponent } from './modules/account/login/login.component';
import { RegisterComponent } from './modules/account/register/register.component';

const routes: Routes = [
  { path: 'collections', component: MainComponent },
  { path: 'collections/:id', component: WordCollectionPageComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
