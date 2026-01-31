import { useQuery, UseQueryResult } from 'react-query';

import { MerchRatingReadModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UseGetMyProductRating = (id: string) => UseQueryResult<MerchRatingReadModel>;

const useGetMyProductRating: UseGetMyProductRating = (id: string) => {
  const { get } = useAxios();

  return useQuery(['useGetMyProductRating', id], () => extractData(get(`my-assigned-merch-rating/${id}`)));
};

export { useGetMyProductRating };
export type { UseGetMyProductRating };
