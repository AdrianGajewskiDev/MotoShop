import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { IdentityPlaceholderComponent } from './identity/identity-placeholder/identity-placeholder.component';


const routes: Routes = [
  {
    path: "identity",
    component: IdentityPlaceholderComponent
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
