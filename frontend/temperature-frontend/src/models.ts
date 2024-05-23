export class ResponseDto<T> {
  responseData?: T;
  messageToClient?: string;
}

export class Account {
  id!: number;
  name!: string;
  email!: string;
}
export interface ModifyAccountDTO {
  name: string;
  email: string;
  password: string;
}

