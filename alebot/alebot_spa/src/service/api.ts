import axios, { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import { enqueueSnackbar } from 'notistack';
import * as amplitude from '@amplitude/analytics-browser';

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_URL,
  timeout: 6000,
  maxRedirects: 0
});

async function updateAccessToken(refreshToken: string): Promise<string> {
  if (!refreshToken)
    return '';
  const response = await api.post(
    "/refresh",
    { refreshToken }
  );
  return response?.data?.accessToken;
}

api.interceptors.request.use(
  (config) => {
    const newConfig = { ...config };
    const token = localStorage.getItem("accessToken");
    if (token) {
      newConfig.headers.Authorization = `Bearer ${token}`;
    }
    newConfig.headers.XAmplitudeDeviceId = amplitude.getDeviceId();
    return newConfig;
  },
  (error) => Promise.reject(error)
);

api.interceptors.response.use(
  (response: AxiosResponse) => response,
  async (error: AxiosError) => {
    const config = error.config as AxiosRequestConfig;
    if (error.response && error.response.status >= 500)
      enqueueSnackbar('Сервер временно не отвечает. Попробуйте обновить страницу через минуту!   \n\r' + error?.response?.data ? JSON.stringify(error?.response?.data) : '', { variant: 'error' });
    if (error?.response?.data)
      enqueueSnackbar('Произошла ошибка при запросе к серверу!   \n\r' + JSON.stringify(error?.response?.data), { variant: 'error' })
    if (error.response && [301, 302, 307].includes(error.response.status)) {
      const redirectUrl = error.response.headers.location;
      const token = localStorage.getItem("accessToken");
      config.headers!.Authorization = `Bearer ${token}`;
      const newConfig = { ...config };
      newConfig.baseURL = '';
      newConfig.url = redirectUrl;
      return api.request(newConfig);
    }
    if (error.response && error.response.status === 401 && !error.config?.url?.includes("/refresh") && !error.config?.url?.includes("/login")) {
      try {
        const refreshToken = localStorage.getItem("refreshToken");
        if (!refreshToken) {
          localStorage.removeItem("accessToken");
          localStorage.removeItem("refreshToken");
          window.location.href = '/sign-in';
          return await Promise.reject(error);
        }
        const accessToken = await updateAccessToken(refreshToken || "");
        if (!accessToken) {
          localStorage.removeItem("accessToken");
          localStorage.removeItem("refreshToken");
          window.location.href = '/sign-in';
          return await Promise.reject(error);
        }
        config.headers!.Authorization = `Bearer ${accessToken}`;
        localStorage.setItem('accessToken', accessToken);
        return api.request(config);
      } catch (refreshError) {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        window.location.href = '/sign-in';
        return await Promise.reject(refreshError);
      }
    }
    return await Promise.reject(error);
  }
);

export default api;
