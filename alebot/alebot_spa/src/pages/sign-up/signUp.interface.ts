export interface ISignUpValues {
  email: string;
  password: string;
  confirmPassword: string;
  phoneNumber: string;
  fullName: string;
}

export interface ISignUpErrors {
  phoneNumber?: string;
  email?: string;
  password?: string;
  confirmPassword?: string;
  fullName?: string;
}

export enum signUpSteps {
  registration = "reg",
  confirmation = 'confirm'
}
export type IOnChange =
  | React.ChangeEventHandler<HTMLTextAreaElement | HTMLInputElement>
  | undefined;
