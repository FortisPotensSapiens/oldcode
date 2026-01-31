import { useQuery, UseQueryOptions, UseQueryResult } from 'react-query';

import { UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { CalculateDeliveryPriceReadModel, DeliveryNowPriceModel } from '../../types/swagger';
import { useAxios } from './axios';

type UsePostDeliveryStandarOrder = (
  payload: DeliveryNowPriceModel,
  options?: Omit<UseQueryOptions<CalculateDeliveryPriceReadModel, UseQueryError>, 'queryKey' | 'queryFn'>,
) => UseQueryResult<CalculateDeliveryPriceReadModel, UseQueryError>;

const usePostDeliveryStandarOrder: UsePostDeliveryStandarOrder = (payload, options) => {
  const { post } = useAxios();

  return useQuery(
    ['apiV1usePostDeliveryStandarOrder', payload],
    () => extractData(post<CalculateDeliveryPriceReadModel>('/delivery/standart-order/price', payload)),
    options,
  );
};

export { usePostDeliveryStandarOrder };
export type { UsePostDeliveryStandarOrder };
