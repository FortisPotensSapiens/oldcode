import { useQuery, UseQueryResult } from 'react-query';

import { MerchRatingFilterModel, MerchRatingReadModelPageResultModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetProductRatingByFilter = (
  payload: MerchRatingFilterModel,
) => UseQueryResult<MerchRatingReadModelPageResultModel>;

const useGetProductRatingByFilter: UseGetProductRatingByFilter = (payload) => {
  const { post } = useAxios();

  return useQuery(['useGetProductRatingByFilter', payload.pageNumber, payload.pageSize], () =>
    extractData(post<MerchRatingReadModelPageResultModel>('merch-ratings/filter', payload)),
  );
};
export { useGetProductRatingByFilter };
export type { UseGetProductRatingByFilter };
