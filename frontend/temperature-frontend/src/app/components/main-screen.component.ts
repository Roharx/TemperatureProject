import { Component, OnInit, OnDestroy } from '@angular/core';
import { trigger, style, transition, animate } from '@angular/animations';
import { NgForm } from '@angular/forms';
import { State } from '../../state';
import { AuthService } from '../services/auth.service';
import { AccountService } from '../services/account.service';
import { OfficeService } from '../services/office.service';
import { AccountOfficeService } from '../services/account-office.service';
import { RoomService } from '../services/room.service';
import { ToastController, AlertController } from '@ionic/angular';
import { firstValueFrom, Subscription } from 'rxjs';
import { ModifyAccountDTO, OfficeDetail, Room } from '../../models';
import { ChangeDetectorRef } from '@angular/core';
import { WebSocketService } from '../services/websocket.service';

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
export class MainScreenComponent implements OnInit, OnDestroy {
  activeCategory$ = this.state.activeCategory$;
  selectedOption$ = this.state.selectedOption$;
  accountName: string;
  officesWithDetails: OfficeDetail[] = [];
  loading: boolean = false;
  roomDetails: any;
  private wsSubscriptions: { [roomName: string]: Subscription } = {};
  private reconnectAttempts: { [roomName: string]: number } = {};

  constructor(
    private state: State,
    private authService: AuthService,
    private accountService: AccountService,
    private officeService: OfficeService,
    private accountOfficeService: AccountOfficeService,
    private roomService: RoomService,
    private toastController: ToastController,
    private alertController: AlertController,
    private webSocketService: WebSocketService,
    private cdr: ChangeDetectorRef
  ) {
    this.accountName = this.accountService.getAccountName() || 'User Account';
  }

  ngOnInit() {
    this.loadOfficesWithDetails();
    this.activeCategory$.subscribe(category => {
      if (category === 'office' && !this.selectedOption$) {
        this.loadOfficesWithDetails();
      }
    });
  }

  ngOnDestroy() {
    this.disconnectWebSockets();
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
      this.officesWithDetails = response.map((officeDetail: any) => ({
        ...officeDetail,
        isEditing: false,
        showDeleteConfirmation: false
      }));
      this.connectWebSockets(); // Connect WebSocket for all rooms
      this.cdr.detectChanges(); // Trigger change detection
    } catch (error) {
      await this.showErrorMessage('Error fetching offices');
    } finally {
      this.loading = false;
    }
  }

  async loadRoomDetails(roomId: number) {
    this.loading = true;
    try {
      const response = await firstValueFrom(this.roomService.getRoomById(roomId));
      this.roomDetails = response.responseData;
      this.state.setSelectedOption('Room Details');
      this.cdr.detectChanges(); // Trigger change detection
    } catch (error) {
      await this.showErrorMessage('Error fetching room details');
    } finally {
      this.loading = false;
    }
  }

  connectWebSockets() {
    this.officesWithDetails.forEach(officeDetail => {
      officeDetail.rooms.forEach((room: Room) => {
        const roomName = `${officeDetail.office.name}/${room.name}`;
        if (!this.wsSubscriptions[roomName]) {
          this.subscribeToRoom(roomName);
        }
      });
    });
  }

  subscribeToRoom(roomName: string) {
    this.wsSubscriptions[roomName] = this.webSocketService.connect().subscribe({
      next: (message: string) => this.handleWebSocketMessage(roomName, message),
      error: (error: any) => {
        console.error('WebSocket error', error);
        this.handleWebSocketError(roomName);
      },
      complete: () => console.log('WebSocket connection closed')
    });
    this.webSocketService.sendMessage(`join:${roomName}`);
  }

  handleWebSocketError(roomName: string) {
    this.reconnectAttempts[roomName] = (this.reconnectAttempts[roomName] || 0) + 1;
    const reconnectDelay = Math.min(1000 * Math.pow(2, this.reconnectAttempts[roomName]), 30000); // Exponential backoff with max delay

    setTimeout(() => {
      console.log(`Attempting to reconnect to WebSocket for room ${roomName} (attempt ${this.reconnectAttempts[roomName]})`);
      this.subscribeToRoom(roomName);
    }, reconnectDelay);
  }

  disconnectWebSockets() {
    Object.keys(this.wsSubscriptions).forEach(roomName => {
      this.webSocketService.sendMessage(`leave:${roomName}`);
      this.wsSubscriptions[roomName].unsubscribe();
      delete this.wsSubscriptions[roomName];
    });
  }

  handleWebSocketMessage(roomName: string, message: string) {
    try {
      const data = JSON.parse(message);
      if (data.source) {
        this.updateRoomDetails(roomName, data);
        this.cdr.detectChanges(); // Trigger change detection to update the UI
      }
    } catch (e) {
      console.log('Non-JSON message received:', message);
    }
  }

  async updateRoomDetails(roomName: string, data: any) {
    const [officeName, roomNameOnly] = roomName.split('/');
    const officeDetail = this.officesWithDetails.find(office => office.office.name === officeName);
    if (officeDetail) {
      const roomDetail = officeDetail.rooms.find(room => room.name === roomNameOnly);
      if (roomDetail) {
        roomDetail.desired_temp = data.targetTemperature;
        roomDetail.window_toggle = data.toggle;
        roomDetail.humidityTreshold = data.humidityTreshold;
        roomDetail.humidityMax = data.humidityMax;

        // Optionally, refresh the specific room details from the backend
        const response = await firstValueFrom(this.roomService.getRoomById(roomDetail.id));
        const updatedRoomDetails = response.responseData;

        // Update room details with fresh data from the backend
        Object.assign(roomDetail, updatedRoomDetails);

        this.cdr.detectChanges(); // Trigger change detection to update the UI
      }
    }
  }

  onRoomUpdated() {
    this.loadOfficesWithDetails();
  }

  onRoomDeleted() {
    this.loadOfficesWithDetails();
  }

  toggleDeleteConfirmation(officeId: number) {
    const office = this.officesWithDetails.find(officeDetail => officeDetail.office.id === officeId);
    if (office) {
      office.showDeleteConfirmation = !office.showDeleteConfirmation;
      this.cdr.detectChanges(); // Trigger change detection
    }
  }

  startEditing(officeDetail: OfficeDetail) {
    officeDetail.isEditing = true;
    this.cdr.detectChanges(); // Trigger change detection
  }

  async saveOffice(officeDetail: OfficeDetail) {
    this.loading = true;
    try {
      await firstValueFrom(this.officeService.updateOffice({
        id: officeDetail.office.id,
        name: officeDetail.office.name,
        location: officeDetail.office.location
      }));
      officeDetail.isEditing = false;
      await this.showSuccessMessage('Office updated successfully');
      this.cdr.detectChanges(); // Trigger change detection
    } catch (error) {
      await this.showErrorMessage('Error updating office');
    } finally {
      this.loading = false;
    }
  }

  cancelEditing(officeDetail: OfficeDetail) {
    officeDetail.isEditing = false;
    this.loadOfficesWithDetails(); // Reload to reset changes
  }

  async confirmDeleteOffice(officeId: number) {
    const alert = await this.alertController.create({
      header: 'Confirm Delete',
      message: 'Are you sure you want to delete this office along with all its rooms?',
      buttons: [
        {
          text: 'Yes',
          cssClass: 'delete-button',
          handler: async () => {
            await this.deleteOffice(officeId);
          }
        },
        {
          text: 'No',
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

  async deleteOffice(officeId: number) {
    this.loading = true;
    try {
      await firstValueFrom(this.officeService.deleteOffice(officeId));
      this.officesWithDetails = this.officesWithDetails.filter(officeDetail => officeDetail.office.id !== officeId);
      await this.showSuccessMessage('Office deleted successfully');
      this.cdr.detectChanges(); // Trigger change detection
    } catch (error) {
      await this.showErrorMessage('Error deleting office');
    } finally {
      this.loading = false;
    }
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      const modifyAccountData: ModifyAccountDTO = form.value;
      this.accountService.modifyAccount(modifyAccountData).subscribe({
        next: async (response) => {
          await this.showSuccessMessage('Account modified successfully');
          this.cdr.detectChanges(); // Trigger change detection
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
        this.cdr.detectChanges(); // Trigger change detection
      } catch (error) {
        await this.showErrorMessage('Error creating office');
      }
    }
  }

  async onCreateRoom(form: NgForm) {
    if (form.valid) {
      const roomData = form.value;
      try {
        const createRoomResponse = await firstValueFrom(this.roomService.createRoom(roomData));
        await this.showSuccessMessage('Room created successfully');
        this.clearState();
        this.loadOfficesWithDetails();
      } catch (error) {
        await this.showErrorMessage('Error creating room');
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
                this.cdr.detectChanges(); // Trigger change detection
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
    this.cdr.detectChanges(); // Trigger change detection
  }

  logout() {
    this.clearState();
    this.authService.logout();
    this.cdr.detectChanges(); // Trigger change detection
  }

  clearState() {
    this.state.setSelectedOption(null);
    this.state.setActiveCategory(null);
    this.roomDetails = null;
    this.disconnectWebSockets();
    this.cdr.detectChanges(); // Trigger change detection
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
