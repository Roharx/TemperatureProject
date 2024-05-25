export class ResponseDto<T> {
  responseData?: T;
  messageToClient?: string;
}

export class Account {
  id!: number;
  name!: string;
  email!: string;
}
export interface Room {
  id: number;
  name: string;
  desired_temp: number;
  window_toggle: boolean;
  humidityTreshold?: number;
  humidityMax?: number;
}

export interface Office {
  id: number;
  name: string;
  location: string;
}

export interface OfficeDetail {
  office: Office;
  rooms: Room[];
  isEditing: boolean;
  showDeleteConfirmation: boolean;
}

export interface ModifyAccountDTO {
  name: string;
  email: string;
  password: string;
}
