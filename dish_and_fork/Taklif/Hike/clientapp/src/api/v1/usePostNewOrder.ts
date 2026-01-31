import { useMutation, UseMutationResult } from 'react-query';

import { CustomCreateOrderModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostNewOrder = () => UseMutationResult<string, unknown, CustomCreateOrderModel, unknown>;

const usePostNewOrder: UsePostNewOrder = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/orders`, data)));
};

export { usePostNewOrder };
export type { UsePostNewOrder };
