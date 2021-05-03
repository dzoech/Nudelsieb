import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { NeuronsComponent } from './neurons/neurons.component';
import { MsalGuard } from '@azure/msal-angular';

const routes: Routes = [
  {
    path: 'neurons',
    component: NeuronsComponent,
    canActivate: [MsalGuard],
  },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
