import { useQuery, UseQueryResult } from 'react-query';

import { OrderReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetOrderById = (id: string) => UseQueryResult<OrderReadModel>;

const useGetOrderById: UseGetOrderById = (id) => {
  const { get } = useAxios();

  return useQuery(['apiV1GetOrderById', id], () => extractData(get(`/orders/${id}`)));
};

export { useGetOrderById };
export type { UseGetOrderById };
