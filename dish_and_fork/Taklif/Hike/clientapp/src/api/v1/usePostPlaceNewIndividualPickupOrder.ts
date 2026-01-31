import { useMutation, UseMutationResult } from 'react-query';

import { OrderIndividualSelfDeliveredCreateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostPlaceNewIndividualPickupOrder = () => UseMutationResult<
  string,
  unknown,
  OrderIndividualSelfDeliveredCreateModel,
  unknown
>;

const usePostPlaceNewIndividualPickupOrder: UsePostPlaceNewIndividualPickupOrder = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/orders/individual-self-delivered`, data)));
};

export { usePostPlaceNewIndividualPickupOrder };
export type { UsePostPlaceNewIndividualPickupOrder };
