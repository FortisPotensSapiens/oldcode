import { useMutation, UseMutationResult } from 'react-query';

import { OrderSerfDeliveredCreateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostNewPickupOrder = () => UseMutationResult<string, unknown, OrderSerfDeliveredCreateModel, unknown>;

const usePostNewPickupOrder: UsePostNewPickupOrder = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/orders/self-delivered`, data)));
};

export { usePostNewPickupOrder };
export type { UsePostNewPickupOrder };
