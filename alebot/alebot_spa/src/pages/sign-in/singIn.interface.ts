export interface ISignInValues {
  email: string;
  password: string;
  refererId?: string;
  registrationQueryParams?: string;
}

export interface ISignInErrors {
  email?: string;
  password?: string;
}
