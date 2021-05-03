import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { NeuronsComponent } from './neurons/neurons.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {
    path: 'neurons',
    component: NeuronsComponent,
    canActivate: [MsalGuard],
  },
  { path: '', component: HomeComponent },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
