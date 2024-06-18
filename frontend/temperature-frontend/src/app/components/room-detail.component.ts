import { Component, Input, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { State } from '../../state';
import { RoomService } from '../services/room.service';
import { OfficeService } from '../services/office.service';
import { firstValueFrom, Subscription } from 'rxjs';
import { ToastController, AlertController } from '@ionic/angular';
import { WebSocketService } from '../services/websocket.service';
import { environment } from '../../environments/environment'; // Import environment

@Component({
  selector: 'app-room-detail',
  templateUrl: '../html/room-detail.component.html',
  styleUrls: ['../scss/room-detail.component.scss']
})
export class RoomDetailComponent implements OnInit, OnDestroy {
  @Input() roomDetails: any;
  editRoomForm!: FormGroup;
  isEditing = false;
  private wsSubscription?: Subscription;
  public officeName: string = ''; // Changed from private to public
  public defaultHumidityTreshold: number = 50; // Default value
  public defaultHumidityMax: number = 70; // Default value

  constructor(
    private state: State,
    private roomService: RoomService,
    private officeService: OfficeService,
    private toastController: ToastController,
    private alertController: AlertController,
    private fb: FormBuilder,
    private webSocketService: WebSocketService,
    private cdr: ChangeDetectorRef
  ) {}

  async ngOnInit() {
    // Set default values for humidityTreshold and humidityMax if they are undefined
    this.editRoomForm = this.fb.group({
      name: [this.roomDetails.name],
      office_name: [this.officeName], // Adjusted to match the new DTO
      desired_temp: [this.roomDetails.desired_temp],
      window_toggle: [this.roomDetails.window_toggle],
      humidityTreshold: [this.roomDetails.humidityTreshold || this.defaultHumidityTreshold],
      humidityMax: [this.roomDetails.humidityMax || this.defaultHumidityMax]
    });

    await this.loadOfficeName();
    this.connectWebSocket();
  }

  ngOnDestroy() {
    this.disconnectWebSocket();
  }

  async loadOfficeName() {
    try {
      const officeDetails = await firstValueFrom(this.officeService.getOfficeById(this.roomDetails.office_id));
      this.officeName = officeDetails.responseData.name; // Extract the office name
      this.editRoomForm.patchValue({ office_name: this.officeName });
      this.cdr.detectChanges();
    } catch (error) {
      console.error('Error fetching office details:', error);
    }
  }

  connectWebSocket() {
    const roomName = this.roomDetails.name;
    const wsUrl = environment.wsUrl; // Get WebSocket URL from environment

    this.wsSubscription = this.webSocketService.connect().subscribe({
      next: (message: string) => this.handleWebSocketMessage(message),
      error: (error: any) => console.error('WebSocket error', error),
      complete: () => console.log('WebSocket connection closed')
    });

    this.webSocketService.sendMessage(`join:${this.officeName}/${roomName}`);
  }

  disconnectWebSocket() {
    if (this.wsSubscription) {
      this.webSocketService.sendMessage(`leave:${this.officeName}/${this.roomDetails.name}`);
      this.wsSubscription.unsubscribe();
    }
  }

  handleWebSocketMessage(message: string) {
    try {
      const data = JSON.parse(message);
      if (data.source) {
        // Update roomDetails directly
        this.roomDetails.desired_temp = data.targetTemperature;
        this.roomDetails.window_toggle = data.toggle;
        this.roomDetails.humidityTreshold = data.humidityTreshold;
        this.roomDetails.humidityMax = data.humidityMax;

        // Update the form values to reflect the new data
        this.editRoomForm.patchValue({
          desired_temp: data.targetTemperature,
          window_toggle: data.toggle,
          humidityTreshold: data.humidityTreshold,
          humidityMax: data.humidityMax
        });

        // Trigger change detection to update the UI
        this.cdr.detectChanges();
      }
    } catch (e) {
      console.log('Non-JSON message received:', message);
    }
  }

  goBack() {
    this.disconnectWebSocket();
    this.state.setSelectedOption(null);
  }

  startEditing() {
    this.isEditing = true;
    this.cdr.detectChanges();
  }

  cancelEditing() {
    this.isEditing = false;
    this.cdr.detectChanges();
  }

  async submitEdit() {
    if (this.editRoomForm.valid) {
      try {
        const updateData = {
          name: this.editRoomForm.value.name,
          office_name: this.editRoomForm.value.office_name,
          desired_temp: this.editRoomForm.value.desired_temp,
          window_toggle: this.editRoomForm.value.window_toggle,
          humidityTreshold: this.editRoomForm.value.humidityTreshold,
          humidityMax: this.editRoomForm.value.humidityMax
        };

        await firstValueFrom(this.roomService.updateTemperature(updateData));
        this.showSuccessMessage('Room updated successfully');
        this.isEditing = false;
        this.cdr.detectChanges();
      } catch (error) {
        this.showErrorMessage('Error updating room');
      }
    }
  }

  async confirmDelete() {
    const alert = await this.alertController.create({
      header: 'Confirm Delete',
      message: `Are you sure you want to delete the room ${this.roomDetails.name}?`,
      buttons: [
        {
          text: 'Delete',
          cssClass: 'delete-button',
          handler: async () => {
            await this.deleteRoom();
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

  async deleteRoom() {
    try {
      await firstValueFrom(this.roomService.deleteRoom(this.roomDetails.id));
      await this.showSuccessMessage('Room deleted successfully');
      this.goBack();
    } catch (error) {
      await this.showErrorMessage('Error deleting room');
    }
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
