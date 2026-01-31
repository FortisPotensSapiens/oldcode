import { useQuery, UseQueryResult } from 'react-query';

import { OrderReadModelPageResultModel, PaginationModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetSellerOrdersByFilter = (payload: PaginationModel) => UseQueryResult<OrderReadModelPageResultModel>;

const useGetSellerOrdersByFilter: UseGetSellerOrdersByFilter = (payload) => {
  const { post } = useAxios();

  return useQuery(['apiV1GetOrdersByFilter', payload.pageNumber, payload.pageSize], () =>
    extractData(post<OrderReadModelPageResultModel>('/seller/orders/filter', payload)),
  );
};
export { useGetSellerOrdersByFilter };
export type { UseGetSellerOrdersByFilter };
