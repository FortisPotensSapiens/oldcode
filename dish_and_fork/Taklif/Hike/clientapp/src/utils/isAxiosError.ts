import { AxiosError } from 'axios';

// eslint-disable-next-line @typescript-eslint/no-explicit-any, @typescript-eslint/explicit-module-boundary-types
const isAxiosError = (payload: any): payload is AxiosError =>
  payload && 'isAxiosError' in payload && payload.isAxiosError;

export { isAxiosError };
