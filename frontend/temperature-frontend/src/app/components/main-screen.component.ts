import { Component, OnInit } from '@angular/core';
import { trigger, style, transition, animate } from '@angular/animations';
import { NgForm } from '@angular/forms';
import { State } from '../../state';
import { AuthService } from '../services/auth.service';
import { AccountService } from '../services/account.service';
import { OfficeService } from '../services/office.service';
import { AccountOfficeService } from '../services/account-office.service';
import { ToastController, AlertController } from '@ionic/angular';
import { firstValueFrom } from 'rxjs';
import { ModifyAccountDTO } from '../../models';

@Component({
  selector: 'app-main-screen',
  templateUrl: '../html/main-screen.component.html',
  styleUrls: ['../scss/main-screen.component.scss'],
  animations: [
    trigger('dropdown', [
      transition(':enter', [
        style({ height: 0, opacity: 0 }),
        animate('300ms ease-out', style({ height: '*', opacity: 1 }))
      ]),
      transition(':leave', [
        animate('300ms ease-out', style({ height: 0, opacity: 0 }))
      ])
    ])
  ]
})
export class MainScreenComponent implements OnInit {
  activeCategory$ = this.state.activeCategory$;
  selectedOption$ = this.state.selectedOption$;
  accountName: string;
  officesWithDetails: any[] = [];
  loading: boolean = false;

  constructor(
    private state: State,
    private authService: AuthService,
    private accountService: AccountService,
    private officeService: OfficeService,
    private accountOfficeService: AccountOfficeService,
    private toastController: ToastController,
    private alertController: AlertController
  ) {
    this.accountName = this.accountService.getAccountName() || 'User Account';
  }

  ngOnInit() {
    this.activeCategory$.subscribe(category => {
      if (category === 'office' && !this.selectedOption$) {
        this.loadOfficesWithDetails();
      }
    });
  }

  toggleCategory(category: string) {
    this.state.setActiveCategory(this.state.activeCategorySubject.value === category ? null : category);
  }

  selectOption(option: string) {
    this.state.setSelectedOption(option);
  }

  listOffices() {
    this.state.setActiveCategory('office');
    this.state.setSelectedOption(null);
    this.loadOfficesWithDetails();
  }

  async loadOfficesWithDetails() {
    this.loading = true;
    const accountId = this.accountService.getAccountId();
    try {
      const response = await firstValueFrom(this.accountOfficeService.getOfficesWithDetails(accountId));
      this.officesWithDetails = response.map(officeDetail => ({
        ...officeDetail,
        isEditing: false
      }));
    } catch (error) {
      await this.showErrorMessage('Error fetching offices');
    } finally {
      this.loading = false;
    }
  }

  startEditing(officeDetail: any) {
    officeDetail.isEditing = true;
  }

  async saveOffice(officeDetail: any) {
    this.loading = true;
    try {
      await firstValueFrom(this.officeService.updateOffice({
        id: officeDetail.office.id,
        name: officeDetail.office.name,
        location: officeDetail.office.location
      }));
      officeDetail.isEditing = false;
      await this.showSuccessMessage('Office updated successfully');
    } catch (error) {
      await this.showErrorMessage('Error updating office');
    } finally {
      this.loading = false;
    }
  }

  cancelEditing(officeDetail: any) {
    officeDetail.isEditing = false;
    this.loadOfficesWithDetails(); // Reload to reset changes
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      const modifyAccountData: ModifyAccountDTO = form.value;
      this.accountService.modifyAccount(modifyAccountData).subscribe({
        next: async (response) => {
          await this.showSuccessMessage('Account modified successfully');
        },
        error: async (error) => {
          await this.showErrorMessage('Error modifying account');
        }
      });
    }
  }

  async onCreateOffice(form: NgForm) {
    if (form.valid) {
      const officeData = form.value;
      try {
        const createOfficeResponse = await firstValueFrom(this.officeService.createOffice(officeData));
        const officeId = createOfficeResponse.responseData;
        const accountId = this.accountService.getAccountId();

        await firstValueFrom(this.accountOfficeService.linkAccountToOffice({
          account_id: accountId,
          office_id: officeId,
          account_rank: 0
        }));

        await this.showSuccessMessage('Office created and linked successfully');
        this.clearState();
      } catch (error) {
        await this.showErrorMessage('Error creating office');
      }
    }
  }

  async confirmDeleteAccount() {
    const alert = await this.alertController.create({
      header: 'Confirm Delete',
      message: `Are you sure you want to delete the account ${this.accountName}?`,
      buttons: [
        {
          text: 'Delete',
          cssClass: 'delete-button',
          handler: async () => {
            this.accountService.deleteAccount().subscribe({
              next: async (response) => {
                await this.showSuccessMessage('Account deleted successfully');
                this.clearState();
                this.authService.logout();
              },
              error: async (error) => {
                await this.showErrorMessage('Error deleting account');
              }
            });
          }
        },
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'cancel-button',
          handler: () => {
            console.log('Delete cancelled');
          }
        }
      ]
    });

    await alert.present();
  }

  cancelDeleteAccount() {
    this.state.setSelectedOption(null);
  }

  logout() {
    this.clearState();
    this.authService.logout();
  }

  clearState() {
    this.state.setSelectedOption(null);
    this.state.setActiveCategory(null);
  }

  private async showSuccessMessage(message: string) {
    const toast = await this.toastController.create({
      message,
      duration: 3000,
      color: 'success',
    });
    await toast.present();
  }

  private async showErrorMessage(message: string) {
    const toast = await this.toastController.create({
      message,
      duration: 3000,
      color: 'danger',
    });
    await toast.present();
  }
}
