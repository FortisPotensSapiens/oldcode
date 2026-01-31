import { useQuery, UseQueryOptions, UseQueryResult } from 'react-query';

import {
  FilterMerchandiseByPartnerForPartnerDetailsModel,
  MerchandiseReadModelPageResultModel,
  UseQueryError,
} from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetPartnerGoodsByFilter = (
  payload: FilterMerchandiseByPartnerForPartnerDetailsModel,
  options?: Omit<UseQueryOptions<MerchandiseReadModelPageResultModel, UseQueryError>, 'queryKey' | 'queryFn'>,
) => UseQueryResult<MerchandiseReadModelPageResultModel, UseQueryError>;

const useGetPartnerGoodsByFilter: UseGetPartnerGoodsByFilter = (payload, options) => {
  const { post } = useAxios();

  return useQuery(
    ['useGetPartnerGoodsByFilter', payload],
    () => extractData(post<MerchandiseReadModelPageResultModel>('/seller/goods/filter/seller-goods-page', payload)),
    options,
  );
};

export { useGetPartnerGoodsByFilter };
export type { UseGetPartnerGoodsByFilter };
