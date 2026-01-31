import { useQuery, UseQueryOptions, UseQueryResult } from 'react-query';

import { UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { CalculateDeliveryPriceReadModel, DeliveryIndividualPriceModel } from '../../types/swagger';
import { useAxios } from './axios';

type UsePostDeliveryIndividualOrder = (
  payload: DeliveryIndividualPriceModel,
  options?: Omit<UseQueryOptions<CalculateDeliveryPriceReadModel, UseQueryError>, 'queryKey' | 'queryFn'>,
) => UseQueryResult<CalculateDeliveryPriceReadModel, UseQueryError>;

const usePostDeliveryIndividualOrder: UsePostDeliveryIndividualOrder = (payload, options) => {
  const { post } = useAxios();

  return useQuery(
    ['apiV1usePostDeliveryIndividualOrder', payload],
    () => extractData(post<CalculateDeliveryPriceReadModel>('/delivery/individual-order/price', payload)),
    options,
  );
};

export { usePostDeliveryIndividualOrder };
export type { UsePostDeliveryIndividualOrder };
