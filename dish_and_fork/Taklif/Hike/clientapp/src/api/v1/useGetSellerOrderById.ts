import { useQuery, UseQueryResult } from 'react-query';

import { OrderReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetSellerOrderById = (id: string) => UseQueryResult<OrderReadModel>;

const useGetSellerOrderById: UseGetSellerOrderById = (id: string) => {
  const { get } = useAxios();

  return useQuery(['apiV1GetPartnerById', id], () => extractData(get(`/seller/orders/${id}`)));
};

export { useGetSellerOrderById };
