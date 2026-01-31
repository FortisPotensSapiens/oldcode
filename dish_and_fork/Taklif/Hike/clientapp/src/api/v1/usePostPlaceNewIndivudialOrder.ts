import { useMutation, UseMutationResult } from 'react-query';

import { OrderIndividualCreateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePostPlaceNewIndivudialOrder = () => UseMutationResult<string, unknown, OrderIndividualCreateModel, unknown>;

const usePostPlaceNewIndivudialOrder: UsePostPlaceNewIndivudialOrder = () => {
  const { post } = useAxios();

  return useMutation((data) => extractData(post(`/orders/individual`, data)));
};

export { usePostPlaceNewIndivudialOrder };
export type { UsePostPlaceNewIndivudialOrder };
