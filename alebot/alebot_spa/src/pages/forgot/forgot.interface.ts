export enum forgotStep {
  newPassword = "new-pass",
  email = "forgot",
  confirmation = "confirm",
}

export interface IConfirmEmail {
  email: string;
  code: string;
}

export interface IForgotValues {
  email: string;
  password: string;
  confirmPassword: string;
  phoneNumber:string
}

export interface IForgotErrors {
  email?: string;
  password?: string;
  confirmPassword?: string;
  phoneNumber?: string;
}

export type IOnChange =
  | React.ChangeEventHandler<HTMLTextAreaElement | HTMLInputElement>
  | undefined;
