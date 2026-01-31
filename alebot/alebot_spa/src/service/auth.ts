import { ISignInValues } from "@/pages/sign-in/singIn.interface";
import api from "./api";
import { IConfirmEmail } from "@/pages/forgot/forgot.interface";

export const signInRequest = async (data: ISignInValues) => {
  return await api
    .post(`/login`, data)
    .then((res) => res.data)
    .catch((e) => console.log(e.message));
};

export const signUpRequest = async (data: ISignInValues) => {
  return await api
    .post(`/api/v1/sso/register`, data)
    .then((res) => res.status)
    .catch((e) => console.log(e.message));
};

export const sendConfirmCodeRequest = async (data: { email: string }) => {
  return await api
    .post(
      `/api/v1/sso/send-confirmation-code-to-email`,
      data
    )
    .then((res) => res.data)
    .catch((e) => e);
};

export const confirmEmailRequest = async (data: IConfirmEmail) => {
  return await api
    .post(`/api/v1/sso/comfirm-email`, data)
    .catch((e) => e);
};

export const resetPasswordRequest = async (data: any) => {
  return await api
    .post(`/api/v1/sso/reset-password`, data)
    .catch((e) => e);
};

export const sendResetPasswordCodeRequest = async (data: { email: string }) => {
  return await api
    .post(
      `/api/v1/sso/send-password-reset-code-to-email`,
      data
    )
    .then((res) => res.data)
    .catch((e) => e);
};

export const checkPasswordResetCodeRequest = async (data: {
  email: string;
}) => {
  return await api
    .post(
      `/api/v1/sso/check-password-reset-code-from-email`,
      data
    )
    .then((res) => res.data)
    .catch((e) => e);
};

export const getUserInfoRequest = async () => {
  return await api
    .get(`/api/v1/sso/users/my-user-info`)
    .then((res) => res.data)
    .catch((e) => e);
};
