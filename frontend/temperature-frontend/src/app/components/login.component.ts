import { Component } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { firstValueFrom } from "rxjs";
import { ResponseDto } from "../../models";
import { environment } from "../../environments/environment";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { ToastController } from "@ionic/angular";
import { State } from "../../state";
import { Router } from '@angular/router';
import { TokenService } from "../services/token.service";
import { trigger, style, transition, animate } from '@angular/animations';

@Component({
  template: `
    <div id="page-container">
      <div @fadeInAnimation class="login-container" *ngIf="isLoginMode">
        <h2 @fadeInAnimation>Project Temperature</h2>
        <form [formGroup]="loginForm" (ngSubmit)="login()">
          <div class="form-group" @fadeInAnimation>
            <label for="username">Username</label>
            <input id="username" type="text" formControlName="username" required>
            <div class="error-message" *ngIf="loginForm.get('username')?.invalid && (loginForm.get('username')?.dirty || loginForm.get('username')?.touched)">
              <div *ngIf="loginForm.get('username')?.errors?.['required']">Username is required.</div>
            </div>
          </div>
          <div class="form-group" @fadeInAnimation>
            <label for="password">Password</label>
            <input id="password" type="password" formControlName="password" required>
            <div class="error-message" *ngIf="loginForm.get('password')?.invalid && (loginForm.get('password')?.dirty || loginForm.get('password')?.touched)">
              <div *ngIf="loginForm.get('password')?.errors?.['required']">Password is required.</div>
              <div *ngIf="loginForm.get('password')?.errors?.['minlength']">Password must be at least 6 characters long.</div>
            </div>
          </div>
          <button type="submit" [disabled]="loginForm.invalid" @fadeInAnimation>Login</button>
        </form>
        <p @fadeInAnimation class="switch-mode" (click)="switchMode()">Don't have an account? Register</p>
      </div>

      <div @fadeInAnimation class="login-container" *ngIf="!isLoginMode">
        <h2 @fadeInAnimation>Register</h2>
        <form [formGroup]="registerForm" (ngSubmit)="register()">
          <div class="form-group" @fadeInAnimation>
            <label for="name">Name</label>
            <input id="name" type="text" formControlName="name" required>
            <div class="error-message" *ngIf="registerForm.get('name')?.invalid && (registerForm.get('name')?.dirty || registerForm.get('name')?.touched)">
              <div *ngIf="registerForm.get('name')?.errors?.['required']">Name is required.</div>
            </div>
          </div>
          <div class="form-group" @fadeInAnimation>
            <label for="email">Email</label>
            <input id="email" type="email" formControlName="email" required>
            <div class="error-message" *ngIf="registerForm.get('email')?.invalid && (registerForm.get('email')?.dirty || registerForm.get('email')?.touched)">
              <div *ngIf="registerForm.get('email')?.errors?.['required']">Email is required.</div>
              <div *ngIf="registerForm.get('email')?.errors?.['email']">Invalid email format.</div>
            </div>
          </div>
          <div class="form-group" @fadeInAnimation>
            <label for="password">Password</label>
            <input id="password" type="password" formControlName="password" required>
            <div class="error-message" *ngIf="registerForm.get('password')?.invalid && (registerForm.get('password')?.dirty || registerForm.get('password')?.touched)">
              <div *ngIf="registerForm.get('password')?.errors?.['required']">Password is required.</div>
              <div *ngIf="registerForm.get('password')?.errors?.['minlength']">Password must be at least 6 characters long.</div>
            </div>
          </div>
          <button type="submit" [disabled]="registerForm.invalid" @fadeInAnimation>Register</button>
        </form>
        <p @fadeInAnimation class="switch-mode" (click)="switchMode()">Already have an account? Login</p>
      </div>
    </div>
  `,
  styleUrls: ['../scss/login.component.scss'],
  selector: 'login-component',
  animations: [
    trigger('fadeInAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-50px)' }), // Start further up
        animate('1000ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ])
  ]
})
export class LoginComponent {
  isLoginMode = true;

  loginForm = this.fb.group({
    username: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  registerForm = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  constructor(
    public fb: FormBuilder,
    public http: HttpClient,
    public state: State,
    public toastController: ToastController,
    private router: Router,
    private tokenService: TokenService
  ) {}

  switchMode() {
    this.isLoginMode = !this.isLoginMode;
  }

  async login() {
    try {
      const response = await firstValueFrom(
        this.http.post<ResponseDto<string>>(
          environment.baseURL + '/api/Account/login',
          this.loginForm.getRawValue()
        )
      );

      response.responseData !== null
        ? this.onSuccessfulLogin(response.responseData!)
        : await this.showErrorMessage("Wrong username or password!");
    } catch (error) {
      if (error instanceof HttpErrorResponse) {
        await this.showErrorMessage(error.error.messageToClient);
      }
    }
  }

  async register() {
    try {
      const response = await firstValueFrom(
        this.http.post<ResponseDto<string>>(
          environment.baseURL + '/api/Account/register',
          this.registerForm.getRawValue()
        )
      );

      if (response.responseData) {
        await this.showSuccessMessage("Registration successful! You can now log in.");
        this.switchMode();
      } else {
        await this.showErrorMessage("Registration failed. Please try again.");
      }
    } catch (error) {
      if (error instanceof HttpErrorResponse) {
        await this.showErrorMessage(error.error.messageToClient);
      }
    }
  }

  async onSuccessfulLogin(token: string) {
    this.tokenService.saveToken(token);
    this.router.navigate(['/main-screen']);
  }

  private async showErrorMessage(message: string) {
    const toast = await this.toastController.create({
      message,
      duration: 4500,
      color: "danger",
    });
    await toast.present();
  }

  private async showSuccessMessage(message: string) {
    const toast = await this.toastController.create({
      message,
      duration: 4500,
      color: "success",
    });
    await toast.present();
  }
}
