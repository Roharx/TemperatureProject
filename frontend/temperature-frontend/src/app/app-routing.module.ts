import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainScreenComponent } from './components/main-screen.component';
import { LoginComponent } from './components/login.component';
import { RoomDetailComponent } from './components/room-detail.component';
import { authGuard } from './services/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'main-screen', component: MainScreenComponent, canActivate: [authGuard] },
  { path: 'room-detail', component: RoomDetailComponent, canActivate: [authGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
