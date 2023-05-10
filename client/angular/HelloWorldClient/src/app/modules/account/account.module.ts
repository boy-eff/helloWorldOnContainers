import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { HttpClientModule } from '@angular/common/http';
import { AccountSettingsComponent } from './account-settings/account-settings.component';
import { ChangePasswordModalComponent } from './account-settings/change-password-modal/change-password-modal.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    AccountSettingsComponent,
    ChangePasswordModalComponent,
  ],
  imports: [HttpClientModule, SharedModule],
})
export class AccountModule {}
