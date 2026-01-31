import { useMutation, UseMutationResult } from 'react-query';

import { UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { DeliveryOrderModel } from '../../types/swagger';
import { useAxios } from './axios';

type UsePostDeliveryOrder = () => UseMutationResult<string, UseQueryError, DeliveryOrderModel, unknown>;

const usePostDeliveryOrder: UsePostDeliveryOrder = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post('/seller/delivery/order', data)));
};

export { usePostDeliveryOrder };
export type { UsePostDeliveryOrder };
