import { useMutation, UseMutationResult } from 'react-query';

import { DeviceCreateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostAddDevice = () => UseMutationResult<DeviceCreateModel, unknown, DeviceCreateModel>;

const usePostAddDevice: UsePostAddDevice = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/devices`, data)));
};

export { usePostAddDevice };
export type { UsePostAddDevice };
