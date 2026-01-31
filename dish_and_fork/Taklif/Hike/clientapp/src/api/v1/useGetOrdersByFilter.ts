import { useQuery, UseQueryResult } from 'react-query';

import { OrderReadModelPageResultModel, PaginationModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetOrdersByFilter = (payload: PaginationModel) => UseQueryResult<OrderReadModelPageResultModel>;

const useGetOrdersByFilter: UseGetOrdersByFilter = (payload) => {
  const { post } = useAxios();

  return useQuery(['apiV1GetOrdersByFilter', payload.pageNumber, payload.pageSize], () =>
    extractData(post<OrderReadModelPageResultModel>('/orders/filter', payload)),
  );
};
export { useGetOrdersByFilter };
export type { UseGetOrdersByFilter };
