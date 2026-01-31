import { useQuery, UseQueryOptions, UseQueryResult } from 'react-query';

import { FilterMerchandiseDetailsModel, MerchandiseReadModelPageResultModel, UseQueryError } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetGoodsByFilter = (
  payload: FilterMerchandiseDetailsModel,
  options?: Omit<UseQueryOptions<MerchandiseReadModelPageResultModel, UseQueryError>, 'queryKey' | 'queryFn'>,
) => UseQueryResult<MerchandiseReadModelPageResultModel, UseQueryError>;

const useGetGoodsByFilter: UseGetGoodsByFilter = (payload, options) => {
  const { post } = useAxios();

  return useQuery(
    ['apiV1GetGoodsByFilter', payload],
    () => extractData(post<MerchandiseReadModelPageResultModel>('/goods/filter', payload)),
    options,
  );
};

export { useGetGoodsByFilter };
export type { UseGetGoodsByFilter };
