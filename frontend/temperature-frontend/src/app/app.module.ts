import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';
import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { LoginComponent } from './components/login.component';
import { MainScreenComponent } from './components/main-screen.component';
import { FormsModule } from '@angular/forms';

import { AuthInterceptor } from './interceptors/auth.interceptor';
import { TokenService } from './services/token.service';
import { AccountService } from './services/account.service';
import { AuthService } from './services/auth.service';
import { State } from '../state';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    MainScreenComponent
  ],
  imports: [
    BrowserModule,
    IonicModule.forRoot(),
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule
  ],
  providers: [
    { provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    TokenService,
    AccountService,
    AuthService,
    State
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
