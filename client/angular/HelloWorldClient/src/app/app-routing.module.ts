import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './modules/auth/login/login.component';
import { RegisterComponent } from './modules/auth/register/register.component';
import { MainComponent } from './modules/word-collection/main/main.component';
import { WordCollectionPageComponent } from './modules/word-collection/word-collection-page/word-collection-page.component';

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
