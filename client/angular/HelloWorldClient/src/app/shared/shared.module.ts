import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { RouterModule } from '@angular/router';
import { EnumToStringPipe } from './pipes/enum-to-string.pipe';

@NgModule({
  declarations: [HeaderComponent, EnumToStringPipe],
  imports: [CommonModule, RouterModule, NgbModule],
  exports: [HeaderComponent, EnumToStringPipe],
})
export class SharedModule {}
