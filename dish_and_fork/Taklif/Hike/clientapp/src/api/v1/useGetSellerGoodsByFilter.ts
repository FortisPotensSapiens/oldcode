import { useQuery, UseQueryOptions, UseQueryResult } from 'react-query';

import {
  FilterMerchandiseByPartnerForPartnerDetailsModel,
  MerchandiseReadModelPageResultModel,
  UseQueryError,
} from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetSellerGoodsByFilter = (
  payload: FilterMerchandiseByPartnerForPartnerDetailsModel,
  options?: Omit<UseQueryOptions<MerchandiseReadModelPageResultModel, UseQueryError>, 'queryKey' | 'queryFn'>,
) => UseQueryResult<MerchandiseReadModelPageResultModel, UseQueryError>;

const useGetSellerGoodsByFilter: UseGetSellerGoodsByFilter = (payload, options) => {
  const { post } = useAxios();

  return useQuery(
    ['apiV1GetGoodsByFilter', payload],
    () => extractData(post<MerchandiseReadModelPageResultModel>('/goods/filter/seller-page', payload)),
    options,
  );
};

export { useGetSellerGoodsByFilter };
export type { UseGetSellerGoodsByFilter };
