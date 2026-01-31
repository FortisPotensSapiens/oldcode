import { useQuery, UseQueryOptions, UseQueryResult } from 'react-query';

import { FilterMerchandiseForAdminDetailsModel, MerchandiseReadModelPageResultModel, UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetAdminGoodsByFilter = (
  payload: FilterMerchandiseForAdminDetailsModel,
  options?: Omit<UseQueryOptions<MerchandiseReadModelPageResultModel, UseQueryError>, 'queryKey' | 'queryFn'>,
) => UseQueryResult<MerchandiseReadModelPageResultModel, UseQueryError>;

const useGetAdminGoodsByFilter: UseGetAdminGoodsByFilter = (payload, options) => {
  const { post } = useAxios();

  return useQuery(
    ['useGetAdminGoodsByFilter', payload],
    () => extractData(post<MerchandiseReadModelPageResultModel>('/admin/goods/filter', payload)),
    options,
  );
};

export { useGetAdminGoodsByFilter };
export type { UseGetAdminGoodsByFilter };
